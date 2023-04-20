using System;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Controller;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

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
		
		public void OnCapture(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed || !grabber.HeldGrabbable)
				return;
			
			if (grabber.HeldGrabbable.TryGetComponent(out CaptureContainer container) && container.HasCapturedItem)
				RetrieveAndGrabItem(container).Forget();
			else if (grabber.HeldGrabbable.TryGetComponent(out Capturellectable item))
				CaptureAndGrabCard(item).Forget();
		}
		
		public async UniTask<CaptureContainer> CaptureAndGrabCard(Capturellectable item)
		{
			(InventoryStorable card, CaptureContainer container, Capturellectable ejectedItem) = await inventory.Store(item);
			MoveEjectedItem(card, ejectedItem);
			TryGrab(container.transform);
			return container;
		}

		private static void MoveEjectedItem(InventoryStorable card, Capturellectable ejectedItem)
		{
			if (!ejectedItem)
				return;
			
			Action<Vector3, Quaternion> move = ejectedItem.TryGetComponent(out Rigidbody ejectedItemRigidbody) ?
				ejectedItemRigidbody.Move :
				ejectedItem.transform.SetPositionAndRotation;
			if (card.DequeElement.ShouldBeDisplayed)
				move(card.transform.position, card.transform.rotation);
			else
				move(card.DequeElement.Deque.transform.position, card.DequeElement.Deque.transform.rotation);
		}
		
		public UniTask<Capturellectable> RetrieveAndGrabItem(CaptureContainer container) =>
			container.TryGetComponent(out InventoryStorable card) ? RetrieveFromInventory(card) : UniTask.FromResult(RetrieveFromContainer(container));
		
		private Capturellectable RetrieveFromContainer(CaptureContainer container)
		{
			Capturellectable item = container.Fetch();
			TryGrab(item.transform);
			return item;
		}
		
		private async UniTask<Capturellectable> RetrieveFromInventory(InventoryStorable card)
		{
			if (!inventory.CanFetch(card))
				return null;
			
			Capturellectable item = await inventory.Fetch(card);
			TryGrab(item.transform);
			return item;
		}
		
		private void TryGrab(Transform item)
		{
			if (item.TryGetComponent(out Rigidbody itemRigidbody))
				itemRigidbody.Move(grabber.transform.position, grabber.transform.rotation);
			else
				item.SetPositionAndRotation(grabber.transform.position, grabber.transform.rotation);
			if (item.TryGetComponent(out Grabbable itemGrabbable))
				grabber.Grab(itemGrabbable);
			else if (item.TryGetComponentInChildren(out Collider itemCollider))
				grabber.Grab(itemCollider);
		}
	}
}
