using System.Linq;
using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	[RequireComponent(typeof(BoxCollider))]
	public class MassiveBox : MassiveBody
	{
		public float gravity = 10;
		public float falloffDistance = 1;

		private BoxCollider box;

		private void Awake()
		{
			box = GetComponent<BoxCollider>();
		}

		public override Vector3 GetPriority(Vector3 centerOfMass)
		{
			Vector3 localCenterOfMass = centerOfMass - box.center;
			return new(
				GetPriorityFalloff(localCenterOfMass.x, box.size.x / 2, falloffDistance),
				localCenterOfMass.y > -box.size.y / 2 ? GetPriorityFalloff(localCenterOfMass.y + box.size.y / 2, box.size.y, falloffDistance) : 0,
				GetPriorityFalloff(localCenterOfMass.z, box.size.z / 2, falloffDistance)
			);
		}

		public static float GetPriorityFalloff(float x, float radius, float falloffDistance)
		{
			return Mathf.Clamp(Mathf.Abs(x).Map(radius, radius - falloffDistance, 0, 1), 0, 1);
		}

		public override Vector3 GetGravity(Vector3 centerOfMass)
		{
			return gravity * Vector3.down;
		}
	}
}
