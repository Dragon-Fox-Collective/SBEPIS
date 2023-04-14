using System;
using SBEPIS.Physics;
using SBEPIS.UI;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(LerpTarget))]
	public class CardTarget : MonoBehaviour
	{
		[FormerlySerializedAs("onCardCreated")]
		public UnityEvent<DequeStorable> onCardBound = new();
		public UnityEvent onPrepareCard = new();
		public UnityEvent onGrab = new();
		public UnityEvent onDrop = new();
		
		public DequeStorable Card { get; set; }
		
		public LerpTarget LerpTarget { get; private set; }

		public void Awake()
		{
			LerpTarget = GetComponent<LerpTarget>();
		}

		public void DropTargettingCard()
		{
			Card.Grabbable.Drop();
		}

		public void AttachToTarget(LerpTargetAnimator animator)
		{
			Card.State.HasBeenAssembled = true;
		}

		public void DetatchFromTarget(LerpTargetAnimator animator)
		{
			Card.State.HasBeenAssembled = false;
		}
	}
}
