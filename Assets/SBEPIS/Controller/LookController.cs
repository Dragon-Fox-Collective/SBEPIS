using UnityEngine;

namespace SBEPIS.Controller
{
	public class LookController : MonoBehaviour
	{
		public Transform pitchRotator;
		public Transform yawRotator;
		public float sensitivity = 1;
		
		public float PitchDrive { private get; set; }
		public float YawDrive { private get; set; }
		public float PitchDelta { private get; set; }
		public float YawDelta { private get; set; }
		
		private float TotalDeltaPitch => PitchDelta * sensitivity + PitchDrive * sensitivity * Time.deltaTime;
		private float TotalDeltaYaw => YawDelta * sensitivity + YawDrive * sensitivity * Time.deltaTime;
		
		private float camPitch;

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
			camPitch = Mathf.Clamp(camPitch - TotalDeltaPitch, -90, 90);
			pitchRotator.transform.localRotation = Quaternion.Euler(Vector3.right * camPitch);
		}

		private void Yaw()
		{
			yawRotator.transform.Rotate(Vector3.up, TotalDeltaYaw);
		}
	}
}
