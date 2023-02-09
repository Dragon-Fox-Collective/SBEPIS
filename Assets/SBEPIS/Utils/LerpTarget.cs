using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	public class LerpTarget : MonoBehaviour
	{
		public UnityEvent<LerpTargetAnimator> onMoveFrom = new();
		public UnityEvent<LerpTargetAnimator> onMoveFromTravelUpdate = new();
		public UnityEvent<LerpTargetAnimator> onMoveToTravelUpdate = new();
		public UnityEvent<LerpTargetAnimator> onMoveTo = new();
	}
}
