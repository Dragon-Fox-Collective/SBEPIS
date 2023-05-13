//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	[System.Serializable]
	internal sealed class StateBehaviourEditorList : BehaviourEditorList<StateBehaviourEditorGUI, StateBehaviour>
	{
		public StateEditor stateEditor
		{
			get
			{
				return nodeEditor as StateEditor;
			}
		}

		public State state
		{
			get
			{
				return node as State;
			}
		}

		public override System.Type targetType
		{
			get
			{
				return typeof(StateBehaviour);
			}
		}

		public override bool isDroppableParameter
		{
			get
			{
				return true;
			}
		}

		public override GUIContent GetAddBehaviourContent()
		{
			return EditorContents.addBehaviour;
		}

		public override GUIContent GetInsertButtonContent()
		{
			return EditorContents.insertBehaviour;
		}

		public override GUIContent GetPasteBehaviourContent()
		{
			return EditorContents.pasteBehaviour;
		}

		public override int GetCount()
		{
			return state.behaviourCount;
		}

		public override Object GetObject(int behaviourIndex)
		{
			return state.GetBehaviourObjectFromIndex(behaviourIndex);
		}

		public override void InsertBehaviour(int index, System.Type classType)
		{
			stateEditor.InsertBehaviour(index, classType);
		}

		public override void InsertSetParameter(int index, Parameter parameter)
		{
			stateEditor.InsertSetParameterBehaviour(index, parameter);
		}

		public override void MoveBehaviour(Node fromNode, int fromIndex, Node toNode, int toIndex, bool isCopy)
		{
			State fromState = fromNode as State;
			State toState = toNode as State;

			ArborFSMInternal stateMachine = fromState.stateMachine;

			Undo.IncrementCurrentGroup();

			Undo.RecordObject(stateMachine, isCopy ? "Paste Behaviour" : "Move Behaviour");

			StateBehaviour fromBehaviour = fromState.GetBehaviourFromIndex(fromIndex);
			StateBehaviour toBehaviour = null;
			if (isCopy)
			{
				toBehaviour = StateEditor.PasteStateBehaviourAsNew(toState, toIndex, fromBehaviour);
			}
			else
			{
				var fromStateEditor = graphEditor.GetNodeEditor(fromState) as StateEditor;
				if (fromStateEditor != null)
				{
					fromStateEditor.RemoveBehaviourEditor(fromIndex);
				}

				fromState.MoveBehaviour(fromIndex, toState, toIndex);

				toBehaviour = fromBehaviour;
			}

			if (toBehaviour != null)
			{
				var toStateEditor = graphEditor.GetNodeEditor(toState) as StateEditor;
				if (toStateEditor != null)
				{
					toStateEditor.InsertBehaviourEditor(toIndex, toBehaviour);
				}
			}

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(stateMachine);

			graphEditor.RaiseOnChangedNodes();
		}

		public override void PasteBehaviour(int index)
		{
			stateEditor.PasteBehaviour(index);
		}

		public override void OpenBehaviourMenu(Rect buttonRect, int index)
		{
			BehaviourMenuWindow.instance.Init(buttonRect, index, stateEditor.CreateStateBehaviourByType);
		}
	}
}