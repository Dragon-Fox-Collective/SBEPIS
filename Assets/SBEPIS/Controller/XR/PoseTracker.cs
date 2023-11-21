using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;
using Pose = UnityEngine.XR.OpenXR.Input.Pose;

namespace SBEPIS.Controller
{
	public class PoseTracker : MonoBehaviour
	{
		[SerializeField]
		private InputAction poseInput;
		[SerializeField]
		private UnityEvent<Pose> onPose;
		
		private void Awake()
		{
			poseInput.performed += OnPose;
			poseInput.canceled += OnPose;
		}
		
		private void OnPose(CallbackContext context) => onPose.Invoke(context.ReadValue<Pose>());

		public void OnEnable()
		{
			poseInput.Enable();
		}

		public void OnDisable()
		{
			poseInput.Disable();
		}
	}
}
