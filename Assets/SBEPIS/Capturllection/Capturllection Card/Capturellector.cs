using SBEPIS.Controller;
using SBEPIS.Items;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabber))]
	public class Capturellector : MonoBehaviour
	{
		public DequeOwner dequeOwner;
		public Capturellectainer cardPrefab;

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
			Capturellectainer container = Instantiate(cardPrefab);
			container.GetComponent<ItemBase>().BecomeItem();
			ResetCardTransform(container);
			container.Capture(item);

			Grabbable cardGrabbable = container.GetComponent<Grabbable>();
			if (cardGrabbable)
				grabber.Grab(cardGrabbable);
			
			DequeStorable card = container.GetComponent<DequeStorable>();
			if (card)
			{
				dequeOwner.storage.StoreCard(card);
				if (dequeOwner.diajector.captureLayout && dequeOwner.diajector.captureLayout.isActiveAndEnabled)
					dequeOwner.diajector.captureLayout.AddPermanentTargetAtTable(card);
				else if (cardGrabbable && cardGrabbable.isBeingHeld)
					dequeOwner.dequeBox.AddHeldTemporaryTarget(card, cardGrabbable);
				else
					dequeOwner.dequeBox.AddTemporaryTarget(card);
			}
		}

		public void RetrieveAndGrabItem(Capturellectainer card)
		{
			if (!card.canRetrieve)
				return;

			grabber.Drop();
			ResetCardTransform(card);
			Grabbable grabbable = card.Fetch().GetComponent<Grabbable>();
			Destroy(card.gameObject);
			grabber.Grab(grabbable);
		}

		private void ResetCardTransform(Capturellectainer card)
		{
			card.transform.SetPositionAndRotation(grabber.transform.position, grabber.transform.rotation * Quaternion.Euler(0, 180, 0));
		}
	}
}
