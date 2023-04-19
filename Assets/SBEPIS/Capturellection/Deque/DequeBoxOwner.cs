using KBCore.Refs;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturellection
{
	public class DequeBoxOwner : MonoBehaviour
	{
		[SerializeField, Self] private LerpTarget lerpTarget;
		public LerpTarget LerpTarget => lerpTarget;
		
		[SerializeField, Self] private CouplingSocket socket;
		public CouplingSocket Socket => socket;
		
		[SerializeField, Anywhere] private Transform tossTarget;
		public Transform TossTarget => tossTarget;
		
		[Tooltip("Height above the hand the deque should toss through")]
		[SerializeField] private float tossHeight;
		public float TossHeight => tossHeight;
		
		[SerializeField, Anywhere(Flag.Optional)] private DequeBox dequeBox;
		
		private bool IsDequeBoxDeployed => dequeBox && dequeBox.IsDeployed;
		
		private void OnValidate()
		{
			this.ValidateRefs();
			tossHeight = Mathf.Max(tossHeight, 0);
		}
		
		private void Start()
		{
			if (dequeBox)
				dequeBox.RetrieveDeque(this);
		}
		
		public void OnToggleDeque(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed)
				return;
			if (!dequeBox)
				return;
			if (dequeBox.TryGetComponent(out Grabbable grabbable) && grabbable.IsBeingHeld)
				return;
			
			if (IsDequeBoxDeployed)
				dequeBox.RetrieveDeque(this);
			else
				dequeBox.TossDeque(this);
		}
		
		public void SetDequeBox(Grabber grabber, Grabbable grabbable)
		{
			if (!grabbable.TryGetComponent(out DequeBox newDequeBox))
				return;
			
			dequeBox = newDequeBox;
		}
	}
}
