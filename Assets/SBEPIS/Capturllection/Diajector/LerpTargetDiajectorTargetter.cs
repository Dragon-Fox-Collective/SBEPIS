using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class LerpTargetDiajectorTargetter : MonoBehaviour
	{
		public Diajector diajector;

		public void TargetDequeOrDiajector(LerpTargetAnimator animator)
		{
			if (diajector.isOpen && diajector.currentPage.HasAnimator(animator))
				animator.TargetTo(diajector.currentPage.GetLerpTargetForAnimator(animator));
			else
				animator.TargetTo(diajector.dequeBox.upperTarget);
		}
	}
}
