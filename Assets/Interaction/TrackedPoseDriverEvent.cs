using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;
using Pose = UnityEngine.XR.OpenXR.Input.Pose;

namespace SBEPIS.Interaction
{
	public class TrackedPoseDriverEvent : MonoBehaviour
	{
		public void OnPose(CallbackContext context)
		{
			Pose pose = context.ReadValue<Pose>();
			transform.localPosition = pose.position;
			transform.localRotation = pose.rotation;
		}

		public void OnPosition(CallbackContext context)
		{
			transform.localPosition = context.ReadValue<Vector3>();
		}

		public void OnRotation(CallbackContext context)
		{
			transform.localRotation = context.ReadValue<Quaternion>();
		}
	}
}