//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree.Actions
{
	using Arbor;
	using Arbor.BehaviourTree;
	using Arbor.BehaviourTree.Actions;

	[CustomEditor(typeof(SubBehaviourTree))]
	internal sealed class SubBehaviourTreeInspector : NodeBehaviourEditor
	{
		[BehaviourMenuItem(typeof(SubBehaviourTree), "Save To Prefab", localization = true)]
		static void SaveToPrefab(MenuCommand command)
		{
			SubBehaviourTree behaviour = command.context as SubBehaviourTree;
			NodeGraph nodeGraph = behaviour.subBT;

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

				SubBehaviourTree subGraph = target as SubBehaviourTree;
				_ArgumentListEditor.nodeGraph = subGraph.subBT;

				return _ArgumentListEditor;
			}
		}

		SerializedObject _BehaviourTreeObject;

		SerializedObject GetBehaviourTreeObject()
		{
			if (_BehaviourTreeObject == null)
			{
				SubBehaviourTree subBehaviourTree = target as SubBehaviourTree;
				BehaviourTree subBT = subBehaviourTree.subBT;
				if (subBT != null)
				{
					_BehaviourTreeObject = new SerializedObject(subBT);
				}
			}
			return _BehaviourTreeObject;
		}

		void OnBehaviourTreeGUI()
		{
			SerializedObject btObject = GetBehaviourTreeObject();
			if (btObject == null)
			{
				return;
			}
			btObject.Update();

			EditorGUILayout.PropertyField(btObject.FindProperty("executionSettings"));

			btObject.ApplyModifiedProperties();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			SubBehaviourTree subBehaviourTree = target as SubBehaviourTree;
			NodeGraph nodeGraph = subBehaviourTree.subBT;

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
						EditorGUIUtility.ExitGUI();
					}
				}
			}

			argumentListEditor.DoLayoutList();

			serializedObject.ApplyModifiedProperties();

			OnBehaviourTreeGUI();
		}
	}
}