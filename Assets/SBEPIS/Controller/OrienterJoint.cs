using UnityEngine;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class OrienterJoint : MonoBehaviour
	{
		private Vector3 up;
		private new Rigidbody rigidbody;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		public void Orient(Vector3 up)
		{
			this.up = up;
		}

		private void FixedUpdate()
		{
			//if (up != Vector3.zero)
				//transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, up), up);

			Quaternion delta = Quaternion.FromToRotation(transform.up, up);
			if (delta.w < 0) delta = delta.Select(x => -x);
			delta.ToAngleAxis(out float angle, out Vector3 axis);
			angle *= Mathf.Deg2Rad;
			rigidbody.angularVelocity = angle / Time.fixedDeltaTime * axis;

		}
	}
}
