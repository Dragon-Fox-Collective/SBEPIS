using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace SBEPIS.Interaction.Physics
{
	public class GravitySum : MonoBehaviour
	{
		public Transform customCenterOfMass;
		public UnityEvent<Vector3> onGravityChanged = new();

		public Vector3 upDirection { get; private set; } = Vector3.up;
		public float gravityAcceleration { get; private set; } = 0;
		public new Rigidbody rigidbody { get; private set; }
		public Vector3 worldCenterOfMass { get; private set; }

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
			worldCenterOfMass = customCenterOfMass ? customCenterOfMass.position : rigidbody ? rigidbody.worldCenterOfMass : transform.position;
			Vector3 gravity = massiveBodies.Count == 0 ? Vector3.zero : massiveBodies
				.Distinct()
				.GroupBy(body => body.priority)
				.OrderBy(group => group.Key)
				.Aggregate(Vector3.zero, (lowerProrityGravity, group) =>
				{
					List<Vector3> localCentersOfMass = group.Select(body => body.transform.InverseTransformPoint(worldCenterOfMass)).ToList();
					List<float> priorities = group.Zip(localCentersOfMass, (body, localCenterOfMass) => body.GetPriority(localCenterOfMass).Aggregate(1, (product, x) => product * x)).ToList();
					return Vector3.Lerp(lowerProrityGravity,
						group.Zip(localCentersOfMass, (body, localCenterOfMass) => body.transform.TransformDirection(body.GetGravity(localCenterOfMass)))
							.Zip(priorities, (gravity, priority) => Vector3.LerpUnclamped(Vector3.zero, gravity, priority)).Sum(),
						priorities.Sum());
				});

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
