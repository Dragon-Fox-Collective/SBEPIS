using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;
using Pose = UnityEngine.XR.OpenXR.Input.Pose;

namespace SBEPIS.Interaction.Controller.XR
{
	public class TrackedPoseDriverEvent : MonoBehaviour
	{
		public Vector3 positionOffset = Vector3.zero;
		public Quaternion rotationOffset = Quaternion.identity;

		public void OnPose(CallbackContext context)
		{
			Pose pose = context.ReadValue<Pose>();
			transform.localPosition = pose.position + positionOffset;
			transform.localRotation = pose.rotation * rotationOffset;
		}

		public void OnPosition(CallbackContext context)
		{
			transform.localPosition = context.ReadValue<Vector3>() + positionOffset;
		}

		public void OnRotation(CallbackContext context)
		{
			transform.localRotation = context.ReadValue<Quaternion>() * rotationOffset;
		}
	}
}