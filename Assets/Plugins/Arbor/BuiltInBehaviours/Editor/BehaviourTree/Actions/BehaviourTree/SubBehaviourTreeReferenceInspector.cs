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

	[CustomEditor(typeof(SubBehaviourTreeReference))]
	internal sealed class SubBehaviourTreeReferenceInspector : NodeBehaviourEditor
	{
		private FlexibleSceneObjectProperty _ExternalBTProperty;

		private GraphArgumentListEditor _ArgumentListEditor = null;

		private GraphArgumentListEditor argumentListEditor
		{
			get
			{
				if (_ArgumentListEditor == null)
				{
					_ArgumentListEditor = new GraphArgumentListEditor(serializedObject.FindProperty("_ArgumentList"));
				}

				_ArgumentListEditor.nodeGraph = GetExternalGraph();

				return _ArgumentListEditor;
			}
		}

		NodeGraph GetExternalGraph()
		{
			var type = _ExternalBTProperty.type;
			if (type == FlexibleSceneObjectType.Constant)
			{
				return _ExternalBTProperty.valueProperty.objectReferenceValue as NodeGraph;
			}

			return null;
		}

		private void OnEnable()
		{
			_ExternalBTProperty = new FlexibleSceneObjectProperty(serializedObject.FindProperty("_ExternalBT"));
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			var flexibleType = _ExternalBTProperty.type;
			var externalGraph = GetExternalGraph();

			bool changedExternalGraph = false;

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_ExternalBTProperty.property);
			if (EditorGUI.EndChangeCheck())
			{
				var newFlexibleType = _ExternalBTProperty.type;
				var newExternalGraph = GetExternalGraph();
				if (flexibleType != newFlexibleType || externalGraph != newExternalGraph)
				{
					changedExternalGraph = true;
				}

				argumentListEditor.UpdateNodeGraph(newExternalGraph);
			}

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_UseDirectlyInScene"));

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_UsePool"));

			SubBehaviourTreeReference subBehaviourTree = target as SubBehaviourTreeReference;
			NodeGraph nodeGraph = subBehaviourTree.runtimeBT;

			if (nodeGraph != null)
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

			if (serializedObject.ApplyModifiedProperties())
			{
				if (changedExternalGraph)
				{
					graphEditor.hostWindow.OnChangedGraphTree();
				}
			}
		}
	}
}
