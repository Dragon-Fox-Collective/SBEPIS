using System;
using SBEPIS.Physics;
using SBEPIS.UI;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(LerpTarget))]
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
		
		public LerpTarget lerpTarget { get; private set; }
		public DequePage page { get; private set; }

		public void Awake()
		{
			lerpTarget = GetComponent<LerpTarget>();
			page = GetComponentInParent<DequePage>();
		}

		public void DropTargettingCard()
		{
			if (card.grabbable.isBeingHeld)
				card.grabbable.grabbingGrabber.Drop();
		}

		public void AttachToTarget(LerpTargetAnimator animator)
		{
			page.CreateCardJoint(this);
			card.state.SetBool(DequeStorable.HasBeenAssembled, true);
		}

		public void DetatchFromTarget(LerpTargetAnimator animator)
		{
			page.DestroyCardJoint(this);
			card.state.SetBool(DequeStorable.HasBeenAssembled, false);
		}
	}
}
