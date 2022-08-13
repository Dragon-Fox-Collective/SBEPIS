using SBEPIS.Controller;
using SBEPIS.Items;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabber))]
	public class Capturllector : MonoBehaviour
	{
		public Capturllectainer cardPrefab;

		private Grabber grabber;

		private void Awake()
		{
			grabber = GetComponent<Grabber>();
		}

		public void OnCapture(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed || !grabber.heldGrabbable)
				return;

			Capturllectainer card = grabber.heldGrabbable.GetComponent<Capturllectainer>();
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
			Capturllectainer card = Instantiate(cardPrefab.gameObject).GetComponent<Capturllectainer>();
			card.GetComponent<ItemBase>().BecomeItem();
			card.transform.SetPositionAndRotation(grabber.transform.position, grabber.transform.rotation * Quaternion.Euler(0, 180, 0));
			card.Capture(item);
			grabber.Grab(card.GetComponent<Grabbable>());
		}

		public void RetrieveAndGrabItem(Capturllectainer card)
		{
			if (!card.canRetrieve)
				return;

			grabber.Drop();
			Grabbable grabbable = card.Retrieve().GetComponent<Grabbable>();
			Destroy(card.gameObject);
			grabber.Grab(grabbable);
		}
	}
}
