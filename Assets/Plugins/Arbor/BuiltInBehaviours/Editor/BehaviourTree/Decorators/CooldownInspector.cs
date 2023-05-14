//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree.Decorators
{
	using Arbor.BehaviourTree.Decorators;
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(Cooldown))]
	internal sealed class CooldownInspector : InspectorBase
	{
		Cooldown _Target;

		protected override void OnRegisterElements()
		{
			_Target = target as Cooldown;

			RegisterProperty("_TimeType");
			RegisterProperty("_Seconds");
			RegisterIMGUI(OnProgressGUI);
		}

		void OnProgressGUI()
		{
			if (Application.isPlaying && _Target.isRevaluation)
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
			return Application.isPlaying && _Target.isRevaluation;
		}
	}
}