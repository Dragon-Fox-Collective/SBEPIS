using Cysharp.Threading.Tasks;
using SBEPIS.Controller;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Grabber))]
	public class Capturellector : MonoBehaviour
	{
		public Inventory inventory;
		
		private Grabber grabber;
		
		private void Awake()
		{
			grabber = GetComponent<Grabber>();
		}
		
		public void OnCapture(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed || !grabber.heldGrabbable)
				return;
			
			Capturellectainer container = grabber.heldGrabbable.GetComponent<Capturellectainer>();
			if (container && container.HasCapturedItem)
				RetrieveAndGrabItem(container).Forget();
			else
				CaptureAndGrabCard().Forget();
		}
		
		private async UniTaskVoid CaptureAndGrabCard()
		{
			if (!grabber.heldGrabbable.TryGetComponent(out Capturellectable item))
				return;
			
			(DequeStorable card, Capturellectainer container, Capturellectable ejectedItem) = await inventory.Store(item);
			
			if (ejectedItem)
				if (card.Deque.diajector.ShouldCardBeDisplayed(card))
					ejectedItem.GetComponent<Rigidbody>().Move(card.transform.position, card.transform.rotation);
				else
					ejectedItem.GetComponent<Rigidbody>().Move(card.Deque.transform.position, card.Deque.transform.rotation);
			
			if (container.TryGetComponent(out Grabbable cardGrabbable))
				grabber.Grab(cardGrabbable);
		}
		
		private async UniTaskVoid RetrieveAndGrabItem(Capturellectainer container)
		{
			if (!container.TryGetComponent(out DequeStorable card) || !inventory.CanFetch(card))
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
