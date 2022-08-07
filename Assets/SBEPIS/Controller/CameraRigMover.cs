using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Controller
{
	public class CameraRigMover : MonoBehaviour
	{
		public Transform headTracker;
		public Transform headAnchor;
		public Rigidbody body;

		public Vector3 actualDelta { get; private set; }

		private void FixedUpdate()
		{
			Vector3 delta = headTracker.position - headAnchor.position;
			Vector3 groundDelta = Vector3.ProjectOnPlane(delta, body.transform.up);
			Vector3 oldBodyPosition = body.position;
			body.MovePosition(body.position + groundDelta);
			transform.position -= groundDelta;
			actualDelta = body.position - oldBodyPosition;
		}
	}
}
