using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class CompoundRigidbody : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private new Rigidbody rigidbody;
		
		public Vector3 WorldCenterOfMass => transform.position + rigidbody.centerOfMass;
		
		private void Start()
		{
			Recalculate();
		}
		
		private void Recalculate()
		{
			RigidbodyPiece[] pieces = GetComponentsInChildren<RigidbodyPiece>();
			if (pieces.Length == 0)
				return;
			
			rigidbody.centerOfMass = Vector3.zero;
			rigidbody.mass = 0;
			rigidbody.inertiaTensor = Vector3.one;
			Matrix4x4 inertiaTensor = new();
			
			foreach (RigidbodyPiece piece in pieces)
			{
				if (piece.gameObject.activeInHierarchy)
				{
					rigidbody.centerOfMass += transform.InverseTransformPoint(piece.WorldCenter) * piece.Mass;
					rigidbody.mass += piece.Mass;
				}
			}
			rigidbody.centerOfMass /= rigidbody.mass;
			
			foreach (RigidbodyPiece piece in pieces)
			{
				// Parallel axis theorem??
				// I' = I + (E (R inner R) - R outer R) m
				// where m is the mass, I is the local inertia tensor, R is the displacement vector from the center of mass to the new point, and E is the identity
				// also inner is dot product
				Matrix4x4 pieceTransform = Matrix4x4.Rotate(piece.transform.rotation);
				Matrix4x4 worldTensor = pieceTransform * piece.LocalInertiaTensor * pieceTransform.transpose;
				Matrix4x4 inverseTransform = Matrix4x4.Rotate(transform.rotation.Inverse());
				Matrix4x4 localTensor = inverseTransform * worldTensor * inverseTransform.transpose;
				Vector3 displacement = WorldCenterOfMass - piece.WorldCenter;
				Matrix4x4 parallelTensor = localTensor.Plus(Matrix4x4.identity.Times(displacement.InnerSquared()).Minus(displacement.OuterSquared()).Times(piece.Mass));
				inertiaTensor = inertiaTensor.Plus(parallelTensor);
			}
			rigidbody.inertiaTensor = inertiaTensor.Diagonalize(out Quaternion inertiaTensorRotation);
			rigidbody.inertiaTensorRotation = inertiaTensorRotation;
			
			rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		}
	}
}
