using UnityEngine;

namespace SBEPIS.Utils
{
	public class TransformSync : MonoBehaviour
	{
		private new Rigidbody rigidbody;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		public void SyncPosition(Transform other)
		{
			if (rigidbody)
				rigidbody.MovePosition(other.position);
			else
				transform.position = other.position;
		}

		public void SyncRotation(Transform other)
		{
			if (rigidbody)
				rigidbody.MoveRotation(other.rotation);
			else
				transform.rotation = other.rotation;
		}
	}
}
