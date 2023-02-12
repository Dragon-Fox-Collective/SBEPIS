using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class LerpTargetUpperDequeTargetter : MonoBehaviour
	{
		public DequeBox deque;

		public void TargetDequeOrDiajector(LerpTargetAnimator animator)
		{
			if (deque.owner.diajector.isOpen && deque.owner.diajector.currentPage.HasAnimator(animator))
				animator.TargetTo(deque.owner.diajector.upperTarget);
			else
				animator.TargetTo(deque.lowerTarget);
		}
	}
}
