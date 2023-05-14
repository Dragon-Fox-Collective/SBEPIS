//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree
{
	using Arbor;
	using Arbor.BehaviourTree;

	[CustomEditor(typeof(BehaviourTreeInternal), true)]
	internal sealed class BehaviourTreeInternalInspector : NodeGraphInspector
	{
		protected override void OnCustomGUI()
		{
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("restartOnFinish"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("executionSettings"));
		}
	}
}