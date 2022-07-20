using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	public class MassiveBody : MonoBehaviour
	{
		public int priority = 0;

		[SerializeField]
		private Vector3 gravity = Vector3.down * 9.81f;

		public Vector3 GetGravity(GravityNormalizer gravityNormalizer)
		{
			return transform.TransformDirection(gravity);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.attachedRigidbody)
				return;

			GravityNormalizer gravityNormalizer = other.attachedRigidbody.GetComponent<GravityNormalizer>();
			if (gravityNormalizer)
				gravityNormalizer.Accumulate(this);
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.attachedRigidbody)
				return;

			GravityNormalizer gravityNormalizer = other.attachedRigidbody.GetComponent<GravityNormalizer>();
			if (gravityNormalizer)
				gravityNormalizer.Deaccumulate(this);
		}
	}
}
