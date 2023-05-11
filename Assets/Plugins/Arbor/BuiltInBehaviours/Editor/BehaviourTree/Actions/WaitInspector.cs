//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree.Actions
{
	using Arbor.BehaviourTree.Actions;
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(Wait))]
	internal sealed class WaitInspector : InspectorBase
	{
		Wait _Target;

		protected override void OnRegisterElements()
		{
			_Target = target as Wait;

			RegisterProperty("_TimeType");
			RegisterProperty("_Seconds");
			RegisterIMGUI(OnProgressGUI);
		}

		void OnProgressGUI()
		{
			if (Application.isPlaying && _Target.treeNode.isActive)
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
			return Application.isPlaying && _Target.treeNode.isActive;
		}
	}
}
