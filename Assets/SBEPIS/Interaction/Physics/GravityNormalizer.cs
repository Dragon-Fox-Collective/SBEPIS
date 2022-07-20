using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace SBEPIS.Interaction.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class GravityNormalizer : MonoBehaviour
	{
		[NonSerialized]
		public Vector3 upDirection = -UnityEngine.Physics.gravity.normalized;
		[NonSerialized]
		public float gravityAcceleration = UnityEngine.Physics.gravity.magnitude;

		public UnityEvent<Vector3> onGravityChanged = new();

		private new Rigidbody rigidbody;
		private readonly List<MassiveBody> massiveBodies = new();

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			UpdateGravity();
			ApplyGravity();
		}

		private void UpdateGravity()
		{
			Vector3 gravity = Vector3.zero;

			if (massiveBodies.Count > 0)
			{
				IEnumerable<MassiveBody> distinctBodies = massiveBodies.Distinct();
				int highestPriority = distinctBodies.Max(body => body.priority);
				IEnumerable<MassiveBody> applicableBodies = distinctBodies.Where(body => body.priority == highestPriority);

				foreach (MassiveBody body in applicableBodies)
					gravity += body.GetGravity(this);
			}

			gravityAcceleration = gravity.magnitude;
			if (gravityAcceleration > 0)
			{
				Vector3 newUpDirection = -gravity.normalized;
				if (upDirection != newUpDirection)
				{
					upDirection = newUpDirection;
					onGravityChanged.Invoke(upDirection);
				}
			}
		}

		private void ApplyGravity()
		{
			rigidbody.velocity += -gravityAcceleration * Time.fixedDeltaTime * upDirection;
		}

		public void Accumulate(MassiveBody body)
		{
			massiveBodies.Add(body);
		}

		public void Deaccumulate(MassiveBody body)
		{
			massiveBodies.Remove(body);
		}
	}
}
