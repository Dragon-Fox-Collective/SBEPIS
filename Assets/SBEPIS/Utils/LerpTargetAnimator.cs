using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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

		private Dictionary<LerpTarget, List<UnityAction<LerpTargetAnimator>>> onMoveToActions = new();

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

		public void SetPausedAt(LerpTarget target)
		{
			pausedAtTarget = target;
		}

		private void End()
		{
			transform.SetPositionAndRotation(currentTarget.transform.position, currentTarget.transform.rotation);
			rigidbody.Enable();
			LerpTarget oldTarget = pausedAtTarget = currentTarget;
			currentTarget = null;
			
			oldTarget.onMoveTo.Invoke(this);
			if (onMoveToActions.ContainsKey(oldTarget))
				foreach (UnityAction<LerpTargetAnimator> action in onMoveToActions[oldTarget].ToList())
					action.Invoke(this);
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
		
		public void AddListenerOnMoveTo(LerpTarget target, UnityAction<LerpTargetAnimator> func)
		{
			if (!onMoveToActions.ContainsKey(target))
				onMoveToActions[target] = new List<UnityAction<LerpTargetAnimator>>();
			
			onMoveToActions[target].Add(func);
		}

		public void RemoveListenerOnMoveTo(LerpTarget target, UnityAction<LerpTargetAnimator> func)
		{
			if (!onMoveToActions.ContainsKey(target))
				return;

			onMoveToActions[target].Remove(func);
		}
	}
}