using UnityEngine;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class OrienterJoint : MonoBehaviour
	{
		public float acceleration = 1;
		
		private Vector3 velocity = Vector3.zero;
		
		private Vector3 up = Vector3.up;
		private float timeSinceStanding = 0;
		private const float StandTimeTimeoutThreshold = 1;
		private new Rigidbody rigidbody;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}
		
		public void Orient(Vector3 up)
		{
			if (up != Vector3.zero)
			{
				timeSinceStanding = 0;
				this.up = up.normalized;
			}
		}

		private void FixedUpdate()
		{
			if (timeSinceStanding > StandTimeTimeoutThreshold)
				return;
			timeSinceStanding += Time.fixedDeltaTime;

			Quaternion delta = Quaternion.FromToRotation(transform.up, up);
			if (delta.w < 0) delta = delta.Select(x => -x);
			delta.ToAngleAxis(out float currentDistance, out Vector3 axis);
			currentDistance *= Mathf.Deg2Rad;

			float velocitySign = Vector3.Dot(velocity, axis) > 0 ? -1 : 1;
			float currentVelocity = Vector3.Project(velocity, axis).magnitude * velocitySign;

			float maxPoint = currentDistance - 0.5f * currentVelocity * currentVelocity / -acceleration;
			float criticalPoint = 0.5f * maxPoint;

			bool decelerate = currentDistance < criticalPoint && currentVelocity < 0;
			float accelerationSign = decelerate ? 1 : -1;

			float deltaVelocity = accelerationSign * acceleration * Time.fixedDeltaTime;
			velocity = (currentVelocity + deltaVelocity) * -axis;

			if (currentDistance < velocity.magnitude * Time.fixedDeltaTime && velocity.magnitude < 2 * acceleration * Time.fixedDeltaTime)
				velocity = currentDistance * axis / Time.fixedDeltaTime;

			rigidbody.MoveRotation(Quaternion.Euler(velocity) * rigidbody.rotation);
		}
	}
}
