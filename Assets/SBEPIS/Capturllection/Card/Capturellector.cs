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
		[FormerlySerializedAs("dequeOwner")]
		public DequeOwner owner;

		private Grabber grabber;

		private void Awake()
		{
			grabber = GetComponent<Grabber>();
		}

		public void OnCapture(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed || !grabber.heldGrabbable)
				return;

			Capturellectainer card = grabber.heldGrabbable.GetComponent<Capturellectainer>();
			if (card && card.capturedItem)
				RetrieveAndGrabItem(card).Forget();
			else
				CaptureAndGrabCard().Forget();
		}

		public async UniTaskVoid CaptureAndGrabCard()
		{
			Grabbable itemGrabbable = grabber.heldGrabbable;
			Capturllectable item = itemGrabbable.GetComponent<Capturllectable>();
			if (!item || !item.canCapturllect)
				return;
			
			(DequeStorable card, Capturellectainer container, Capturllectable ejectedItem) = await owner.inventory.Store(item);
			
			if (ejectedItem)
				if (owner.diajector.ShouldCardBeDisplayed(card))
					ejectedItem.GetComponent<Rigidbody>().Move(card.transform.position, card.transform.rotation);
				else
					ejectedItem.GetComponent<Rigidbody>().Move(owner.dequeBox.transform.position, owner.dequeBox.transform.rotation);
			
			if (container.TryGetComponent(out Grabbable cardGrabbable))
				grabber.Grab(cardGrabbable);
		}

		public async UniTaskVoid RetrieveAndGrabItem(Capturellectainer container)
		{
			if (!container.canFetch)
				return;
			if (!container.TryGetComponent(out DequeStorable card) || !owner.inventory.CanFetch(card))
				return;

			grabber.Drop();
			
			Capturllectable item = await owner.inventory.Fetch(card);
			item.transform.SetPositionAndRotation(grabber.transform.position, grabber.transform.rotation);
			item.GetComponent<Rigidbody>().Move(grabber.transform.position, grabber.transform.rotation);
			
			if (item.TryGetComponent(out Grabbable itemGrabbable))
				grabber.Grab(itemGrabbable);
		}
	}
}
