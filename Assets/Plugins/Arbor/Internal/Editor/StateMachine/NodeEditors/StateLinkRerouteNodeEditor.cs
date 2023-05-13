//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace ArborEditor
{
	using Arbor;
	using ArborEditor.UIElements;

	[CustomNodeEditor(typeof(StateLinkRerouteNode))]
	internal sealed class StateLinkRerouteNodeEditor : NodeEditor
	{
		public StateLinkRerouteNode stateLinkRerouteNode
		{
			get
			{
				return node as StateLinkRerouteNode;
			}
		}

		protected override bool HasHeaderGUI()
		{
			return false;
		}

		public override string GetTitle()
		{
			return Localization.GetWord("StateLinkRerouteNode");
		}

		protected override float GetWidth()
		{
			return 32f;
		}

		protected override bool HasContentBackground()
		{
			return false;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			isNormalInvisibleStyle = true;
			showContextMenuInWindow = ShowContextMenu.Show;
			isUsedMouseDownOnMainGUI = false;
			isResizable = false;
		}

		protected override void OnUndoRedoPerformed()
		{
			if (stateLinkRerouteNode == null)
			{
				return;
			}

			_DirectionElement.SetValueWithoutNotify(stateLinkRerouteNode.direction);
			_LinkElement.UpdateDirection();

			var stateMachieGraphEditor = graphEditor as StateMachineGraphEditor;
			stateMachieGraphEditor.UpdateStateLinkTargetPosition(nodeID);
		}

		public bool isDragDirection
		{
			get
			{
				return _DirectionElement.isDragging;
			}
		}

		void DeleteKeepConnection()
		{
			NodeGraph nodeGraph = this.node.nodeGraph;
			ArborFSMInternal stateMachine = nodeGraph as ArborFSMInternal;
			StateLinkRerouteNode rerouteNode = stateLinkRerouteNode;

			Undo.IncrementCurrentGroup();
			int undoGroup = Undo.GetCurrentGroup();

			int nextStateID = _LinkElement.stateLink.stateID;
			
			StateLinkRerouteNodeList stateLinkRerouteNodes = stateMachine.stateLinkRerouteNodes;
			int rerouteCount = stateLinkRerouteNodes.count;
			for (int i = 0; i < rerouteCount; i++)
			{
				StateLinkRerouteNode node = stateLinkRerouteNodes[i];

				if (node == null || node.link.stateID != rerouteNode.nodeID)
				{
					continue;
				}

				StateLinkRerouteNodeEditor nodeEditor = graphEditor.GetNodeEditor(node) as StateLinkRerouteNodeEditor;
				if (nodeEditor == null)
				{
					continue;
				}

				Undo.RecordObject(stateMachine, "Delete Keep Connection");

				var linkElement = nodeEditor._LinkElement;
				linkElement.SetLinkState(nextStateID);

				graphEditor.VisibleNode(node);
			}

			for (int stateIndex = 0, count = stateMachine.stateCount; stateIndex < count; stateIndex++)
			{
				State state = stateMachine.GetStateFromIndex(stateIndex);
				StateEditor stateEditor = graphEditor.GetNodeEditor(state) as StateEditor;

				bool changed = false;

				for (int behaviourIndex = 0, behaviourCount = state.behaviourCount; behaviourIndex < behaviourCount; behaviourIndex++)
				{
					StateBehaviourEditorGUI behaviourEditor = stateEditor.GetBehaviourEditor(behaviourIndex);

					if (behaviourEditor == null)
					{
						continue;
					}

					if (behaviourEditor.DeleteKeepConnection(rerouteNode.nodeID, nextStateID))
					{
						changed = true;
					}
				}

				if (changed)
				{
					graphEditor.VisibleNode(state);
				}
			}

			graphEditor.DeleteNodes(new Node[] { rerouteNode });

			Undo.CollapseUndoOperations(undoGroup);

			EditorUtility.SetDirty(nodeGraph);

			Repaint();
		}

		bool IsConnected()
		{
			NodeGraph nodeGraph = this.node.nodeGraph;
			ArborFSMInternal stateMachine = nodeGraph as ArborFSMInternal;
			StateLinkRerouteNode rerouteNode = stateLinkRerouteNode;

			int nextStateID = rerouteNode.link.stateID;

			if (nextStateID == 0)
			{
				return false;
			}

			StateLinkRerouteNodeList stateLinkRerouteNodes = stateMachine.stateLinkRerouteNodes;
			int rerouteCount = stateLinkRerouteNodes.count;
			for (int i = 0; i < rerouteCount; i++)
			{
				StateLinkRerouteNode node = stateLinkRerouteNodes[i];

				if (node != null && node.link.stateID == rerouteNode.nodeID)
				{
					return true;
				}
			}

			for (int stateIndex = 0, count = stateMachine.stateCount; stateIndex < count; stateIndex++)
			{
				State state = stateMachine.GetStateFromIndex(stateIndex);
				StateEditor stateEditor = graphEditor.GetNodeEditor(state) as StateEditor;

				for (int behaviourIndex = 0, behaviourCount = state.behaviourCount; behaviourIndex < behaviourCount; behaviourIndex++)
				{
					StateBehaviourEditorGUI behaviourEditor = stateEditor.GetBehaviourEditor(behaviourIndex);

					if (behaviourEditor == null)
					{
						continue;
					}

					if (behaviourEditor.IsConnected(rerouteNode.nodeID))
					{
						return true;
					}
				}
			}

			return false;
		}

		protected override void SetDeleteContextMenu(GenericMenu menu, bool deletable, bool editable)
		{
			if (deletable && IsConnected() && editable)
			{
				menu.AddItem(EditorContents.deleteKeepConnection, false, DeleteKeepConnection);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.deleteKeepConnection);
			}
		}

		protected override void SetContextMenu(GenericMenu menu, Rect headerPosition, bool editable)
		{
			Rect mousePosition = new Rect(0, 0, 0, 0);
			mousePosition.position = Event.current.mousePosition;
			Rect position = GUIUtility.GUIToScreenRect(mousePosition);

			menu.AddItem(EditorContents.settings, false, () =>
			{
				_LinkElement.OpenSettingsWindow(GUIUtility.ScreenToGUIRect(position), false);
			});
		}

		private StateLinkRerouteElement _LinkElement;
		private DirectionElement _DirectionElement;

		protected override VisualElement CreateContentElement()
		{
			VisualElement root = new VisualElement();
			root.AddToClassList("reroute-node-content");

			_LinkElement = new StateLinkRerouteElement(this);
			_LinkElement.onChangedPinColor += (c) =>
			{
				_DirectionElement.arrowColor = c;
			};
			root.Add(_LinkElement);

			_DirectionElement = new DirectionElement()
			{
				arrowColor = _LinkElement.pinColor,
			};
			_DirectionElement.StretchToParentSize();
			_DirectionElement.RegisterCallback<ChangeEvent<Vector2>>(OnChangeDirection);

			_DirectionElement.SetValueWithoutNotify(stateLinkRerouteNode.direction);
			root.Add(_DirectionElement);

			if (!isSelection)
			{
				_DirectionElement.style.display = DisplayStyle.None;
			}

			return root;
		}

		void OnChangeDirection(ChangeEvent<Vector2> e)
		{
			Undo.RecordObject(node.nodeGraph, "Change Reroute Direction");
			stateLinkRerouteNode.direction = e.newValue;
			EditorUtility.SetDirty(node.nodeGraph);

			_LinkElement.UpdateDirection();

			var stateMachieGraphEditor = graphEditor as StateMachineGraphEditor;
			stateMachieGraphEditor.UpdateStateLinkTargetPosition(nodeID);
		}

		protected override void RegisterCallbackOnElement()
		{
			base.RegisterCallbackOnElement();

			nodeElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedElement);
			nodeElement.RegisterCallback<ChangeNodePositionEvent>(OnChangeNodePosition);
		}

		void OnGeometryChangedElement(GeometryChangedEvent e)
		{
			var stateMachineGraphEditor = graphEditor as StateMachineGraphEditor;
			stateMachineGraphEditor.UpdateStateLinkTargetPosition(nodeID);
		}

		void OnChangeNodePosition(ChangeNodePositionEvent e)
		{
			var stateMachineGraphEditor = graphEditor as StateMachineGraphEditor;
			stateMachineGraphEditor.UpdateStateLinkTargetPosition(nodeID);
		}

		internal void UpdateStateLinkTargetPosition(int nodeID)
		{
			if (_LinkElement.stateLink.stateID == nodeID)
			{
				_LinkElement.UpdateBezier();
			}
		}

		protected override void OnChangeSelection(bool isSelection)
		{
			base.OnChangeSelection(isSelection);

			if (isSelection)
			{
				_DirectionElement.style.display = DisplayStyle.Flex;
			}
			else
			{
				_DirectionElement.style.display = DisplayStyle.None;
			}
		}

		public override bool IsDraggingVisible()
		{
			Node targetNode = graphEditor.nodeGraph.GetNodeFromID(stateLinkRerouteNode.link.stateID);
			if (targetNode != null)
			{
				if (graphEditor.IsDraggingNode(targetNode))
				{
					return true;
				}
				StateLinkRerouteNodeEditor rerouteNodeEditor = graphEditor.GetNodeEditor(targetNode) as StateLinkRerouteNodeEditor;
				if (rerouteNodeEditor != null && rerouteNodeEditor.isDragDirection)
				{
					return true;
				}
			}
			return false;
		}

		public override MinimapLayer minimapLayer
		{
			get
			{
				return MinimapLayer.None;
			}
		}
	}
}
