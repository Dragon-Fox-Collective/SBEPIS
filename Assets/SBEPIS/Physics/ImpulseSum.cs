using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Physics
{
	public class ImpulseSum : MonoBehaviour
	{
		public UnityEvent<Vector3> onImpulseRecieved = new();

		private Vector3 impulse = Vector3.zero;

		private void FixedUpdate()
		{
			//impulse = transform.TransformVector(impulse);
			print($"Summed {impulse}");
			onImpulseRecieved.Invoke(impulse);
			impulse = Vector3.zero;
		}

		private void OnCollisionEnter(Collision collision)
		{
			print($"Entering {gameObject} {collision.collider} {collision.impulse} {collision.relativeVelocity}");
			impulse += collision.impulse;
		}

		private void OnCollisionStay(Collision collision)
		{
			print($"Staying {gameObject} {collision.collider} {collision.impulse} {collision.relativeVelocity}");
			impulse += collision.impulse;
		}

		private void OnCollisionExit(Collision collision)
		{
			print($"Exiting {gameObject} {collision.collider} {collision.impulse} {collision.relativeVelocity}");
			impulse += collision.impulse;
		}
	}
}
