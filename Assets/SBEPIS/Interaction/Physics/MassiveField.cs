using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	public class MassiveField : MassiveBody
	{
		public Vector3 gravity = Vector3.down * 9.81f;

		public override Vector3 GetGravity(GravitySum gravityNormalizer)
		{
			return transform.TransformDirection(gravity);
		}
	}
}
