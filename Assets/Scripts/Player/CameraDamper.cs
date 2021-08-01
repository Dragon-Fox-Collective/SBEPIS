using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WrightWay.SBEPIS.Util;

namespace WrightWay.SBEPIS.Player
{
	public class CameraDamper : MonoBehaviour
	{
		public new Transform camera;

		private Vector3 cameraVel;
		private Quaternion cameraDeriv;

		private void Update()
		{
			camera.localPosition = Vector3.SmoothDamp(camera.localPosition, Vector3.zero, ref cameraVel, 0.3f);
			camera.localRotation = QuaternionUtil.SmoothDamp(camera.localRotation, Quaternion.identity, ref cameraDeriv, 0.2f);
		}
	}
}