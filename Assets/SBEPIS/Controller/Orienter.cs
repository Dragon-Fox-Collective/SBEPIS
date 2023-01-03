using UnityEngine;

namespace SBEPIS.Controller
{
	public class Orienter : MonoBehaviour
	{
		public void Orient(Vector3 up)
		{
			if (up == Vector3.zero)
				return;

			up = up.normalized;
			transform.LookAt(transform.position + Vector3.ProjectOnPlane(transform.forward, up), up);
		}
	}
}
