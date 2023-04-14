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
		
		public DequeBox DequeBox { get; private set; }
		
		private bool IsDequeBoxDeployed => DequeBox && DequeBox.IsDeployed;
		
		private void Awake()
		{
			socket.onDecouple.AddListener(SetDequeBoxDecoupledState);
		}
		
		private void Start()
		{
			if (DequeBox)
				DequeBox.RetrieveDeque(this);
		}
		
		public void OnToggleDeque(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed)
				return;
			if (!DequeBox)
				return;
			if (DequeBox.TryGetComponent(out Grabbable grabbable) && grabbable.IsBeingHeld)
				return;
			
			if (IsDequeBoxDeployed)
				DequeBox.RetrieveDeque(this);
			else
				DequeBox.TossDeque(this);
		}
		
		private static void SetDequeBoxDecoupledState(CouplingPlug plug, CouplingSocket socket) => plug.GetComponent<DequeBox>().SetDecoupledState();
		
		public void SetDequeBox(Grabber grabber, Grabbable grabbable)
		{
			if (!grabbable.TryGetComponent(out DequeBox dequeBox))
				return;
			DequeBox = dequeBox;
		}
	}
}
