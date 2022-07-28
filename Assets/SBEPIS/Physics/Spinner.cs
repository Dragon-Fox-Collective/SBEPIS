using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class Spinner : MonoBehaviour
	{
		public float speed;

		private new Rigidbody rigidbody;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(Vector3.right * speed));
		}
	}
}
