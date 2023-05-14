//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	using Arbor;
	using Arbor.Playables;
	using ArborEditor.UIElements;

	[CustomNodeGraphEditor(typeof(ArborFSMInternal))]
	public sealed class StateMachineGraphEditor : NodeGraphEditor
	{
		private bool _DragStateBranchEnable = false;
		private int _DragStateBranchNodeID = 0;
		private int _DragStateBranchHoverStateID = 0;

		public ArborFSMInternal stateMachine
		{
			get
			{
				return nodeGraph as ArborFSMInternal;
			}
			set
			{
				nodeGraph = value;
			}
		}

		private BezierElement _DragBezierElement;

		private BezierElement GetDragBezierElement()
		{
			if (_DragBezierElement == null)
			{
				_DragBezierElement = new BezierElement(graphView.contentContainer)
				{
					pickingMode = PickingMode.Ignore,
					lineColor = dragBezierColor,
					shadow = true,
					shadowColor = bezierShadowColor,
					edgeWidth = 5.0f,
					arrow = true,
					hoverable = false,
				};

				_DragBezierElement.arrowCapElement.pickingMode = PickingMode.Ignore;
			}
			return _DragBezierElement;
		}

		public event System.Action<int> onChangedDragBranchHover;

		public void BeginDragStateBranch(int nodeID)
		{
			var dragBezierElement = GetDragBezierElement();

			graphView.branchOverlayLayer.Add(dragBezierElement);

			_DragStateBranchEnable = true;
			_DragStateBranchNodeID = nodeID;
			_DragStateBranchHoverStateID = 0;

			onChangedDragBranchHover?.Invoke(_DragStateBranchHoverStateID);

			graphView.autoScroll = true;
		}

		public void EndDragStateBranch()
		{
			_DragBezierElement?.RemoveFromHierarchy();
				
			_DragStateBranchEnable = false;
			_DragStateBranchNodeID = 0;
			_DragStateBranchHoverStateID = 0;

			onChangedDragBranchHover?.Invoke(_DragStateBranchHoverStateID);

			graphView.autoScroll = false;

			hostWindow.Repaint();
		}

		public void DragStateBranchBezie(Bezier2D bezier)
		{
			var dragBezierElement = GetDragBezierElement();
			dragBezierElement.startPosition = bezier.startPosition;
			dragBezierElement.startControl = bezier.startControl;
			dragBezierElement.endPosition = bezier.endPosition;
			dragBezierElement.endControl = bezier.endControl;

			dragBezierElement.UpdateLayout();
		}

		public void DragStateBranchHoverStateID(int stateID)
		{
			if (_DragStateBranchHoverStateID != stateID)
			{
				_DragStateBranchHoverStateID = stateID;
				if (_DragStateBranchEnable)
				{
					onChangedDragBranchHover?.Invoke(_DragStateBranchHoverStateID);
				}
			}
		}

		public bool IsDragBranchHover(Node node)
		{
			return _DragStateBranchEnable && _DragStateBranchHoverStateID == node.nodeID;
		}

		public override bool IsDraggingBranch(Node node)
		{
			return base.IsDraggingBranch(node) ||
				_DragStateBranchEnable && _DragStateBranchNodeID == node.nodeID;
		}

		public override bool IsDragBranch()
		{
			return base.IsDragBranch() || _DragStateBranchEnable;
		}

		bool IsLinkedRerouteNode(State state, StateLinkRerouteNode rerouteNode)
		{
			StateEditor stateEditor = GetNodeEditor(state) as StateEditor;

			if (stateEditor == null)
			{
				return false;
			}

			int behaviourCount = state.behaviourCount;

			for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
			{
				StateBehaviourEditorGUI behaviourEditor = stateEditor.GetBehaviourEditor(behaviourIndex);

				if (behaviourEditor != null)
				{
					if (behaviourEditor.IsLinkedRerouteNode(rerouteNode))
					{
						return true;
					}
				}
			}

			return false;
		}

		internal List<StateEditor> GetParentStateEditors(StateLinkRerouteNode rerouteNode)
		{
			List<StateEditor> stateEditors = new List<StateEditor>();

			int stateCount = stateMachine.stateCount;
			for (int i = 0; i < stateCount; i++)
			{
				State state = stateMachine.GetStateFromIndex(i);
				StateEditor stateEditor = GetNodeEditor(state) as StateEditor;
				if (IsLinkedRerouteNode(state, rerouteNode))
				{
					stateEditors.Add(stateEditor);
				}
			}
			return stateEditors;
		}

		public static readonly Color reservedColor = new Color(0.5f, 0.0f, 1.0f);

		public State CreateState(Vector2 position, bool resident, IList<System.Type> types = null)
		{
			Undo.IncrementCurrentGroup();

			State state = stateMachine.CreateState(resident, types);

			if (state != null)
			{
				Undo.RecordObject(stateMachine, "Created State");

				state.position = EditorGUITools.SnapPositionToGrid(new Rect(position.x, position.y, Node.defaultWidth, 100));

				EditorUtility.SetDirty(stateMachine);

				OnCreatedNode(state);
			}

			Repaint();

			return state;
		}

		public StateLinkRerouteNode CreateStateLinkRerouteNode(Vector2 position, Color lineColor, Vector2 direction, int targetID)
		{
			StateLinkRerouteNode stateLinkRerouteNode = stateMachine.CreateStateLinkRerouteNode(EditorGUITools.SnapToGrid(position), lineColor, direction, targetID);
			if (stateLinkRerouteNode != null)
			{
				OnCreatedNode(stateLinkRerouteNode, false);
			}

			Repaint();

			return stateLinkRerouteNode;
		}

		public StateLinkRerouteNode CreateStateLinkRerouteNode(Vector2 position, Color lineColor)
		{
			return CreateStateLinkRerouteNode(position, lineColor, StateLinkRerouteNode.kDefaultDirection, 0);
		}

		void CreateState(object obj)
		{
			Vector2 position = (Vector2)obj;

			CreateState(position, false);
		}

		void CreateResidentState(object obj)
		{
			Vector2 position = (Vector2)obj;

			CreateState(position, true);
		}

		protected override void SetCreateNodeContextMenu(GenericMenu menu, MousePosition mousePosition, bool editable)
		{
			if (editable)
			{
				menu.AddItem(EditorContents.createState, false, CreateState, mousePosition.guiPoint);
				menu.AddItem(EditorContents.createResidentState, false, CreateResidentState, mousePosition.guiPoint);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.createState);
				menu.AddDisabledItem(EditorContents.createResidentState);
			}
		}

		void ClearCount()
		{
			stateMachine.ClearTransitionCount();
		}

		void SetBreakPoints()
		{
			Undo.RecordObject(stateMachine, "BreakPoint On");

			for (int i = 0; i < selectionCount; i++)
			{
				Node node = GetSelectionNode(i);
				State state = node as State;
				if (state != null)
				{
					state.breakPoint = true;

					StateEditor stateEditor = GetNodeEditorFromID(state.nodeID) as StateEditor;
					if (stateEditor != null)
					{
						stateEditor.ShowBreakPoint(state.breakPoint);
					}
				}
			}

			EditorUtility.SetDirty(stateMachine);

			RaiseOnChangedNodes();
		}

		void ReleaseBreakPoints()
		{
			Undo.RecordObject(stateMachine, "BreakPoint Off");

			for (int i = 0; i < selectionCount; i++)
			{
				Node node = GetSelectionNode(i);
				State state = node as State;
				if (state != null)
				{
					state.breakPoint = false;
					StateEditor stateEditor = GetNodeEditorFromID(state.nodeID) as StateEditor;
					if (stateEditor != null)
					{
						stateEditor.ShowBreakPoint(state.breakPoint);
					}
				}
			}

			EditorUtility.SetDirty(stateMachine);

			RaiseOnChangedNodes();
		}

		void ReleaseAllBreakPoints()
		{
			Undo.RecordObject(stateMachine, "Delete All BreakPoint");

			for (int stateIndex = 0, stateCount = stateMachine.stateCount; stateIndex < stateCount; stateIndex++)
			{
				State state = stateMachine.GetStateFromIndex(stateIndex);

				state.breakPoint = false;

				StateEditor stateEditor = GetNodeEditorFromID(state.nodeID) as StateEditor;
				if (stateEditor != null)
				{
					stateEditor.ShowBreakPoint(state.breakPoint);
				}
			}

			EditorUtility.SetDirty(stateMachine);

			RaiseOnChangedNodes();
		}

		internal static int InternalNodeListSortComparison(NodeEditor a, NodeEditor b)
		{
			StateEditor stateEditorA = a as StateEditor;
			StateEditor stateEditorB = b as StateEditor;
			if (stateEditorA == null || stateEditorB == null)
			{
				return NodeListElement.Defaults.SortComparison(a, b);
			}

			ArborFSMInternal stateMachine = stateEditorA.graphEditor.nodeGraph as ArborFSMInternal;

			if (stateMachine.startStateID == stateEditorA.state.nodeID)
			{
				return -1;
			}
			else if (stateMachine.startStateID == stateEditorB.state.nodeID)
			{
				return 1;
			}
			if (!stateEditorA.state.resident && stateEditorB.state.resident)
			{
				return -1;
			}
			else if (stateEditorA.state.resident && !stateEditorB.state.resident)
			{
				return 1;
			}
			return stateEditorA.state.name.CompareTo(stateEditorB.state.name);
		}

		protected override int NodeListSortComparison(NodeEditor a, NodeEditor b)
		{
			return InternalNodeListSortComparison(a, b);
		}

		protected override bool HasViewMenu()
		{
			return true;
		}

		protected override void OnSetViewMenu(GenericMenu menu)
		{
			menu.AddItem(EditorContents.stateLinkShowNodeTop, ArborSettings.stateLinkShowMode == StateLinkShowMode.NodeTop, () =>
			{
				ArborSettings.stateLinkShowMode = StateLinkShowMode.NodeTop;
				Repaint();
			});
			menu.AddItem(EditorContents.stateLinkShowBehaviourTop, ArborSettings.stateLinkShowMode == StateLinkShowMode.BehaviourTop, () =>
			{
				ArborSettings.stateLinkShowMode = StateLinkShowMode.BehaviourTop;
				Repaint();
			});
			menu.AddItem(EditorContents.stateLinkShowBehaviourBottom, ArborSettings.stateLinkShowMode == StateLinkShowMode.BehaviourBottom, () =>
			{
				ArborSettings.stateLinkShowMode = StateLinkShowMode.BehaviourBottom;
				Repaint();
			});
			menu.AddItem(EditorContents.stateLinkShowNodeBottom, ArborSettings.stateLinkShowMode == StateLinkShowMode.NodeBottom, () =>
			{
				ArborSettings.stateLinkShowMode = StateLinkShowMode.NodeBottom;
				Repaint();
			});
		}

		protected override bool HasDebugMenu()
		{
			return true;
		}

		protected override void OnSetDebugMenu(GenericMenu menu)
		{
			bool isSelectionState = false;
			for (int i = 0; i < selectionCount; i++)
			{
				Node node = GetSelectionNode(i);
				if (node is State)
				{
					isSelectionState = true;
					break;
				}
			}

			bool editable = this.editable;

			if (isSelectionState && editable)
			{
				menu.AddItem(EditorContents.setBreakPoints, false, SetBreakPoints);
				menu.AddItem(EditorContents.releaseBreakPoints, false, ReleaseBreakPoints);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.setBreakPoints);
				menu.AddDisabledItem(EditorContents.releaseBreakPoints);
			}

			if (editable)
			{
				menu.AddItem(EditorContents.releaseAllBreakPoints, false, ReleaseAllBreakPoints);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.releaseAllBreakPoints);
			}

			if (Application.isPlaying && editable)
			{
				menu.AddItem(EditorContents.clearCount, false, ClearCount);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.clearCount);
			}
		}

		public override GUIContent GetGraphLabel()
		{
			return EditorContents.stateMachine;
		}

		public override bool HasPlayState()
		{
			return true;
		}

		public override PlayState GetPlayState()
		{
			return stateMachine.playState;
		}

		bool CheckLoop(StateLinkRerouteNode current, StateLinkRerouteNode target)
		{
			if (current == null)
			{
				return false;
			}

			while (target != null)
			{
				StateLinkRerouteNode nextNode = stateMachine.GetNodeFromID(target.link.stateID) as StateLinkRerouteNode;
				if (nextNode == null)
				{
					break;
				}

				if (nextNode == current)
				{
					return true;
				}

				target = nextNode;
			}

			return false;
		}

		public NodeEditor GetTargetNodeEditorFromPosition(Vector2 position, Node node)
		{
			for (int i = 0, count = stateMachine.stateCount; i < count; i++)
			{
				State state = stateMachine.GetStateFromIndex(i);
				NodeEditor nodeEditor = GetNodeEditorFromID(state.nodeID);
				if (!state.resident && nodeEditor.rect.Contains(position))
				{
					return nodeEditor;
				}
			}

			StateLinkRerouteNode rerouteNode = node as StateLinkRerouteNode;

			StateLinkRerouteNodeList stateLinks = stateMachine.stateLinkRerouteNodes;
			for (int i = 0, count = stateLinks.count; i < count; i++)
			{
				StateLinkRerouteNode stateLinkNode = stateLinks[i];
				NodeEditor nodeEditor = GetNodeEditorFromID(stateLinkNode.nodeID);
				if (rerouteNode != stateLinkNode && nodeEditor.rect.Contains(position))
				{
					if (!CheckLoop(rerouteNode, stateLinkNode))
					{
						return nodeEditor;
					}
				}
			}

			return null;
		}

		protected override Node GetActiveNode()
		{
			return stateMachine.currentState;
		}

		protected override void OnCreateSetParameter(Vector2 position, Parameter parameter)
		{
			State state = CreateState(position, false, StateEditor.Types.SetParameterBehaviourTypes);

			Arbor.ParameterBehaviours.SetParameterBehaviourInternal setParameterBehaviour = state.GetBehaviour<Arbor.ParameterBehaviours.SetParameterBehaviourInternal>();

			Undo.RecordObject(setParameterBehaviour, "Add Behaviour");

			setParameterBehaviour.SetParameter(parameter);

			EditorUtility.SetDirty(setParameterBehaviour);
		}

		internal void UpdateStateLinkTargetPosition(int nodeID)
		{
			for (int nodeIndex = 0, nodeCount = nodeEditorCount; nodeIndex < nodeCount; nodeIndex++)
			{
				var nodeEditor = GetNodeEditor(nodeIndex);

				if (nodeEditor is StateEditor stateEditor)
				{
					stateEditor.UpdateStateLinkTargetPosition(nodeID);
				}
				else if (nodeEditor is StateLinkRerouteNodeEditor rerouteEditor)
				{
					rerouteEditor.UpdateStateLinkTargetPosition(nodeID);
				}
			}
		}
	}
}