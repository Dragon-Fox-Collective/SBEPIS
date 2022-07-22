using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	public class MassivePoint : MassiveBody
	{
		public float standardRadius;
		public float gravityAtRadius;

		public override Vector3 GetPriority(Vector3 centerOfMass)
		{
			return Vector3.one;
		}

		public override Vector3 GetGravity(Vector3 centerOfMass)
		{
			float mass = gravityAtRadius * standardRadius * standardRadius;
			return mass / -centerOfMass.sqrMagnitude * centerOfMass.normalized;
		}
	}
}
