using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	public class MassiveBody : MonoBehaviour
	{
		public float gravity = 9.81f;
		public float falloff = 2;
		public int priority = 0;

		public bool useX = true;
		public bool useY = true;
		public bool useZ = true;

		public Vector3 GetGravity(GravitySum gravityNormalizer)
		{
			Vector3 localDelta = -transform.InverseTransformPoint(gravityNormalizer.rigidbody.worldCenterOfMass);
			Vector3 toggledLocalDelta = new(useX ? localDelta.x : 0, useY ? localDelta.y : 0, useZ ? localDelta.z : 0);
			Vector3 globalDelta = transform.TransformDirection(toggledLocalDelta);
			return gravity / Mathf.Pow(globalDelta.magnitude, falloff) * globalDelta.normalized;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.attachedRigidbody)
				return;

			GravitySum gravityNormalizer = other.attachedRigidbody.GetComponent<GravitySum>();
			if (gravityNormalizer)
				gravityNormalizer.Accumulate(this);
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.attachedRigidbody)
				return;

			GravitySum gravityNormalizer = other.attachedRigidbody.GetComponent<GravitySum>();
			if (gravityNormalizer)
				gravityNormalizer.Deaccumulate(this);
		}
	}
}
