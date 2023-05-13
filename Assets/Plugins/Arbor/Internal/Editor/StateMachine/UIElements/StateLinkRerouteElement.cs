//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	using Arbor;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class StateLinkRerouteElement : StateLinkElementBase
	{
		public StateLinkRerouteNodeEditor _NodeEditor;

		private StateLinkReroutePinElement _Pin;
		private bool _IsDragHover;
		private Color _PinColor = Color.white;

		private ConnectManipulator _ConnectManipulator;

		public bool isDragHover
		{
			get
			{
				return _IsDragHover;
			}
			private set
			{
				if (_IsDragHover != value)
				{
					_IsDragHover = value;
					_Pin.EnableActive(_IsDragHover);
					UpdatePinElementColor();
					UpdatePinRotation();
				}
			}
		}

		void UpdatePinElementColor()
		{
			if (_IsDragHover || _ConnectManipulator.isActive)
			{
				_Pin.pinColor = StyleKeyword.Null;
			}
			else
			{
				_Pin.pinColor = _PinColor;
			}
		}

		public Color pinColor
		{
			get
			{
				return _PinColor;
			}
		}

		public StateLinkRerouteElement(StateLinkRerouteNodeEditor nodeEditor) : base(nodeEditor, null)
		{
			_NodeEditor = nodeEditor;

			AddToClassList("reroute-link-slot");
			AddToClassList("state-link-reroute-slot");

			_Pin = new StateLinkReroutePinElement();
			Add(_Pin);

			_ConnectManipulator = new ConnectManipulator();
			_ConnectManipulator.onChangedActive += isActive =>
			{
				EnableInClassList("reroute-link-slot-active", isActive);

				UpdatePinElementColor();
				UpdatePinRotation();
			};
			this.AddManipulator(_ConnectManipulator);

			var graphEditor = nodeEditor.graphEditor as StateMachineGraphEditor;
			if (graphEditor != null)
			{
				StateLinkRerouteNode stateLinkRerouteNode = _NodeEditor.stateLinkRerouteNode;
				isDragHover = graphEditor.IsDragBranchHover(stateLinkRerouteNode);
			}

			Setup(_NodeEditor.stateLinkRerouteNode.link, null);

			UpdatePinRotation();
		}

		protected override void OnSettingsChanged()
		{
			UpdateLineColor();
		}

		protected override void OnAttachToPanel(AttachToPanelEvent e)
		{
			base.OnAttachToPanel(e);

			var graphEditor = _NodeEditor.graphEditor as StateMachineGraphEditor;
			if (graphEditor != null)
			{
				graphEditor.onChangedDragBranchHover -= OnChanedDragBranchHover;
				graphEditor.onChangedDragBranchHover += OnChanedDragBranchHover;
			}
		}

		protected override void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			base.OnDetachFromPanel(e);

			var graphEditor = _NodeEditor.graphEditor as StateMachineGraphEditor;
			if (graphEditor != null)
			{
				graphEditor.onChangedDragBranchHover -= OnChanedDragBranchHover;
			}
		}

		protected override void OnRebuildElement(RebuildElementEvent e)
		{
			Setup(_NodeEditor.stateLinkRerouteNode.link, null);

			base.OnRebuildElement(e);
		}

		protected override void OnConnectionChanged(bool on)
		{
			EnableInClassList("reroute-link-slot-on", on);

			UpdatePinRotation();
		}

		void OnChanedDragBranchHover(int nodeID)
		{
			isDragHover = _NodeEditor.nodeID == nodeID;
		}

		protected override void OnSetup(bool changedStateLink, bool changedFieldInfo)
		{
			if (changedStateLink)
			{
				UpdateLineColor();
			}
		}

		protected override void OnUndoRedoPerformed()
		{
			UpdateLineColor();
		}

		protected override Bezier2D OnUpdateBezier(Rect pinPos, NodeEditor targetNodeEditor)
		{
			NodeEditor nodeEditor = this.nodeEditor;
			
			bool isRight = false;
			Bezier2D bezier = GetTargetBezier(nodeEditor, targetNodeEditor, pinPos.center, pinPos.center, ref isRight);

			return bezier;
		}

		public event System.Action<Color> onChangedPinColor;

		void UpdateLineColor()
		{
			StateLink stateLink = this.stateLink;

			Color lineColor = stateLink.lineColor;
			lineColor.a = 1f;

			if (_PinColor != lineColor)
			{
				_PinColor = lineColor;

				onChangedPinColor?.Invoke(_PinColor);

				UpdatePinElementColor();
			}
		}

		void UpdatePinRotation()
		{
			if (_IsDragHover || on || _ConnectManipulator.isActive)
			{
				_Pin.transform.rotation = Quaternion.identity;
			}
			else
			{
				_Pin.transform.rotation = Quaternion.FromToRotation(Vector2.right, _NodeEditor.stateLinkRerouteNode.direction);
			}
		}

		public void UpdateDirection()
		{
			UpdatePinRotation();
			UpdateBezier();
		}

		sealed class ConnectManipulator : DragOnGraphManipulator
		{
			private NodeEditor _DragTargetNodeEditor = null;

			public ConnectManipulator()
			{
				activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse });
			}

			protected override void RegisterCallbacksOnTarget()
			{
				base.RegisterCallbacksOnTarget();
				target.RegisterCallback<KeyDownEvent>(OnKeyDown);
			}

			protected override void UnregisterCallbacksFromTarget()
			{
				base.UnregisterCallbacksFromTarget();
				target.UnregisterCallback<KeyDownEvent>(OnKeyDown);
			}

			protected override void OnChangeGraphView(IChangeGraphViewEvent e)
			{
				EventBase evtBase = e as EventBase;
				GraphView graphView = evtBase.target as GraphView;

				Vector2 mousePosition = graphView.GetMousePosition(target);
				UpdateMousePosition(mousePosition);
			}

			protected override void OnMouseDown(MouseDownEvent e)
			{
				StateLinkRerouteElement linkElement = target as StateLinkRerouteElement;
				if (linkElement == null)
				{
					return;
				}

				_DragTargetNodeEditor = null;

				var nodeEditor = linkElement._NodeEditor;
				var nodePosition = nodeEditor.rect;
				var stateLinkRerouteNode = nodeEditor.stateLinkRerouteNode;
				var graphEditor = linkElement._GraphEditor;
				
				if (graphEditor != null)
				{

					graphEditor.BeginDragStateBranch(nodeEditor.nodeID);

					Vector2 mousePosition = (e.target as VisualElement).ChangeCoordinatesTo(nodeEditor.nodeElement, e.localMousePosition);
					Rect pinPos = linkElement.parent.ChangeCoordinatesTo(nodeEditor.nodeElement, linkElement.layout);

					Bezier2D bezier = new Bezier2D();
					bezier.startPosition = pinPos.center;
					bezier.startControl = bezier.startPosition + stateLinkRerouteNode.direction * EditorGUITools.kBezierTangent;
					bezier.endPosition = mousePosition;
					bezier.endControl = bezier.startControl;

					Vector2 statePosition = new Vector2(nodePosition.x, nodePosition.y);
					bezier.startPosition += statePosition;
					bezier.startControl += statePosition;
					bezier.endPosition += statePosition;
					bezier.endControl += statePosition;

					graphEditor.DragStateBranchBezie(bezier);
				}

				e.StopPropagation();
			}

			void UpdateMousePosition(Vector2 localMousePosition)
			{
				StateLinkRerouteElement linkElement = target as StateLinkRerouteElement;
				if (linkElement == null)
				{
					return;
				}

				var nodeEditor = linkElement._NodeEditor;
				var nodePosition = nodeEditor.rect;
				var stateLinkRerouteNode = nodeEditor.stateLinkRerouteNode;
				var graphEditor = linkElement._GraphEditor;

				Vector2 mousePosition = target.ChangeCoordinatesTo(nodeEditor.nodeElement, localMousePosition);

				NodeEditor hoverNodeEditor = graphEditor.GetTargetNodeEditorFromPosition(graphEditor.graphView.ElementToGraph(nodeEditor.nodeElement, mousePosition), stateLinkRerouteNode);
				if (hoverNodeEditor != null)
				{
					if (graphEditor != null)
					{
						graphEditor.DragStateBranchHoverStateID(hoverNodeEditor.nodeID);
					}

					_DragTargetNodeEditor = hoverNodeEditor;
				}
				else
				{
					if (graphEditor != null)
					{
						graphEditor.DragStateBranchHoverStateID(0);
					}
					_DragTargetNodeEditor = null;
				}

				Rect pinPos = linkElement.parent.ChangeCoordinatesTo(nodeEditor.nodeElement, linkElement.layout);

				Bezier2D bezier = new Bezier2D();
				if (_DragTargetNodeEditor != null)
				{
					bool isRight = false;
					bezier = StateLinkElementBase.GetTargetBezier(nodeEditor, _DragTargetNodeEditor, pinPos.center, pinPos.center, ref isRight);
				}
				else
				{
					bezier.startPosition = pinPos.center;
					bezier.startControl = bezier.startPosition + stateLinkRerouteNode.direction * EditorGUITools.kBezierTangent;
					bezier.endPosition = mousePosition;
					bezier.endControl = bezier.startControl;
				}

				Vector2 statePosition = new Vector2(nodePosition.x, nodePosition.y);
				bezier.startPosition += statePosition;
				bezier.startControl += statePosition;
				bezier.endPosition += statePosition;
				bezier.endControl += statePosition;

				graphEditor.DragStateBranchBezie(bezier);
			}

			protected override void OnMouseMove(MouseMoveEvent e)
			{
				StateLinkRerouteElement linkElement = target as StateLinkRerouteElement;
				if (linkElement == null)
				{
					return;
				}

				DragAndDrop.PrepareStartDrag();

				UpdateMousePosition(e.localMousePosition);

				e.StopPropagation();
			}

			protected override void OnMouseUp(MouseUpEvent e)
			{
				StateLinkRerouteElement linkElement = target as StateLinkRerouteElement;
				if (linkElement == null)
				{
					return;
				}

				var stateLink = linkElement.stateLink;
				var graphEditor = linkElement._GraphEditor;

				if (_DragTargetNodeEditor == null)
				{
					Vector2 localMousePosition = e.localMousePosition;
					VisualElement eventTarget = e.target as VisualElement;

					GenericMenu menu = new GenericMenu();

					Vector2 graphMousePosition = graphEditor.graphView.ElementToGraph(eventTarget, localMousePosition);
					Vector2 screenMousePosition = eventTarget.LocalToScreen(localMousePosition);

					menu.AddItem(EditorContents.createState, false, () =>
					{
						graphMousePosition -= new Vector2(8f, 12f);

						State newState = graphEditor.CreateState(graphMousePosition, false);

						Undo.RecordObject(graphEditor.nodeGraph, "Link State");

						linkElement.SetLinkState(newState.nodeID, false);

						EditorUtility.SetDirty(graphEditor.nodeGraph);
					});

					menu.AddItem(EditorContents.reroute, false, () =>
					{
						Undo.IncrementCurrentGroup();
						int undoGroup = Undo.GetCurrentGroup();

						graphMousePosition -= new Vector2(16f, 16f);

						Color lineColor = stateLink.lineColor;

						StateLinkRerouteNode newStateLinkNode = graphEditor.CreateStateLinkRerouteNode(graphMousePosition, lineColor);

						Undo.RecordObject(graphEditor.nodeGraph, "Link State");

						linkElement.SetLinkState(newStateLinkNode.nodeID, false);

						Undo.CollapseUndoOperations(undoGroup);

						EditorUtility.SetDirty(graphEditor.nodeGraph);
					});

					menu.AddSeparator("");

					menu.AddItem(EditorContents.nodeListSelection, false, () =>
					{
						StateLinkRerouteElement currentLinkElement = linkElement;
						NodeGraph currentGraph = graphEditor.nodeGraph;

						StateLinkSelectorWindow.instance.Open(graphEditor, new Rect(screenMousePosition, Vector2.zero), currentLinkElement.stateLink.stateID,
							(selectedNodeEditor) =>
							{
								Undo.RecordObject(currentGraph, "Link State");

								currentLinkElement.SetLinkState(selectedNodeEditor.nodeID);

								EditorUtility.SetDirty(currentGraph);

								//graphEditor.BeginFrameSelected(selectedNodeEditor.node);
							}
						);
					});

					if (stateLink.stateID != 0)
					{
						menu.AddSeparator("");
						menu.AddItem(EditorContents.disconnect, false, () =>
						{
							Undo.RecordObject(graphEditor.nodeGraph, "Disconect StateLink");

							stateLink.stateID = 0;

							EditorUtility.SetDirty(graphEditor.nodeGraph);
						});
					}
					menu.ShowAsContext();

					return;
				}
				
				NodeEditor targetNodeEditor = graphEditor.GetNodeEditorFromID(stateLink.stateID);
				if (_DragTargetNodeEditor != targetNodeEditor)
				{
					Undo.RecordObject(graphEditor.nodeGraph, "Link State");

					linkElement.SetLinkState(_DragTargetNodeEditor.nodeID);

					EditorUtility.SetDirty(graphEditor.nodeGraph);
				}
			}

			protected override void OnEndDrag()
			{
				base.OnEndDrag();

				StateLinkRerouteElement linkElement = target as StateLinkRerouteElement;
				if (linkElement == null)
				{
					return;
				}

				var graphEditor = linkElement._GraphEditor;

				if (graphEditor != null)
				{
					graphEditor.EndDragStateBranch();
				}

				_DragTargetNodeEditor = null;

			}

			void OnKeyDown(KeyDownEvent e)
			{
				StateLinkRerouteElement linkElement = target as StateLinkRerouteElement;
				if (linkElement == null)
				{
					return;
				}

				if (!isActive || e.keyCode != KeyCode.Escape)
				{
					return;
				}

				EndDrag();
				e.StopPropagation();
			}
		}
	}
}