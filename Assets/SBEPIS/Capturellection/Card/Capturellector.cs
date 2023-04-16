using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Controller;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Grabber))]
	public class Capturellector : MonoBehaviour
	{
		[SerializeField, Self]
		private Grabber grabber;
		
		private void OnValidate() => this.ValidateRefs();
		
		public Inventory inventory; // TODO: reference to this
		
		public void OnCapture(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed || !grabber.HeldGrabbable)
				return;
			
			Capturellectainer container = grabber.HeldGrabbable.GetComponent<Capturellectainer>();
			if (container && container.HasCapturedItem)
				RetrieveAndGrabItem(container).Forget();
			else
				CaptureAndGrabCard().Forget();
		}
		
		private async UniTaskVoid CaptureAndGrabCard()
		{
			if (!grabber.HeldGrabbable.TryGetComponent(out Capturellectable item))
				return;
			
			(InventoryStorable card, Capturellectainer container, Capturellectable ejectedItem) = await inventory.Store(item);

			Rigidbody ejectedItemRigidbody = ejectedItem.GetComponent<Rigidbody>();
			if (ejectedItem)
				if (card.DequeElement.Deque.diajector.ShouldCardBeDisplayed(card.DequeElement))
					ejectedItemRigidbody.Move(card.transform.position, card.transform.rotation);
				else
					ejectedItemRigidbody.Move(card.DequeElement.Deque.transform.position, card.DequeElement.Deque.transform.rotation);
			
			if (container.TryGetComponent(out Grabbable cardGrabbable))
				grabber.Grab(cardGrabbable);
		}
		
		private async UniTaskVoid RetrieveAndGrabItem(Capturellectainer container)
		{
			if (!container.TryGetComponent(out InventoryStorable card) || !inventory.CanFetch(card))
				return;
			
			grabber.Drop();
			
			Capturellectable item = await inventory.Fetch(card);
			item.transform.SetPositionAndRotation(grabber.transform.position, grabber.transform.rotation);
			item.GetComponent<Rigidbody>().Move(grabber.transform.position, grabber.transform.rotation);
			
			if (item.TryGetComponent(out Grabbable itemGrabbable))
				grabber.Grab(itemGrabbable);
		}
	}
}
