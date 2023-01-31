using SBEPIS.Physics;
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

		public Transform attachmentTarget;
		public StrengthSettings attachmentStrength;

		private JointTargetter joint;

		private CaptureDeque _deque;
		public CaptureDeque deque
		{
			get => _deque;
			set
			{
				if (_deque == value)
					return;
				
				if (_deque)
					_deque.collisionTrigger.trigger.RemoveListener(StartDiajectorAssembly);
				
				_deque = value;
				_deque.collisionTrigger.trigger.AddListener(StartDiajectorAssembly);
				diajector.deque = _deque;
				CreateJoint();
			}
		}

		private bool dequeDeployed;

		private void Start()
		{
			deque = initialDeque;
		}

		public void OnToggleDeque(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed)
				return;
			if (!diajector.deque)
				return;

			ToggleDiajector();
		}
		
		public void ToggleDiajector()
		{
			if (dequeDeployed)
				RetrieveDeque();
			else
				TossDeque();
		}

		private void TossDeque()
		{
			dequeDeployed = true;
			Destroy(joint);
			
			deque.collisionTrigger.StartPrime();

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
			dequeDeployed = false;
			deque.collisionTrigger.CancelPrime();
			CreateJoint();
			if (diajector.isOpen)
				diajector.StartDisassembly();
		}

		private void CreateJoint()
		{
			if (joint)
				Destroy(joint);
			
			if (deque && !dequeDeployed)
			{
				joint = gameObject.AddComponent<JointTargetter>();
				joint.connectedBody = deque.grabbable.rigidbody;
				joint.target = attachmentTarget;
				joint.strength = attachmentStrength;
			}
		}

		private void StartDiajectorAssembly()
		{
			Vector3 position = deque.transform.position;
			Vector3 upDirection = deque.gravitySum.upDirection;
			Vector3 groundDelta = Vector3.ProjectOnPlane(transform.position - position, upDirection);
			Quaternion rotation = Quaternion.LookRotation(groundDelta, upDirection);
			diajector.StartAssembly(position, rotation);
		}
	}
}
