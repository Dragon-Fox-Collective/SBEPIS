using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturellection
{
	public class DequeBoxOwner : MonoBehaviour
	{
		public LerpTarget lerpTarget;
		public CouplingSocket socket;
		
		public Transform tossTarget;
		[Tooltip("Height above the hand the deque should toss through, must be non-negative")]
		public float tossHeight;
		
		private DequeBox dequeBox;
		
		private bool IsDequeBoxDeployed => dequeBox && dequeBox.IsDeployed;
		
		private void Awake()
		{
			socket.onDecouple.AddListener(SetDequeBoxDecoupledState);
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
		
		private static void SetDequeBoxDecoupledState(CouplingPlug plug, CouplingSocket socket) => plug.GetComponent<DequeBox>().SetDecoupledState();
		
		public void SetDequeBox(Grabber grabber, Grabbable grabbable)
		{
			if (!grabbable.TryGetComponent(out DequeBox newDequeBox))
				return;
			dequeBox = newDequeBox;
		}
	}
}
