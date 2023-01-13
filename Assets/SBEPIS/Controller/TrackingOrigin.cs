using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Controller
{
	public class TrackingOrigin : MonoBehaviour
	{
		public Transform headTracker;

		public Rigidbody body;
		public Transform bodyFootPosition;

		private Transform prevHeadTracker;

		private void Start()
		{
			body.position = transform.position + (body.position - bodyFootPosition.position);

			prevHeadTracker = new GameObject("Previous " + headTracker.name).transform;
			prevHeadTracker.parent = headTracker.parent;
			prevHeadTracker.position = headTracker.position;
			prevHeadTracker.rotation = headTracker.rotation;
		}

		private void FixedUpdate()
		{
			if (headTracker.position != prevHeadTracker.position)
			{
				Vector3 groundDelta = Vector3.ProjectOnPlane(headTracker.position - prevHeadTracker.position, body.transform.up);
				body.MovePosition(body.position + groundDelta);
				prevHeadTracker.localPosition = headTracker.localPosition;
			}
		}

		private void Update()
		{
			transform.rotation = body.rotation;
			
			Vector3 footRelativeToTargetOriginPosition = Vector3.ProjectOnPlane(headTracker.position - transform.position, body.transform.up);
			transform.position = bodyFootPosition.position - footRelativeToTargetOriginPosition;
		}
	}
}
