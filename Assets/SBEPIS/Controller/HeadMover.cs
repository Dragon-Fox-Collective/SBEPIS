using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Controller
{
	public class HeadMover : MonoBehaviour
	{
		public Transform headTracker;

		private void FixedUpdate()
		{
			transform.localPosition = Vector3.up * headTracker.localPosition.y;
			transform.localRotation = Quaternion.Euler(headTracker.localRotation.eulerAngles.x, 0, headTracker.localRotation.eulerAngles.z);
		}
	}
}
