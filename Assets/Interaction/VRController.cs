using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Input;
using Pose = UnityEngine.XR.OpenXR.Input.Pose;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(PlayerInput))]
	public class VRController : MonoBehaviour
	{
		public Transform head;
		public Transform hand;

		private void OnHeadPosition(InputValue value)
		{
			head.localPosition = value.Get<Vector3>();
		}

		private void OnHeadRotation(InputValue value)
		{
			head.localRotation = value.Get<Quaternion>();
		}

		private void OnHandPose(InputValue value)
		{
			Pose pose = value.Get<Pose>();
			hand.localPosition = pose.position;
			hand.localRotation = pose.rotation;
		}
	}
}