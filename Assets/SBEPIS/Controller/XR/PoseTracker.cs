using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Controller
{
	public class PoseTracker : MonoBehaviour
	{
		[SerializeField]
		private InputAction poseInput;
		[SerializeField]
		private UnityEvent<PoseState> onPose;
		
		private void Awake()
		{
			poseInput.performed += OnPose;
			poseInput.canceled += OnPose;
		}
		
		private void OnPose(CallbackContext context) => onPose.Invoke(context.ReadValue<PoseState>());

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
