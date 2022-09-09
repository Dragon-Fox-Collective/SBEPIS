using UnityEngine;

namespace SBEPIS.Controller
{
	public class OrienterJoint : MonoBehaviour
	{
		public ConfigurableJoint joint;

		private Vector3 up;

		public void Orient(Vector3 up)
		{
			this.up = up;
		}

		private void FixedUpdate()
		{
			joint.targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, up), Vector3.up);

			//Quaternion delta = Quaternion.FromToRotation(transform.up, up);
			//if (delta.w < 0) delta = delta.Select(x => -x);
			//delta.ToAngleAxis(out float angle, out Vector3 axis);
			//angle *= Mathf.Deg2Rad;
			//joint.targetAngularVelocity = angle / Time.fixedDeltaTime * axis;

			print(joint.targetRotation.eulerAngles);
		}
	}
}
