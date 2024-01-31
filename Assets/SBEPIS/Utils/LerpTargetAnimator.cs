using System;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	public class LerpTargetAnimator : ValidatedMonoBehaviour
	{
		[SerializeField, Self(Flag.Optional)] private new Rigidbody rigidbody;

		[SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
		
		private float time;

		private LerpTarget currentTarget;
		private LerpTarget pausedAtTarget;
		private LerpTarget prevTarget;
		private Vector3 startPosition;
		private Quaternion startRotation;

		private Dictionary<LerpTarget, List<UnityAction<LerpTargetAnimator>>> onMoveToActions = new();
		private UnityAction<LerpTargetAnimator>[] tempOnMoveToActions;
		
		public void TargetTo(LerpTarget target, params UnityAction<LerpTargetAnimator>[] tempActions)
		{
			if (!target) throw new NullReferenceException($"Tried to target {this} to null");
			if (rigidbody) rigidbody.Disable();
			currentTarget = target;
			SetStartPositionAndRotation(transform.position, transform.rotation);
			time = 0;
			tempOnMoveToActions = tempActions;

			if (pausedAtTarget)
			{
				prevTarget = pausedAtTarget;
				pausedAtTarget = null;
				prevTarget.OnMoveFrom.Invoke(this);
			}
		}
		
		public void SetStartPositionAndRotation(Vector3 position, Quaternion rotation)
		{
			startPosition = position;
			startRotation = rotation;
		}
		
		public void TeleportTo(LerpTarget target)
		{
			if (!target) throw new NullReferenceException($"Tried to teleport {this} to null");
			TargetTo(target);
			End();
		}
		
		public void SetPausedAt(LerpTarget target)
		{
			pausedAtTarget = target;
		}
		
		public void Cancel()
		{
			if (!currentTarget) return;
			
			currentTarget = null;
			if (rigidbody) rigidbody.Enable();
		}
		
		private void End()
		{
			transform.SetPositionAndRotation(currentTarget.transform.position, currentTarget.transform.rotation);
			LerpTarget oldTarget = pausedAtTarget = currentTarget;
			Cancel();
			
			oldTarget.OnMoveTo.Invoke(this);
			foreach (UnityAction<LerpTargetAnimator> action in tempOnMoveToActions.ToList())
				action?.Invoke(this);
			
			if (onMoveToActions.TryGetValue(oldTarget, out List<UnityAction<LerpTargetAnimator>> actions))
				foreach (UnityAction<LerpTargetAnimator> action in actions.ToList())
					action?.Invoke(this);
		}
		
		private void Update()
		{
			if (!currentTarget)
				return;
			
			if (prevTarget)
				prevTarget.OnMoveFromTravelUpdate.Invoke(this);
			currentTarget.OnMoveToTravelUpdate.Invoke(this);

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