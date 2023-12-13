using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class Thruster : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private new Rigidbody rigidbody;
		
		[SerializeField]
		private float speed = 100;
		[SerializeField]
		private float acceleration = 50;
		
		private bool launched;
		private float totalSpeed;
		
		private Quaternion initialRotation;
		
		private void FixedUpdate()
		{
			if (launched && totalSpeed < speed)
			{
				rigidbody.AddForce(transform.forward * acceleration, ForceMode.Acceleration);
				totalSpeed += acceleration * Time.fixedDeltaTime;
				rigidbody.MoveRotation(initialRotation);
			}
		}
		
		public void Launch()
		{
			launched = true;
			initialRotation = rigidbody.rotation;
		}
	}
}
