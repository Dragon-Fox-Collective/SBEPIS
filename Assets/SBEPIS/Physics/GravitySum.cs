using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace SBEPIS.Physics
{
	public class GravitySum : MonoBehaviour
	{
		public Transform customCenterOfMass;
		public UnityEvent<Vector3> onGravityChanged = new();

		public Vector3 UpDirection { get; private set; } = Vector3.up;
		public float GravityAcceleration { get; private set; } = 0;
		public Rigidbody Rigidbody { get; private set; }
		public Vector3 WorldCenterOfMass { get; private set; }

		private readonly List<MassiveBody> massiveBodies = new();

		private void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			UpdateGravity();
			ApplyGravity();
		}

		private void UpdateGravity()
		{
			WorldCenterOfMass = customCenterOfMass ? customCenterOfMass.position : Rigidbody ? Rigidbody.worldCenterOfMass : transform.position;
			Vector3 gravity = massiveBodies.Count == 0 ? Vector3.zero : massiveBodies
				.Distinct()
				.GroupBy(body => body.priority)
				.OrderBy(group => group.Key)
				.Aggregate(Vector3.zero, (lowerProrityGravity, group) =>
				{
					List<Vector3> localCentersOfMass = group.Select(body => body.transform.InverseTransformPoint(WorldCenterOfMass)).ToList();
					List<float> priorities = group.Zip(localCentersOfMass, (body, localCenterOfMass) => body.GetPriority(localCenterOfMass).Aggregate(1, (product, x) => product * x)).ToList();
					return Vector3.Lerp(lowerProrityGravity,
						group.Zip(localCentersOfMass, (body, localCenterOfMass) => body.transform.TransformDirection(body.GetGravity(localCenterOfMass)))
							.Zip(priorities, (gravity, priority) => Vector3.LerpUnclamped(Vector3.zero, gravity, priority)).Sum(),
						priorities.Sum());
				});
			
			GravityAcceleration = gravity.magnitude;
			if (GravityAcceleration > 0)
			{
				Vector3 newUpDirection = -gravity.normalized;
				if (UpDirection != newUpDirection)
				{
					UpDirection = newUpDirection;
					onGravityChanged.Invoke(UpDirection);
				}
			}
		}

		private void ApplyGravity()
		{
			if (Rigidbody && !Rigidbody.isKinematic)
				Rigidbody.velocity += -GravityAcceleration * Time.fixedDeltaTime * UpDirection;
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
