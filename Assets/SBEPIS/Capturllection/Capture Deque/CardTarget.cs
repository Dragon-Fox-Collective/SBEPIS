using System;
using SBEPIS.Physics;
using SBEPIS.UI;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(LerpTarget)), RequireComponent(typeof(ElectricArc))]
	public class CardTarget : MonoBehaviour
	{
		public string label;
		
		public UnityEvent<DequeStorable> onCardCreated = new();
		public UnityEvent onPrepareCard = new();
		public UnityEvent onGrab = new();
		public UnityEvent onDrop = new();

		private DequeStorable _card;
		public DequeStorable card
		{
			get => _card;
			set
			{
				_card = value;
				electricArc.otherPoint = _card.transform;
			}
		}
		public bool isTemporary { get; set; }
		public JointTargetter targetter { get; set; }
		public LerpTarget lerpTarget { get; private set; }
		public DequePage page { get; private set; }
		private ElectricArc electricArc;

		public void Awake()
		{
			lerpTarget = GetComponent<LerpTarget>();
			page = GetComponentInParent<DequePage>();
			electricArc = GetComponent<ElectricArc>();
		}

		public void DropTargettingCard()
		{
			if (card.grabbable.isBeingHeld)
				card.grabbable.grabbingGrabber.Drop();
		}

		public void CreateCardJoint(LerpTargetAnimator animator)
		{
			page.CreateCardJoint(this);
		}

		public void DestroyCardJoint(LerpTargetAnimator animator)
		{
			page.DestroyCardJoint(this);
		}
	}
}
