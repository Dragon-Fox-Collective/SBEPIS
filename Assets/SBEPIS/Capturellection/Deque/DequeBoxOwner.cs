using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(CouplingSocket), typeof(LerpTarget), typeof(DequeOwner))]
	public class DequeBoxOwner : MonoBehaviour
	{
		public Transform tossTarget;
		[Tooltip("Height above the hand the deque should toss through, must be non-negative")]
		public float tossHeight;
		
		public AnimationCurve retrievalAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
		
		public Deque Deque => DequeOwner.Deque;
		public DequeBox DequeBox { get; private set; }
		
		public DequeOwner DequeOwner { get; private set; }
		public CouplingSocket Socket { get; private set; }
		public LerpTarget LerpTarget { get; private set; }
		
		public LerpTargetAnimator DequeAnimator { get; private set; }
		
		private bool IsDequeBoxDeployed => DequeBox && DequeBox.IsDeployed;
		
		private void Awake()
		{
			DequeOwner = GetComponent<DequeOwner>();
			Socket = GetComponent<CouplingSocket>();
			LerpTarget = GetComponent<LerpTarget>();
			
			Socket.onDecouple.AddListener(SetDequeBoxDecoupledState);
		}
		
		private void Start()
		{
			if (DequeBox)
				DequeBox.RetrieveDeque();
		}
		
		public void OnToggleDeque(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed)
				return;
			if (!DequeBox)
				return;
			
			if (DequeBox.TryGetComponent(out Grabbable grabbable) && grabbable.isBeingHeld)
				DequeBox.CloseDiajector();
			else if (IsDequeBoxDeployed)
				DequeBox.RetrieveDeque();
			else
				DequeBox.TossDeque();
		}
		
		private static void SetDequeBoxDecoupledState(CouplingPlug plug, CouplingSocket socket) => plug.GetComponent<DequeBox>().SetDecoupledState();
		
		public void SetDequeBox(DequeOwner dequeOwner, Deque deque)
		{
			DequeBox = deque.GetComponent<DequeBox>();
			if (!DequeBox)
			{
				Debug.LogError($"{dequeOwner} with {this} got a new deque {deque} that's not a DequeBox");
				return;
			}
			
			DequeBox.dequeBoxOwner = this;
			
			DequeAnimator = DequeBox.gameObject.AddComponent<LerpTargetAnimator>();
			DequeAnimator.curve = retrievalAnimationCurve;
		}

		public void UnsetDequeBox(DequeOwner dequeOwner, Deque deque)
		{
			DequeBox.dequeBoxOwner = null;
			DequeBox = null;
			
			Destroy(DequeAnimator);
		}
	}
}
