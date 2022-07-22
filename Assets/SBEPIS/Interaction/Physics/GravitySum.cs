using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using SBEPIS.Interaction.Controller;

namespace SBEPIS.Interaction.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class GravitySum : MonoBehaviour
	{
		public Transform customCenterOfMass;
		public UnityEvent<Vector3> onGravityChanged = new();

		[NonSerialized]
		public Vector3 upDirection = -UnityEngine.Physics.gravity.normalized;
		[NonSerialized]
		public float gravityAcceleration = UnityEngine.Physics.gravity.magnitude;

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
			Vector3 gravity = massiveBodies.Count == 0 ? Vector3.zero :
				massiveBodies
					.Distinct()
					.GroupBy(body => body.priority)
					.OrderBy(group => group.Key)
					.Aggregate(Vector3.zero, (lowerProrityGravity, group) => 
						group.Aggregate(Vector3.zero, (thisPriorityGravity, body) =>
							thisPriorityGravity + body.GetGravity(customCenterOfMass ? customCenterOfMass.position : rigidbody.worldCenterOfMass, lowerProrityGravity / group.Count())));

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
