using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(CouplingSocket), typeof(LerpTarget), typeof(DequeStorage))]
	public class DequeOwner : MonoBehaviour
	{
		[SerializeField]
		private DequeBox initialDeque;
		public Diajector diajector;
		
		public Transform tossTarget;
		[Tooltip("Height above the hand the deque should toss through, must be non-negative")]
		public float tossHeight;
		
		public AnimationCurve retrievalAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
		
		public CouplingSocket socket { get; private set; }
		public LerpTarget lerpTarget { get; private set; }
		public DequeStorage storage { get; private set; }
		
		private LerpTargetAnimator animator;
		
		private bool isDequeDeployed => !socket.isCoupled;

		private DequeBox _dequeBox;
		public DequeBox dequeBox
		{
			get => _dequeBox;
			set
			{
				if (_dequeBox == value)
					return;
				
				if (_dequeBox)
					FinishUpOldDeque();
				
				if (value)
				{
					_dequeBox = value;
					SetupNewDeque();
				}
				else
				{
					UnsetDeque();
					_dequeBox = null;
				}
			}
		}
		
		private void Awake()
		{
			socket = GetComponent<CouplingSocket>();
			lerpTarget = GetComponent<LerpTarget>();
			storage = GetComponent<DequeStorage>();
		}
		
		private void Start()
		{
			socket.onDecouple.AddListener(CheckForPriming);
			socket.onCouple.AddListener(CancelPriming);
			socket.onCouple.AddListener(CloseDiajector);
			
			dequeBox = initialDeque;
		}
		
		private void FinishUpOldDeque()
		{
			dequeBox.owner = null;
			dequeBox.collisionTrigger.trigger.RemoveListener(StartDiajectorAssembly);
			dequeBox.grabbable.onDrop.RemoveListener(CheckForPriming);
			dequeBox.grabbable.onGrab.RemoveListener(CancelPriming);
			dequeBox.grabbable.onUse.RemoveListener(CloseDiajector);

			storage.definition = null;
			
			Destroy(animator);
			
			if (!isDequeDeployed)
			{
				socket.Decouple(dequeBox.plug);
				dequeBox.transform.position += dequeBox.transform.forward * 0.1f;
			}
		}
		
		private void SetupNewDeque()
		{
			dequeBox.owner = this;
			dequeBox.collisionTrigger.trigger.AddListener(StartDiajectorAssembly);
			dequeBox.grabbable.onDrop.AddListener(CheckForPriming);
			dequeBox.grabbable.onGrab.AddListener(CancelPriming);
			dequeBox.grabbable.onUse.AddListener(CloseDiajector);

			storage.definition = dequeBox.definition;
			
			animator = dequeBox.gameObject.AddComponent<LerpTargetAnimator>();
			animator.curve = retrievalAnimationCurve;
			if (!diajector.isOpen)
				animator.TargetTo(lerpTarget);
			
			diajector.dequeBox = dequeBox;
			if (diajector.isOpen)
				diajector.RefreshPage();
		}
		
		private void UnsetDeque()
		{
			diajector.dequeBox = null;
			if (diajector.isOpen)
				diajector.ForceClose(_dequeBox.lowerTarget);
		}
		
		public void OnToggleDeque(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed)
				return;
			if (!diajector.dequeBox)
				return;
			
			if (isDequeDeployed)
				RetrieveDeque();
			else
				TossDeque();
		}
		
		private void TossDeque()
		{
			socket.Decouple(dequeBox.plug);

			Vector3 upDirection = tossTarget.up;
			
			float gravityMag = -dequeBox.gravitySum.gravityAcceleration;
			float startHeight = tossTarget.InverseTransformPoint(dequeBox.transform.position).y;
			float upTossSpeed = CalcTossYVelocity(gravityMag, startHeight, startHeight + tossHeight);
			Vector3 upwardVelocity = upDirection * upTossSpeed;

			float timeToHit = (-upTossSpeed - Mathf.Sqrt(upTossSpeed * upTossSpeed - 2 * gravityMag * startHeight)) / gravityMag;
			Vector3 groundDelta = Vector3.ProjectOnPlane(tossTarget.position - dequeBox.transform.position, upDirection);
			Vector3 groundVelocity = groundDelta / timeToHit;

			dequeBox.gravitySum.rigidbody.velocity = upwardVelocity + groundVelocity;
		}
		
		private static float CalcTossYVelocity(float gravity, float startHeight, float peakHeight) => Mathf.Sqrt(2 * gravity * (startHeight - peakHeight));
		
		private void RetrieveDeque()
		{
			animator.TargetTo(lerpTarget);
		}
		
		private void StartDiajectorAssembly()
		{
			Vector3 position = dequeBox.transform.position;
			Vector3 upDirection = dequeBox.gravitySum.upDirection;
			Vector3 groundDelta = Vector3.ProjectOnPlane(transform.position - position, upDirection);
			Quaternion rotation = Quaternion.LookRotation(groundDelta, upDirection);
			diajector.StartAssembly(position, rotation);
		}
		
		private void CheckForPriming(Grabber grabber, Grabbable grabbable) => CheckForPriming();
		private void CheckForPriming(CouplingPlug plug, CouplingSocket socket) => CheckForPriming();
		private void CheckForPriming()
		{
			if (dequeBox.grabbable.isBeingHeld || dequeBox.plug.isCoupled || diajector.isOpen)
				return;
			
			dequeBox.collisionTrigger.StartPrime();
		}
		
		private void CancelPriming(Grabber grabber, Grabbable grabbable) => CancelPriming();
		private void CancelPriming(CouplingPlug plug, CouplingSocket socket) => CancelPriming();
		private void CancelPriming()
		{
			dequeBox.collisionTrigger.CancelPrime();
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
