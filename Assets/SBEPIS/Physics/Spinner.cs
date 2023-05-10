using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class Spinner : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private new Rigidbody rigidbody;
		
		public float speed;
		
		private void FixedUpdate()
		{
			rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(Vector3.right * speed));
		}
	}
}
