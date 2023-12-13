using UnityEngine;

namespace SBEPIS.Physics
{
	public class MassivePoint : MassiveBody
	{
		[SerializeField]
		private float standardRadius = 10;
		[SerializeField]
		private float gravityAtRadius = 10;

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
