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
			Grabbable grabbable = grabber.heldGrabbable;
			Capturllectable item = grabbable.GetComponent<Capturllectable>();
			if (!item || !item.canCapturllect)
				return;

			grabber.Drop();
			Capturellectainer card = Instantiate(cardPrefab);
			card.GetComponent<ItemBase>().BecomeItem();
			ResetCardTransform(card);
			card.Capture(item);

			Grabbable cardGrabbable = card.GetComponent<Grabbable>();
			if (cardGrabbable)
				grabber.Grab(cardGrabbable);
			DequeStorable dequeStorable = card.GetComponent<DequeStorable>();
			if (dequeStorable)
				dequeOwner.diajector.captureLayout.AddPermanentTarget(dequeStorable);
		}

		public void RetrieveAndGrabItem(Capturellectainer card)
		{
			if (!card.canRetrieve)
				return;

			grabber.Drop();
			ResetCardTransform(card);
			Grabbable grabbable = card.Retrieve().GetComponent<Grabbable>();
			Destroy(card.gameObject);
			grabber.Grab(grabbable);
		}

		private void ResetCardTransform(Capturellectainer card)
		{
			card.transform.SetPositionAndRotation(grabber.transform.position, grabber.transform.rotation * Quaternion.Euler(0, 180, 0));
		}
	}
}
