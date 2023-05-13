//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	public sealed class FrameSelected
	{
		private Vector2 _Target;
		private Vector2 _TargetRaw;
		private double _CurrentTime;
		private Vector2 _CurrentVelocity = Vector2.zero;

		public float stoppingDistance = 1f;

		public bool isPlaying
		{
			get;
			private set;
		}

		public void Begin(Vector2 frameSelectTarget)
		{
			if (!isPlaying || _TargetRaw != frameSelectTarget)
			{
				_TargetRaw = frameSelectTarget;

				_Target.x = Mathf.Floor(_TargetRaw.x);
				_Target.y = Mathf.Floor(_TargetRaw.y);

				isPlaying = true;
				_CurrentVelocity = Vector2.zero;

				_CurrentTime = EditorApplication.timeSinceStartup;
			}
		}

		public Vector2 Update(Vector2 value, Vector2 offset)
		{
			if (!isPlaying)
			{
				return value;
			}

			Vector2 target = _Target + offset;
			double time = EditorApplication.timeSinceStartup;
			float deltaTime = (float)(time - _CurrentTime);
			_CurrentTime = time;

			value = Vector2.SmoothDamp(value, target, ref _CurrentVelocity, 0.05f, Mathf.Infinity, deltaTime);

			Vector2 v = target - value;
			float distance = v.sqrMagnitude;
			float stoppingDistance = this.stoppingDistance * this.stoppingDistance;

			if (distance <= stoppingDistance || Mathf.Approximately(distance, stoppingDistance))
			{
				value = target;
				isPlaying = false;
			}

			return value;
		}

		public void End()
		{
			isPlaying = false;
		}
	}
}