using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class LerpTargetStartPositionSync : MonoBehaviour
	{
		public LerpTarget otherTarget;
		
		public void SyncStartTransformTravellingToHere(LerpTargetAnimator animator)
		{
			animator.SetStartPositionAndRotation(otherTarget.transform.position, otherTarget.transform.rotation);
		}

		public void SyncStartTransformTravellingFromHere(LerpTargetAnimator animator)
		{
			animator.SetStartPositionAndRotation(transform.position, transform.rotation);
		}
	}
}
