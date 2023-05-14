//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree.Actions
{
	using Arbor;
	using Arbor.BehaviourTree.Actions;

	[CustomEditor(typeof(SubStateMachine))]
	internal sealed class SubStateMachineInspector : NodeBehaviourEditor
	{
		[BehaviourMenuItem(typeof(SubStateMachine), "Save To Prefab", localization = true)]
		static void SaveToPrefab(MenuCommand command)
		{
			SubStateMachine behaviour = command.context as SubStateMachine;
			NodeGraph nodeGraph = behaviour.subFSM;

			string path = EditorUtility.SaveFilePanelInProject(Localization.GetWord("Save To Prefab"), nodeGraph.graphName, "prefab", "");
			if (string.IsNullOrEmpty(path))
			{
				return;
			}

			Clipboard.SaveToPrefab(path, nodeGraph);
		}

		private GraphArgumentListEditor _ArgumentListEditor = null;

		private GraphArgumentListEditor argumentListEditor
		{
			get
			{
				if (_ArgumentListEditor == null)
				{
					_ArgumentListEditor = new GraphArgumentListEditor(serializedObject.FindProperty("_ArgumentList"));
				}

				SubStateMachine subGraph = target as SubStateMachine;
				_ArgumentListEditor.nodeGraph = subGraph.subFSM;

				return _ArgumentListEditor;
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			SubStateMachine subStateMachine = target as SubStateMachine;
			NodeGraph nodeGraph = subStateMachine.subFSM;

			if (graphEditor != null && graphEditor.isExternalGraph)
			{
				GUILayout.Button("Open " + nodeGraph.displayGraphName, ArborEditor.BuiltInStyles.largeButton);
			}
			else
			{
				if (EditorGUITools.ButtonForceEnabled("Open " + nodeGraph.displayGraphName, ArborEditor.BuiltInStyles.largeButton))
				{
					if (graphEditor != null)
					{
						var hostWindow = graphEditor.hostWindow;
						hostWindow.ChangeCurrentNodeGraph(target.GetInstanceID());
						GUIUtility.ExitGUI();
					}
				}
			}

			argumentListEditor.DoLayoutList();

			serializedObject.ApplyModifiedProperties();
		}
	}
}