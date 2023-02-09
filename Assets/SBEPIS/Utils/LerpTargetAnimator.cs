using UnityEngine;

namespace SBEPIS.Utils
{
	public class LerpTargetAnimator : MonoBehaviour
	{
		public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		private new Rigidbody rigidbody;

		private float time;
		
		public LerpTarget currentTarget { get; private set; }
		private LerpTarget pausedAtTarget;
		private LerpTarget prevTarget;
		private Vector3 startPosition;
		private Quaternion startRotation;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}
		
		public void TargetTo(LerpTarget target)
		{
			rigidbody.Disable();
			currentTarget = target;
			SetStartPositionAndRotation(transform.position, transform.rotation);
			time = 0;

			if (pausedAtTarget)
			{
				prevTarget = pausedAtTarget;
				pausedAtTarget = null;
				prevTarget.onMoveFrom.Invoke(this);
			}
		}

		public void SetStartPositionAndRotation(Vector3 position, Quaternion rotation)
		{
			startPosition = position;
			startRotation = rotation;
		}
		
		public void TeleportTo(LerpTarget target)
		{
			TargetTo(target);
			End();
		}

		private void End()
		{
			transform.SetPositionAndRotation(currentTarget.transform.position, currentTarget.transform.rotation);
			rigidbody.Enable();
			pausedAtTarget = currentTarget;
			currentTarget = null;
			
			pausedAtTarget.onMoveTo.Invoke(this);
		}
		
		private void Update()
		{
			if (!currentTarget)
				return;
			
			if (prevTarget)
				prevTarget.onMoveFromTravelUpdate.Invoke(this);
			currentTarget.onMoveToTravelUpdate.Invoke(this);

			time += Time.deltaTime;
			if (time < curve[curve.length - 1].time)
			{
				float evaluation = curve.Evaluate(time);
				transform.SetPositionAndRotation(
					Vector3.Lerp(startPosition, currentTarget.transform.position, evaluation),
					Quaternion.Lerp(startRotation, currentTarget.transform.rotation, evaluation));
			}
			else
			{
				End();
			}
		}
	}
}