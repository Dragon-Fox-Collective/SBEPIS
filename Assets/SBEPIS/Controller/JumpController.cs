using UnityEngine;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Orientation))]
	public class JumpController : MonoBehaviour
	{
		public ConfigurableJoint footballJoint;
		public float jumpSpeed = 3;
		public Vector3 connectedAnchorTarget;

		private Vector3 initialConnectedAnchor;
		private bool isJumping;

		private void Awake()
		{
			initialConnectedAnchor = footballJoint.connectedAnchor;
		}

		private void FixedUpdate()
		{
			if (isJumping)
				MoveFoot();
		}

		public void Jump()
		{
			isJumping = true;
		}

		private void MoveFoot()
		{
			if (footballJoint.connectedAnchor == connectedAnchorTarget)
			{
				isJumping = false;
				footballJoint.connectedAnchor = initialConnectedAnchor;
			}
			else
				footballJoint.connectedAnchor = Vector3.MoveTowards(footballJoint.connectedAnchor, connectedAnchorTarget, jumpSpeed * Time.fixedDeltaTime);
		}
	}
}
