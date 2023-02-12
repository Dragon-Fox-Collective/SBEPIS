using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class LerpTargetLowerDequeTargetter : MonoBehaviour
	{
		public LerpTarget upperTarget;
		
		public void ActivateAnimator(LerpTargetAnimator animator)
		{
			animator.gameObject.SetActive(true);
		}
		
		public void DeactivateAnimator(LerpTargetAnimator animator)
		{
			animator.gameObject.SetActive(false);
		}
		
		public void SyncStartTransformTravellingToHere(LerpTargetAnimator animator)
		{
			animator.SetStartPositionAndRotation(upperTarget.transform.position, upperTarget.transform.rotation);
		}

		public void SyncStartTransformTravellingFromHere(LerpTargetAnimator animator)
		{
			animator.SetStartPositionAndRotation(transform.position, transform.rotation);
		}
	}
}
