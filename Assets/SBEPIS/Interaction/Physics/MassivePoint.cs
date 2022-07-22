using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	public class MassivePoint : MassiveBody
	{
		public float standardRadius = 10;
		public float gravityAtRadius = 10;

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
