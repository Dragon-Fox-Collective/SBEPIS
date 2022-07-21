using SBEPIS.Interaction.Physics;
using UnityEngine;

namespace SBEPIS.Interaction.Controller
{
	[RequireComponent(typeof(Rigidbody), typeof(GravitySum))]
	public class GroundDetector : MonoBehaviour
	{
		public Transform groundCheck;
		public float groundCheckDistance = 0.3f;
		public LayerMask groundCheckMask;

		private new Rigidbody rigidbody;
		private GravitySum gravityNormalizer;
		private readonly Collider[] groundedColliders = new Collider[1];
		private float delayTimeLeft;

		public bool isGrounded { get; private set; }
		public Collider groundCollider { get; private set; }
		public Rigidbody groundRigidbody => groundCollider ? groundCollider.attachedRigidbody : null;

		public Vector3 relativeVelocity => groundRigidbody ? rigidbody.velocity - groundRigidbody.velocity : rigidbody.velocity;
		public Vector3 groundVelocity => Vector3.ProjectOnPlane(relativeVelocity, gravityNormalizer.upDirection);
		public Vector3 verticalVelocity => Vector3.Project(relativeVelocity, gravityNormalizer.upDirection);
		public Vector3 upDirection => gravityNormalizer.upDirection;
		public bool isFalling => Vector3.Dot(verticalVelocity, upDirection) < 0;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			gravityNormalizer = GetComponent<GravitySum>();
		}

		private void FixedUpdate()
		{
			if (delayTimeLeft > 0)
				delayTimeLeft -= Time.fixedDeltaTime;
			isGrounded = delayTimeLeft <= 0 && UnityEngine.Physics.OverlapSphereNonAlloc(groundCheck.position, groundCheckDistance, groundedColliders, groundCheckMask, QueryTriggerInteraction.Ignore) > 0;
			groundCollider = isGrounded ? groundedColliders[0] : null;
		}

		public void Delay(float time)
		{
			delayTimeLeft = Mathf.Max(delayTimeLeft, time);
		}
	}
}
