using Cysharp.Threading.Tasks;
using SBEPIS.Controller;
using UnityEngine;
using UnityEngine.Serialization;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabber))]
	public class Capturellector : MonoBehaviour
	{
		[FormerlySerializedAs("owner")]
		public DequeOwner dequeOwner;
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
			
			(Card card, Capturellectainer container, Capturellectable ejectedItem) = await inventory.Store(item);
			
			if (ejectedItem)
				if (dequeOwner.diajector.ShouldCardBeDisplayed(card))
					ejectedItem.GetComponent<Rigidbody>().Move(card.transform.position, card.transform.rotation);
				else
					ejectedItem.GetComponent<Rigidbody>().Move(dequeOwner.Deque.transform.position, dequeOwner.Deque.transform.rotation);
			
			if (container.TryGetComponent(out Grabbable cardGrabbable))
				grabber.Grab(cardGrabbable);
		}
		
		private async UniTaskVoid RetrieveAndGrabItem(Capturellectainer container)
		{
			if (!container.TryGetComponent(out Card card) || !inventory.CanFetch(card))
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
