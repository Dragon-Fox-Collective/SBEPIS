using KBCore.Refs;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(BoxCollider))]
	public class MassiveBox : MassiveBody
	{
		[SerializeField, Self] private BoxCollider box;
		private void OnValidate() => this.ValidateRefs();
		
		public float gravity = 10;
		public float falloffDistance = 1;
		
		public override Vector3 GetPriority(Vector3 centerOfMass)
		{
			Vector3 localCenterOfMass = centerOfMass - box.center;
			return new(
				GetPriorityFalloff(localCenterOfMass.x, box.size.x / 2, falloffDistance),
				localCenterOfMass.y > -box.size.y / 2 ? GetPriorityFalloff(localCenterOfMass.y + box.size.y / 2, box.size.y, falloffDistance) : 0,
				GetPriorityFalloff(localCenterOfMass.z, box.size.z / 2, falloffDistance)
			);
		}
		
		public static float GetPriorityFalloff(float x, float radius, float falloffDistance) =>
			falloffDistance > 0 ? Mathf.Clamp(Mathf.Abs(x).Map(radius, radius - falloffDistance, 0, 1), 0, 1) : Mathf.Abs(x) < radius ? 1 : 0;

		public override Vector3 GetGravity(Vector3 centerOfMass) => gravity * Vector3.down;
	}
}
