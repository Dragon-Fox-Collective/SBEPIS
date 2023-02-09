using System;
using SBEPIS.Physics;
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

		public void CreateCardJoint(LerpTargetAnimator animator)
		{
			if (animator.gameObject != card.gameObject)
				return;
			
			page.CreateCardJoint(this);
		}

		public void DestroyCardJoint(LerpTargetAnimator animator)
		{
			if (animator.gameObject != card.gameObject)
				return;
			
			page.DestroyCardJoint(this);
		}
	}
}
