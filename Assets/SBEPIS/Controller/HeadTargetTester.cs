using UnityEngine;

namespace SBEPIS.Controller
{
	public class HeadTargetTester : MonoBehaviour
	{
		public Transform headTarget;
		public Transform targetOrigin;

		public Rigidbody body;
		public Transform bodyHead;
		public Transform bodyFeet;

		private Transform prevHeadTarget;

		private void Start()
		{
			body.position = targetOrigin.position + (body.position - bodyFeet.position);

			prevHeadTarget = new GameObject("Previous " + headTarget.name).transform;
			prevHeadTarget.parent = headTarget.parent;
			prevHeadTarget.position = headTarget.position;
			prevHeadTarget.rotation = headTarget.rotation;
		}

		private void FixedUpdate()
		{
			if (headTarget.position != prevHeadTarget.position)
			{
				Vector3 groundDelta = Vector3.ProjectOnPlane(headTarget.position - prevHeadTarget.position, body.transform.up);
				body.MovePosition(body.position + groundDelta);
				prevHeadTarget.localPosition = headTarget.localPosition;
			}
			
			bodyHead.position = headTarget.position;
		}

		private void Update()
		{
			Vector3 feetRelativeToTargetOriginPosition = Vector3.ProjectOnPlane(headTarget.position - targetOrigin.position, body.transform.up);
			targetOrigin.position = bodyFeet.position - feetRelativeToTargetOriginPosition;
		}
	}
}
