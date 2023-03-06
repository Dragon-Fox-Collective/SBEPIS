using SBEPIS.Controller;
using SBEPIS.Items;
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
				RetrieveAndGrabItem(card);
			else
				CaptureAndGrabCard();
		}

		public void CaptureAndGrabCard()
		{
			Grabbable itemGrabbable = grabber.heldGrabbable;
			Capturllectable item = itemGrabbable.GetComponent<Capturllectable>();
			if (!item || !item.canCapturllect)
				return;
			
			grabber.Drop();
			(DequeStorable card, Capturellectainer container) = owner.dequeBox.inventory.Store(item, out Capturllectable ejectedItem);
			
			if (ejectedItem)
				if (owner.diajector.ShouldCardBeDisplayed(card))
					ejectedItem.GetComponent<Rigidbody>().Move(card.transform.position, card.transform.rotation);
				else
					ejectedItem.GetComponent<Rigidbody>().Move(owner.dequeBox.transform.position, owner.dequeBox.transform.rotation);

			if (container.TryGetComponent(out Grabbable cardGrabbable))
				grabber.Grab(cardGrabbable);
		}

		public void RetrieveAndGrabItem(Capturellectainer container)
		{
			if (!container.canFetch)
				return;
			if (!container.TryGetComponent(out DequeStorable card) || !owner.dequeBox.inventory.CanFetch(card))
				return;

			grabber.Drop();
			Capturllectable item = owner.dequeBox.inventory.Fetch(card);
			item.transform.SetPositionAndRotation(grabber.transform.position, grabber.transform.rotation);
			item.GetComponent<Rigidbody>().Move(grabber.transform.position, grabber.transform.rotation);
			
			if (item.TryGetComponent(out Grabbable itemGrabbable))
				grabber.Grab(itemGrabbable);
		}
	}
}
