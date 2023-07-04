using UnityEngine;

namespace SBEPIS.Physics
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
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color32(145, 244, 139, 210);
			if (standardRadius > 0)
				Gizmos.DrawWireSphere(transform.position, standardRadius);
		}
	}
}
