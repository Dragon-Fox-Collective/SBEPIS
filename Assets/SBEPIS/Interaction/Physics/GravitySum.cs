using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace SBEPIS.Interaction.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class GravitySum : MonoBehaviour
	{
		public Transform customCenterOfMass;
		public UnityEvent<Vector3> onGravityChanged = new();

		[NonSerialized]
		public Vector3 upDirection = Vector3.up;
		[NonSerialized]
		public float gravityAcceleration = 0;

		public new Rigidbody rigidbody { get; private set; }
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
			Vector3 centerOfMass = customCenterOfMass ? customCenterOfMass.position : rigidbody.worldCenterOfMass;
			Vector3 gravity = massiveBodies.Count == 0 ? Vector3.zero : massiveBodies
				.Distinct()
				.GroupBy(body => body.priority)
				.OrderBy(group => group.Key)
				.Aggregate(Vector3.zero, (lowerProrityGravity, group) => 
					group.Sum(body =>
					{
						Vector3 localCenterOfMass = body.transform.InverseTransformPoint(centerOfMass);
						return Vector3.Lerp(
							lowerProrityGravity / group.Count(),
							body.transform.TransformDirection(body.GetGravity(localCenterOfMass)),
							body.GetPriority(localCenterOfMass).Aggregate(1, (product, x) => product * x));
					}));

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
