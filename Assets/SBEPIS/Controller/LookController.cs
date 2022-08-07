using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Controller
{
	public class LookController : MonoBehaviour
	{
		public Transform pitchRotator;
		public Transform yawRotator;
		public float sensitivity = 1;

		private float pitchControl;
		private float yawControl;
		private float camPitch;

		public float deltaYaw => yawControl * sensitivity * Time.deltaTime;
		public float deltaPitch => pitchControl * sensitivity * Time.deltaTime;

		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void Update()
		{
			RotateCamera();
		}

		private void RotateCamera()
		{
			Pitch();
			Yaw();
		}

		private void Pitch()
		{
			float pitch = deltaPitch;
			camPitch = Mathf.Clamp(camPitch - pitch, -90, 90);

			pitchRotator.transform.localRotation = Quaternion.Euler(Vector3.right * camPitch);
		}

		private void Yaw()
		{
			float yaw = deltaYaw;

			yawRotator.transform.Rotate(Vector3.up, yaw);
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
