using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class Thruster : MonoBehaviour
	{
		public float speed = 100;
		public float acceleration = 50;

		private bool launched;
		private float totalSpeed;

		private Quaternion initialRotation;

		private new Rigidbody rigidbody;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

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
