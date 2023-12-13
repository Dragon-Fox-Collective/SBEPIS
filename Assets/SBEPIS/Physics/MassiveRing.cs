using UnityEngine;

namespace SBEPIS.Physics
{
	public class MassiveRing : MassiveBody
	{
		[SerializeField]
		private float gravity = 10;
		[SerializeField]
		private float standingRadius = 1;
		[SerializeField]
		private float falloffRadius = 2;
		[SerializeField]
		private float width = 1;
		[SerializeField]
		private float falloffDistance = 1;

		public override Vector3 GetPriority(Vector3 centerOfMass)
		{
			Vector3 ringCenterOfMass = new(0, centerOfMass.y, centerOfMass.z);
			float distance = ringCenterOfMass.magnitude;
			float radiusPriority = falloffRadius > standingRadius ?
				distance > standingRadius ? MassiveBox.GetPriorityFalloff(distance - standingRadius, falloffRadius - standingRadius, falloffDistance) : 0 :
				distance < standingRadius ? MassiveBox.GetPriorityFalloff(standingRadius - distance, standingRadius - falloffRadius, falloffDistance) : 0;
			Vector3 priority = new(
				MassiveBox.GetPriorityFalloff(centerOfMass.x, width / 2, falloffDistance),
				radiusPriority,
				1
			);
			return priority;
		}

		public override Vector3 GetGravity(Vector3 centerOfMass)
		{
			return -gravity * Mathf.Sign(falloffRadius - standingRadius) * new Vector3(0, centerOfMass.y, centerOfMass.z).normalized;
		}
	}
}
