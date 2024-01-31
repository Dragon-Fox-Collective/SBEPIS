using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Utils
{
	public class LerpTarget : MonoBehaviour
	{
		[FormerlySerializedAs("onMoveFrom")]
		public UnityEvent<LerpTargetAnimator> OnMoveFrom = new();
		[FormerlySerializedAs("onMoveFromTravelUpdate")]
		public UnityEvent<LerpTargetAnimator> OnMoveFromTravelUpdate = new();
		[FormerlySerializedAs("onMoveToTravelUpdate")]
		public UnityEvent<LerpTargetAnimator> OnMoveToTravelUpdate = new();
		[FormerlySerializedAs("onMoveTo")]
		public UnityEvent<LerpTargetAnimator> OnMoveTo = new();
	}
}
