using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction.Controller
{
	public class CameraRigMover : MonoBehaviour
	{
		public Transform cameraRig;
		public Transform headTracker;
		public Transform headAnchor;
		public Rigidbody body;
		public Orientation orientation;

		private void FixedUpdate()
		{
			Vector3 delta = headTracker.position - headAnchor.position;
			Vector3 groundDelta = Vector3.ProjectOnPlane(delta, orientation.upDirection);
			body.MovePosition(body.position + groundDelta);
			cameraRig.position -= groundDelta;
		}
	}
}
