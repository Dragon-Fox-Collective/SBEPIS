using UnityEngine;

namespace SBEPIS.Interaction
{
	public class Damper : MonoBehaviour
	{
		private Vector3 vel;
		private Quaternion deriv;

		private void Update()
		{
			transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref vel, 0.3f);
			transform.localRotation = QuaternionUtil.SmoothDamp(transform.localRotation, Quaternion.identity, ref deriv, 0.2f);
		}
	}
}