using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	public abstract class MassiveBody : MonoBehaviour
	{
		public int priority = 0;

		public abstract Vector3 GetGravity(GravitySum gravityNormalizer);

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
