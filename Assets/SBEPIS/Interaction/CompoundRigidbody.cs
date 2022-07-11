using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Rigidbody))]
	public class CompoundRigidbody : MonoBehaviour
	{
		public new Rigidbody rigidbody { get; private set; }

		public Vector3 WorldCenterOfMass => transform.position + rigidbody.centerOfMass;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			Recalculate();
		}

		public void Recalculate()
		{
			RigidbodyPiece[] pieces = GetComponentsInChildren<RigidbodyPiece>();
			if (pieces.Length == 0)
				return;

			rigidbody.centerOfMass = Vector3.zero;
			rigidbody.mass = 0;
			//rigidbody.inertiaTensor = Vector3.zero; // Gotta be nonzero :rolling_eyes:
			Matrix4x4 inertiaTensor = new Matrix4x4();

			foreach (RigidbodyPiece piece in pieces)
			{
				if (piece.gameObject.activeInHierarchy)
				{
					rigidbody.centerOfMass += (piece.WorldCenter - transform.position) * piece.mass;
					rigidbody.mass += piece.mass;
				}
			}
			rigidbody.centerOfMass /= rigidbody.mass;

			foreach (RigidbodyPiece piece in pieces)
			{
				// Parallel axis theorem??
				// I' = I + m ((R dot R) E - R outer R)
				// where m is the mass, I is the local inertia tensor, R is the displacement vector of the center of mass perpendicular to I, and E is the identity
				Matrix4x4 localTensor = piece.LocalInertiaTensor; // * Matrix4x4.Rotate(piece.transform.rotation * Quaternion.Inverse(transform.rotation)); // FIXME: ???
				Vector3 displacement = piece.WorldCenter - WorldCenterOfMass;
				inertiaTensor = inertiaTensor.AddedBy(localTensor.AddedBy(Matrix4x4.identity.ScaledBy(Vector3.Dot(displacement, displacement)).SubtractedBy(displacement.OuterSquared()).ScaledBy(piece.mass)));
			}
			rigidbody.inertiaTensor = inertiaTensor.Diagonalize(out Quaternion inertiaTensorRotation);
			rigidbody.inertiaTensorRotation = inertiaTensorRotation;
		}
	}
}
