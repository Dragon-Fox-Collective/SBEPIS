using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class OrienterJoint : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private new Rigidbody rigidbody;
		
		public float acceleration = 1;
		
		private Vector3 velocity = Vector3.zero;
		
		private Vector3 up = Vector3.up;
		private float timeSinceStanding = 0;
		private const float StandTimeTimeoutThreshold = 1;
		
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

			Vector3 axis = Quaternion.FromToRotation(transform.up, up).ToEulersAngleAxis();
			float currentDistance = axis.magnitude;
			axis = axis.normalized;

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
