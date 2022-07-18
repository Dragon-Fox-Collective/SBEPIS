using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction
{
	public class LookController : MonoBehaviour
	{
		public Transform pitchRotator;
		public Transform yawRotator;
		public float sensitivity = 1;

		private float pitchControl;
		private float yawControl;
		private float camPitch;
		private float camYaw;

		private void Update()
		{
			RotateCamera();
		}

		private void RotateCamera()
		{
			float pitch = pitchControl * sensitivity;
			camPitch = Mathf.Clamp(camPitch - pitch, -90, 90);

			float yaw = yawControl * sensitivity;
			camYaw += yaw;

			Vector3 localRotation = pitchRotator.transform.localRotation.eulerAngles;
			localRotation.x = camPitch;
			localRotation.y = camYaw;
			pitchRotator.transform.localRotation = Quaternion.Euler(localRotation);
		}
		public void OnLookPitch(CallbackContext context)
		{
			pitchControl = context.ReadValue<float>();
		}

		public void OnLookYaw(CallbackContext context)
		{
			yawControl = context.ReadValue<float>();
		}
	}
}
