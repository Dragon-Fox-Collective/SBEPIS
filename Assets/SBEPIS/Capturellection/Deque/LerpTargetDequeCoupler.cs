using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class LerpTargetDequeCoupler : MonoBehaviour
	{
		public void Couple(LerpTargetAnimator animator)
		{
			DequeBox dequeBox = animator.GetComponent<DequeBox>();
			dequeBox.SetCoupledState();
		}
	}
}