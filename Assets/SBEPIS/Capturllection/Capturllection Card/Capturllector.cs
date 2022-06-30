using SBEPIS.Interaction;
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
			if (!context.performed || !grabber.heldGrabbable)
				return;

			Capturllectainer card = grabber.heldGrabbable.GetComponent<Capturllectainer>();
			if (card)
				RetrieveAndGrabItem(card);
			else
				CaptureAndGrabCard();
		}

		public void CaptureAndGrabCard()
		{
			Grabbable grabbable = grabber.heldGrabbable;
			Item item = grabbable.GetComponent<Item>();
			if (!item)
				return;

			grabber.Release();
			Capturllectainer card = Instantiate(cardPrefab.gameObject).GetComponent<Capturllectainer>();
			card.GetComponent<ItemBase>().BecomeItem();
			card.transform.SetPositionAndRotation(grabber.transform.position, grabber.transform.rotation);
			card.Capture(item);
			grabber.Grab(card.GetComponent<Grabbable>());
		}

		public void RetrieveAndGrabItem(Capturllectainer card)
		{
			grabber.Release();
			Grabbable grabbable = card.Retrieve().GetComponent<Grabbable>();
			Destroy(card.gameObject);
			grabber.Grab(grabbable);
		}
	}
}
