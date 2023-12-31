using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace SBEPIS.Utils
{
	public class TransformSync : ValidatedMonoBehaviour
	{
		[SerializeField, Self(Flag.Optional)]
		private new Rigidbody rigidbody;
		
		public Vector3 positionOffset = Vector3.zero;
		public Quaternion rotationOffset = Quaternion.identity;
		
		// ---- Main syncs
		public void SyncLocalPosition(Vector3 localPosition)
		{
			localPosition += positionOffset;
			if (rigidbody)
				rigidbody.MovePosition(transform.parent.TransformPoint(localPosition));
			else
				transform.localPosition = localPosition;
		}

		public void SyncLocalRotation(Quaternion localRotation)
		{
			localRotation *= rotationOffset;
			if (rigidbody)
				rigidbody.MoveRotation(transform.parent.rotation * localRotation);
			else
				transform.localRotation = localRotation;
		}

		public void SyncPosition(Vector3 position)
		{
			SyncLocalPosition(transform.parent.InverseTransformPoint(position));
		}

		public void SyncRotation(Quaternion rotation)
		{
			SyncLocalRotation(transform.parent.rotation.Inverse() * rotation);
		}
		
		public void SyncPose(PoseState pose)
		{
			SyncLocalPosition(pose.position);
			SyncLocalRotation(pose.rotation);
			SyncVelocity(pose.velocity);
			SyncAngularVelocity(pose.angularVelocity);
		}
		
		// ---- Transform syncs
		public void SyncTransform(Transform transform)
		{
			SyncPosition(transform.position);
			SyncRotation(transform.rotation);
		}

		public void SyncPosition(Transform transform)
		{
			SyncPosition(transform.position);
		}

		public void SyncRotation(Transform transform)
		{
			SyncRotation(transform.rotation);
		}

		public void SyncLocalTransform(Transform transform)
		{
			SyncLocalPosition(transform.localPosition);
			SyncLocalRotation(transform.localRotation);
		}

		public void SyncLocalPosition(Transform transform)
		{
			SyncLocalPosition(transform.localPosition);
		}

		public void SyncLocalRotation(Transform transform)
		{
			SyncLocalRotation(transform.localRotation);
		}

		// ---- Velocity syncs
		public void SyncVelocity(Vector3 velocity)
		{
			if (rigidbody)
				rigidbody.velocity = velocity;
		}

		public void SyncAngularVelocity(Vector3 angularVelocity)
		{
			if (rigidbody)
				rigidbody.angularVelocity = angularVelocity;
		}
	}
}
