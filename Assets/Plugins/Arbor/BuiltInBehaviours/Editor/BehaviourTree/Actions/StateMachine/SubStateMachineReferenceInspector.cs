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

	[CustomEditor(typeof(SubStateMachineReference))]
	internal sealed class SubStateMachineReferenceInspector : NodeBehaviourEditor
	{
		private FlexibleSceneObjectProperty _ExternalFSMProperty;

		private void OnEnable()
		{
			_ExternalFSMProperty = new FlexibleSceneObjectProperty(serializedObject.FindProperty("_ExternalFSM"));
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

				_ArgumentListEditor.nodeGraph = GetExternalGraph();

				return _ArgumentListEditor;
			}
		}

		NodeGraph GetExternalGraph()
		{
			var type = _ExternalFSMProperty.type;
			if (type == FlexibleSceneObjectType.Constant)
			{
				return _ExternalFSMProperty.valueProperty.objectReferenceValue as NodeGraph;
			}

			return null;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			var flexibleType = _ExternalFSMProperty.type;
			var externalGraph = GetExternalGraph();

			bool changedExternalGraph = false;

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_ExternalFSMProperty.property);
			if (EditorGUI.EndChangeCheck())
			{
				var newFlexibleType = _ExternalFSMProperty.type;
				var newExternalGraph = GetExternalGraph();
				if (flexibleType != newFlexibleType || externalGraph != newExternalGraph)
				{
					changedExternalGraph = true;
				}

				argumentListEditor.UpdateNodeGraph(newExternalGraph);
			}

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_UseDirectlyInScene"));

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_UsePool"));

			SubStateMachineReference subStateMachine = target as SubStateMachineReference;
			NodeGraph nodeGraph = subStateMachine.runtimeFSM;

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
