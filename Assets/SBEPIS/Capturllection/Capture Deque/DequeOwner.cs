using SBEPIS.Controller;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturllection
{
	public class DequeOwner : MonoBehaviour
	{
		[SerializeField]
		private CaptureDeque initialDeque;
		public Diajector diajector;

		public Transform tossTarget;
		[Tooltip("Height above the hand the deque should toss through, must be non-negative")]
		public float tossHeight;

		public CouplingSocket socket;
		
		private bool isDequeDeployed => !socket.isCoupled;

		private CaptureDeque _deque;
		public CaptureDeque deque
		{
			get => _deque;
			set
			{
				if (_deque == value)
					return;

				if (_deque)
					FinishUpOldDeque();

				if (value)
				{
					_deque = value;
					SetupNewDeque();
				}
				else
				{
					UnsetDeque();
					_deque = null;
				}
			}
		}

		private void Start()
		{
			socket.onDecouple.AddListener(CheckForPriming);
			socket.onCouple.AddListener(CancelPriming);
			socket.onCouple.AddListener(CloseDiajector);
			
			deque = initialDeque;
		}

		private void FinishUpOldDeque()
		{
			deque.collisionTrigger.trigger.RemoveListener(StartDiajectorAssembly);
			deque.grabbable.onDrop.RemoveListener(CheckForPriming);
			deque.grabbable.onGrab.RemoveListener(CancelPriming);
			deque.grabbable.onUse.RemoveListener(CloseDiajector);

			if (!isDequeDeployed)
			{
				socket.Decouple(deque.plug);
				deque.transform.position += deque.transform.forward * 0.1f;
			}
		}
		
		private void SetupNewDeque()
		{
			deque.collisionTrigger.trigger.AddListener(StartDiajectorAssembly);
			deque.grabbable.onDrop.AddListener(CheckForPriming);
			deque.grabbable.onGrab.AddListener(CancelPriming);
			deque.grabbable.onUse.AddListener(CloseDiajector);
			
			if (!diajector.isOpen)
				socket.Couple(deque.plug);

			diajector.deque = deque;
			if (diajector.isOpen)
				diajector.RefreshPage();
		}

		private void UnsetDeque()
		{
			diajector.deque = null;
			if (diajector.isOpen)
				diajector.ForceClose();
		}

		public void OnToggleDeque(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed)
				return;
			if (!diajector.deque)
				return;

			if (isDequeDeployed)
				RetrieveDeque();
			else
				TossDeque();
		}

		private void TossDeque()
		{
			socket.Decouple(deque.plug);

			Vector3 upDirection = tossTarget.up;
			
			float gravityMag = -deque.gravitySum.gravityAcceleration;
			float startHeight = tossTarget.InverseTransformPoint(deque.transform.position).y;
			float upTossSpeed = CalcTossYVelocity(gravityMag, startHeight, startHeight + tossHeight);
			Vector3 upwardVelocity = upDirection * upTossSpeed;

			float timeToHit = (-upTossSpeed - Mathf.Sqrt(upTossSpeed * upTossSpeed - 2 * gravityMag * startHeight)) / gravityMag;
			Vector3 groundDelta = Vector3.ProjectOnPlane(tossTarget.position - deque.transform.position, upDirection);
			Vector3 groundVelocity = groundDelta / timeToHit;

			deque.gravitySum.rigidbody.velocity = upwardVelocity + groundVelocity;
		}
		
		private static float CalcTossYVelocity(float gravity, float startHeight, float peakHeight) => Mathf.Sqrt(2 * gravity * (startHeight - peakHeight));

		private void RetrieveDeque()
		{
			socket.Couple(deque.plug);
		}

		private void StartDiajectorAssembly()
		{
			Vector3 position = deque.transform.position;
			Vector3 upDirection = deque.gravitySum.upDirection;
			Vector3 groundDelta = Vector3.ProjectOnPlane(transform.position - position, upDirection);
			Quaternion rotation = Quaternion.LookRotation(groundDelta, upDirection);
			diajector.StartAssembly(position, rotation);
		}
	
		private void CheckForPriming(Grabber grabber, Grabbable grabbable) => CheckForPriming();
		private void CheckForPriming(CouplingPlug plug, CouplingSocket socket) => CheckForPriming();
		private void CheckForPriming()
		{
			if (deque.grabbable.isBeingHeld || deque.plug.isCoupled || diajector.isOpen)
				return;
			
			deque.collisionTrigger.StartPrime();
		}
		
		private void CancelPriming(Grabber grabber, Grabbable grabbable) => CancelPriming();
		private void CancelPriming(CouplingPlug plug, CouplingSocket socket) => CancelPriming();
		private void CancelPriming()
		{
			deque.collisionTrigger.CancelPrime();
		}
		
		private void CloseDiajector(Grabber grabber, Grabbable grabbable) => CloseDiajector();
		private void CloseDiajector(CouplingPlug plug, CouplingSocket socket) => CloseDiajector();
		private void CloseDiajector()
		{
			if (diajector.isOpen)
				diajector.StartDisassembly();
		}
	}
}
