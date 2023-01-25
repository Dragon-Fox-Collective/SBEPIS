using SBEPIS.Physics;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class CardTarget : MonoBehaviour
	{
		public string label;
		
		public UnityEvent<DequeStorable> onCardCreated = new();
		public UnityEvent onPrepareCard = new();
		public UnityEvent onGrab = new();
		public UnityEvent onDrop = new();

		public DequeStorable card { get; set; }
		public bool isTemporary { get; set; }
		public JointTargetter targetter { get; set; }

		public void DropTargettingCard()
		{
			if (card.grabbable.isBeingHeld)
				card.grabbable.grabbingGrabber.Drop();
		}
	}
}
