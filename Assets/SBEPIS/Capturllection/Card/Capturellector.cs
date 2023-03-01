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
			(DequeStorable card, Capturellectainer container) = owner.storage.StoreItem(item);
			ResetCardTransform(container);
			
			// put card in right state
			
			if (container.TryGetComponent(out Grabbable cardGrabbable))
				grabber.Grab(cardGrabbable);
		}

		public void RetrieveAndGrabItem(Capturellectainer container)
		{
			if (!container.canFetch)
				return;
			if (!container.TryGetComponent(out DequeStorable card) || !owner.storage.CanFetch(card))
				return;

			grabber.Drop();
			ResetCardTransform(container);
			Capturllectable item = owner.storage.FetchItem(card, container);
			
			// put card in right state
			
			if (item.TryGetComponent(out Grabbable itemGrabbable))
				grabber.Grab(itemGrabbable);
		}

		private void ResetCardTransform(Capturellectainer card)
		{
			card.transform.SetPositionAndRotation(grabber.transform.position, grabber.transform.rotation * Quaternion.Euler(0, 180, 0));
		}
	}
}
