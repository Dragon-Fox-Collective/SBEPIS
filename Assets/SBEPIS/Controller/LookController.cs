using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Controller
{
	public class LookController : MonoBehaviour
	{
		public Transform pitchRotator;
		public Transform yawRotator;
		public float sensitivity = 1;
		
		public float PitchControl { private get; set; }
		public float YawControl { private get; set; }
		private float camPitch;

		private float DeltaPitch => PitchControl * sensitivity * Time.deltaTime;
		private float DeltaYaw => YawControl * sensitivity * Time.deltaTime;

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
			camPitch = Mathf.Clamp(camPitch - DeltaPitch, -90, 90);
			pitchRotator.transform.localRotation = Quaternion.Euler(Vector3.right * camPitch);
			PitchControl = 0;
		}

		private void Yaw()
		{
			yawRotator.transform.Rotate(Vector3.up, DeltaYaw);
			YawControl = 0;
		}
	}
}
