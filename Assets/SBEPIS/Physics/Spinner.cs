using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class Spinner : MonoBehaviour
	{
		[SerializeField, Self]
		private new Rigidbody rigidbody;
		
		private void OnValidate() => this.ValidateRefs();
		
		public float speed;
		
		private void FixedUpdate()
		{
			rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(Vector3.right * speed));
		}
	}
}
