//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(TimeTransition))]
	internal sealed class TimeTransitionInspector : InspectorBase
	{
		TimeTransition _Target;

		protected override void OnRegisterElements()
		{
			_Target = target as TimeTransition;

			RegisterProperty("_TimeType");
			RegisterProperty("_Seconds");
			RegisterIMGUI(OnProgressGUI);
		}

		void OnProgressGUI()
		{
			if (Application.isPlaying && _Target.stateMachine.currentState == _Target.state)
			{
				var schedulerProgress = _Target.schedulerProgress;
				if (schedulerProgress != null)
				{
					Rect r = EditorGUILayout.BeginVertical();
					EditorGUI.ProgressBar(r, schedulerProgress.progress, schedulerProgress.ToString());
					GUILayout.Space(16);
					EditorGUILayout.EndVertical();
				}
			}
		}

		public override bool RequiresConstantRepaint()
		{
			return Application.isPlaying && _Target.stateMachine.currentState == _Target.state;
		}
	}
}
