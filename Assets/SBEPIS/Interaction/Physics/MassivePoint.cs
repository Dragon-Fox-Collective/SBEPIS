using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	public class MassivePoint : MassiveBody
	{
		public float gravity = 9.81f;
		public float falloff = 2;

		public override Vector3 GetGravity(GravitySum gravityNormalizer)
		{
			Vector3 delta = transform.position - gravityNormalizer.rigidbody.worldCenterOfMass;
			return gravity / Mathf.Pow(delta.magnitude, falloff) * delta.normalized;
		}
	}
}
