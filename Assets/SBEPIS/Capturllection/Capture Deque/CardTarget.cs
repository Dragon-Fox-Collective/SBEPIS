using SBEPIS.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class CardTarget : MonoBehaviour
	{
		public UnityEvent<DequeStorable> onCardCreated = new();
		public UnityEvent onPrepareCard = new();
		public UnityEvent<CaptureDeque> onGrab = new(), onDrop = new();

		public DequeStorable card { get; set; }
		public bool isTemporary { get; set; }
		public JointTargetter targetter { get; set; }

		public DequeCardInfo dequeCardInfo { get; set; }

		public void DropTargettingCard()
		{
			if (card.grabbable.isBeingHeld)
				card.grabbable.grabbingGrabber.Drop();
		}
	}
}
