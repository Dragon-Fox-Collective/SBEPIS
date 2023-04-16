using KBCore.Refs;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;
using Pose = UnityEngine.XR.OpenXR.Input.Pose;

namespace SBEPIS.Utils
{
	public class TransformSync : MonoBehaviour
	{
		[SerializeField, Self(Flag.Optional)]
		private new Rigidbody rigidbody;
		
		private void OnValidate() => this.ValidateRefs();
		
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

		// ---- Input syncs
		public void SyncPose(CallbackContext context)
		{
			Pose pose = context.ReadValue<Pose>();
			SyncLocalPosition(pose.position);
			SyncLocalRotation(pose.rotation);
			SyncVelocity(pose.velocity);
			SyncAngularVelocity(pose.angularVelocity);
		}

		public void SyncPosition(CallbackContext context)
		{
			SyncLocalPosition(context.ReadValue<Vector3>());
		}

		public void SyncRotation(CallbackContext context)
		{
			SyncLocalRotation(context.ReadValue<Quaternion>());
		}
	}
}
