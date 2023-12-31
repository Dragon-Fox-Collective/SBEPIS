using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Capturellection.Storage;
using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Grabber))]
	public class Capturellector : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private Grabber grabber;
		
		[SerializeField, Anywhere(Flag.Optional)] private Inventory inventory;
		public Inventory Inventory
		{
			get => inventory;
			set => inventory = value;
		}
		
		public void Capture()
		{
			if (!isActiveAndEnabled || !grabber.HeldGrabbable)
				return;
			
			if (grabber.HeldGrabbable.TryGetComponent(out InventoryStorable card))
				RetrieveFromInventory(card).Forget();
			else if (grabber.HeldGrabbable.TryGetComponent(out CaptureContainer container))
				RetrieveFromContainer(container);
			else if (grabber.HeldGrabbable.TryGetComponent(out Capturellectable item))
				CaptureAndGrabCard(item).Forget();
		}
		
		public async UniTask<CaptureContainer> CaptureAndGrabCard(Capturellectable item)
		{
			if (item.IsBeingCaptured)
				return null;
			
			StoreResult result = await inventory.StoreItem(item);
			if (!result.WasSuccessful) return null;
			
			if (!result.Card.gameObject.activeSelf) result.Card.gameObject.SetActive(true);
			TryGrab(result.Card.transform);
			return result.Container;
		}
		
		private Capturellectable RetrieveFromContainer(CaptureContainer container)
		{
			if (!container.CanBeFetchedByCapturellectors) return null;
			Capturellectable item = container.Fetch();
			if (item) TryGrab(item.transform);
			return item;
		}
		
		public async UniTask<Capturellectable> RetrieveFromInventory(InventoryStorable card)
		{
			if (!inventory.CanFetch(card) || card.IsBeingFetched)
				return null;
			
			FetchResult result = await inventory.FetchItem(card);
			if (!result.WasSuccessful) return null;
			
			if (result.FetchedItem) TryGrab(result.FetchedItem.transform);
			return result.FetchedItem;
		}
		
		private void TryGrab(Transform item)
		{
			item.SetPositionAndRotation(grabber.transform.position, grabber.transform.rotation);
			
			if (item.TryGetComponent(out Grabbable itemGrabbable))
				grabber.GrabManually(itemGrabbable, true);
			else if (item.TryGetComponentInChildren(out Collider itemCollider))
				grabber.GrabManually(itemCollider, true);
		}
	}
}
