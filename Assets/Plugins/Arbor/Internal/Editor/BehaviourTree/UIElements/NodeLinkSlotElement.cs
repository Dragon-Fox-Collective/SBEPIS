//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.BehaviourTree.UIElements
{
	using Arbor;
	using Arbor.BehaviourTree;
	using ArborEditor.UIElements;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal abstract class NodeLinkSlotElement : VisualElement
	{
		const float kDefaultNodeHeight = 93f; // Approximate height

		//public static readonly Vector2 kBezierTangentOffset = EditorGUITools.kBezierTangentOffset;
		public static readonly Vector2 kBezierTangentOffset = new Vector2(0f, EditorGUITools.kBezierTangent);

		protected static Vector2 GetNodeLinkPinPos(Rect position)
		{
			return position.center;
		}

		public readonly TreeNodeBaseEditor nodeEditor;
		private readonly bool _IsParentSlot;

		private bool _IsLayouted = false;
		private bool _On;

		private VisualElement _PinElement;
		private bool _IsDragHover;

		protected virtual GUIContent GetDisconnectContent()
		{
			return EditorContents.disconnect;
		}

		public BehaviourTreeGraphEditor graphEditor
		{
			get
			{
				return nodeEditor.graphEditor as BehaviourTreeGraphEditor;
			}
		}

		public bool on
		{
			get
			{
				return _On;
			}
			protected set
			{
				if (_On != value)
				{
					_On = value;
					EnableInClassList("node-link-slot-on", _On);
				}
			}
		}

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
					_PinElement.EnableInClassList("pin-active", _IsDragHover);
				}
			}
		}

		private ConnectManipulator _ConnectManipulator;

		public NodeLinkSlotElement(TreeNodeBaseEditor nodeEditor, bool isParentSlot)
		{
			this.nodeEditor = nodeEditor;
			_IsParentSlot = isParentSlot;

			AddToClassList("node-link-slot");
			AddToClassList("bt-node-link-slot");

			_PinElement = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
			};
			_PinElement.AddToClassList("pin");
			Add(_PinElement);

			_ConnectManipulator = new ConnectManipulator();
			_ConnectManipulator.onChangedActive += isActive =>
			{
				EnableInClassList("node-link-slot-active", isActive);
			};
			this.AddManipulator(_ConnectManipulator);

			this.AddManipulator(new ContextClickManipulator(OnContextClick));
			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

			var graphEditor = nodeEditor.graphEditor;
			if (graphEditor != null)
			{
				SetEnabled(graphEditor.editable);
			}

			SetNode(nodeEditor.node);
		}

		void OnGeometryChanged(GeometryChangedEvent e)
		{
			VisualElement target = e.target as VisualElement;
			if (UIElementsUtility.IsVisible(target))
			{
				_IsLayouted = true;
				DoChangedPosition();
			}
		}

		List<VisualElement> _RegisterElements = new List<VisualElement>();

		protected virtual void OnAttachToPanel(AttachToPanelEvent e)
		{
			SetNode(nodeEditor.node);

			NodeElement nodeElement = nodeEditor.nodeElement;
			nodeElement.RegisterCallback<ChangeNodePositionEvent>(OnChangeNodePosition);
			nodeElement.RegisterCallback<RebuildElementEvent>(OnRebuildElement);
			nodeElement.RegisterCallback<UndoRedoPerformedEvent>(OnUndoRedoPerformed);

			var graphEditor = nodeEditor.graphEditor as BehaviourTreeGraphEditor;
			if (graphEditor != null)
			{
				graphEditor.onChangedEditable -= OnChangedEditable;
				graphEditor.onChangedEditable += OnChangedEditable;

				graphEditor.onChangedDragBranchHover -= OnChanedDragBranchHover;
				graphEditor.onChangedDragBranchHover += OnChanedDragBranchHover;
			}

			foreach (var element in _RegisterElements)
			{
				element.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedParent);
			}
			_RegisterElements.Clear();

			var current = this.hierarchy.parent;
			while (current != null && current != nodeElement)
			{
				current.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedParent);

				_RegisterElements.Add(current);

				current = current.hierarchy.parent;
			}

			UpdateOn();
		}

		protected virtual void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			NodeElement nodeElement = nodeEditor.nodeElement;
			nodeElement.UnregisterCallback<ChangeNodePositionEvent>(OnChangeNodePosition);
			nodeElement.UnregisterCallback<RebuildElementEvent>(OnRebuildElement);
			nodeElement.UnregisterCallback<UndoRedoPerformedEvent>(OnUndoRedoPerformed);

			var graphEditor = nodeEditor.graphEditor as BehaviourTreeGraphEditor;
			if (graphEditor != null)
			{
				graphEditor.onChangedEditable -= OnChangedEditable;
				graphEditor.onChangedDragBranchHover -= OnChanedDragBranchHover;
			}

			foreach (var element in _RegisterElements)
			{
				element.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedParent);
			}
			_RegisterElements.Clear();
		}

		void OnGeometryChangedParent(GeometryChangedEvent e)
		{
			DoChangedPosition();
		}

		protected virtual void OnRebuildElement(RebuildElementEvent e)
		{
			SetNode(nodeEditor.node);

			DoChangedPosition();
		}

		void OnChangeNodePosition(ChangeNodePositionEvent e)
		{
			DoChangedPosition();
		}

		void OnUndoRedoPerformed(UndoRedoPerformedEvent e)
		{
			DoChangedPosition();

			UpdateOn();
		}

		void OnChangedEditable(bool editable)
		{
			SetEnabled(editable);

			if (!editable)
			{
				CancelDrag();
			}
		}

		void OnChanedDragBranchHover(int nodeID)
		{
			isDragHover = nodeEditor.nodeID == nodeID && graphEditor.IsDragParentSlot() != _IsParentSlot;
		}

		protected void CancelDrag()
		{
			var graphEditor = this.graphEditor;
			if (graphEditor == null)
			{
				return;
			}

			if (_ConnectManipulator.isActive)
			{
				_ConnectManipulator.EndDrag();
			}
		}

		protected void DoChangedPosition()
		{
			if (_IsLayouted)
			{
				OnChangedPosition();
				graphEditor.UpdateHighlight(this);
			}
		}

		void OnContextClick(ContextClickEvent e)
		{
			GenericMenu menu = new GenericMenu();

			if (IsConnecting())
			{
				menu.AddItem(GetDisconnectContent(), false, OnDisconnect);
			}
			else
			{
				menu.AddDisabledItem(GetDisconnectContent());
			}

			menu.ShowAsContext();

			e.StopPropagation();
		}

		private NodeLinkSlot _NodeLinkSlot;

		protected void SetNodeLinkSlot(NodeLinkSlot nodeLinkSlot)
		{
			if (_NodeLinkSlot != nodeLinkSlot)
			{
				if (_NodeLinkSlot != null)
				{
					_NodeLinkSlot.onConnectionChanged -= OnConnectionChanged;
				}

				_NodeLinkSlot = nodeLinkSlot;

				if (_NodeLinkSlot != null)
				{
					_NodeLinkSlot.onConnectionChanged += OnConnectionChanged;
				}
			}
		}

		protected virtual void OnConnectionChanged()
		{
			DoChangedPosition();

			UpdateOn();
		}

		void UpdateOn()
		{
			on = IsConnecting();
		}

		protected abstract void SetNode(Node node);

		protected abstract bool IsConnecting();
		protected abstract bool IsConnected(int hoverID);

		protected abstract void OnChangedPosition();
		protected abstract void OnDisconnect();

		protected virtual bool CanStartDrag()
		{
			return true;
		}

		protected virtual void OnPreConnect()
		{
		}

		sealed class ConnectManipulator : DragOnGraphManipulator
		{
			private Bezier2D _DragBezier = new Bezier2D();

			public ConnectManipulator()
			{
				activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
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

				UpdateMousePosition(graphView.mousePosition);
			}

			private bool _Dragging = false;

			protected override void OnMouseDown(MouseDownEvent e)
			{
				NodeLinkSlotElement slotElement = target as NodeLinkSlotElement;
				if (slotElement == null)
				{
					return;
				}

				BehaviourTreeGraphEditor graphEditor = slotElement.graphEditor;
				if (graphEditor != null && slotElement.CanStartDrag())
				{
					graphEditor.BeginDragBranch(slotElement.nodeEditor.nodeID, slotElement._IsParentSlot);

					Vector2 pinPos = GetNodeLinkPinPos(slotElement.layout);
					pinPos = graphEditor.graphView.ElementToGraph(slotElement.parent, pinPos);
					Vector2 mousePos = graphEditor.graphView.ElementToGraph(slotElement, e.localMousePosition);

					if (slotElement._IsParentSlot)
					{
						_DragBezier.startPosition = mousePos;
						_DragBezier.endPosition = pinPos;
					}
					else
					{
						_DragBezier.startPosition = pinPos;
						_DragBezier.endPosition = mousePos;
					}
					_DragBezier.startControl = _DragBezier.startPosition + kBezierTangentOffset;
					_DragBezier.endControl = _DragBezier.endPosition - kBezierTangentOffset;

					graphEditor.DragBranchBezie(_DragBezier);

					_Dragging = true;
				}
				else
				{
					_Dragging = false;
				}

				e.StopPropagation();
			}

			void UpdateMousePosition(Vector2 mousePos)
			{
				NodeLinkSlotElement slotElement = target as NodeLinkSlotElement;
				if (slotElement == null)
				{
					return;
				}

				BehaviourTreeGraphEditor graphEditor = slotElement.graphEditor;
				GraphView graphView = graphEditor.graphView;
				Node hoverNode = slotElement._IsParentSlot ? graphEditor.GetHoverParentNode(slotElement.nodeEditor.treeNode, mousePos) : graphEditor.GetHoverChildNode(slotElement.nodeEditor.treeNode, mousePos);
				Vector2 hoverPin = Vector2.zero;
				if (hoverNode != null)
				{
					graphEditor.DragBranchHoverID(hoverNode.nodeID);

					TreeNodeBaseEditor hoverEditor = graphEditor.GetNodeEditor(hoverNode) as TreeNodeBaseEditor;
					if (slotElement._IsParentSlot)
					{
						hoverPin = GetNodeLinkPinPos(hoverEditor.childLinkSlotPosition);
					}
					else
					{
						hoverPin = GetNodeLinkPinPos(hoverEditor.parentLinkSlotPosition);
					}
				}
				else
				{
					graphEditor.DragBranchHoverID(0);

					hoverPin = mousePos;
				}

				if (slotElement._IsParentSlot)
				{
					_DragBezier.startPosition = hoverPin;
					_DragBezier.startControl = _DragBezier.startPosition + kBezierTangentOffset;
				}
				else
				{
					_DragBezier.endPosition = hoverPin;
					_DragBezier.endControl = _DragBezier.endPosition - kBezierTangentOffset;
				}

				graphEditor.DragBranchBezie(_DragBezier);
			}

			protected override void OnMouseMove(MouseMoveEvent e)
			{
				NodeLinkSlotElement slotElement = target as NodeLinkSlotElement;
				if (slotElement == null)
				{
					return;
				}

				BehaviourTreeGraphEditor graphEditor = slotElement.graphEditor;
				if (graphEditor != null && _Dragging)
				{
					DragAndDrop.PrepareStartDrag();

					GraphView graphView = graphEditor.graphView;
					Vector2 mousePos = graphView.ElementToGraph(slotElement, e.localMousePosition);

					UpdateMousePosition(mousePos);
				}

				e.StopPropagation();
			}

			protected override void OnMouseUp(MouseUpEvent e)
			{
				NodeLinkSlotElement slotElement = target as NodeLinkSlotElement;
				if (slotElement == null)
				{
					return;
				}

				BehaviourTreeGraphEditor graphEditor = slotElement.graphEditor;
				if (graphEditor != null && _Dragging)
				{
					BehaviourTreeInternal behaviourTree = graphEditor.nodeGraph as BehaviourTreeInternal;

					int hoverID = graphEditor.GetDragBranchHoverID();
					TreeNodeBase hoverNode = behaviourTree.GetNodeFromID(hoverID) as TreeNodeBase;

					if (hoverNode != null)
					{
						if (!slotElement.IsConnected(hoverID))
						{
							TreeNodeBase parentNode = slotElement._IsParentSlot ? hoverNode : slotElement.nodeEditor.treeNode;
							TreeNodeBase childNode = slotElement._IsParentSlot ? slotElement.nodeEditor.treeNode : hoverNode;
							if (behaviourTree.CheckLoop(parentNode, childNode))
							{
								Debug.LogError("Node has become an infinite loop.");
							}
							else
							{
								slotElement.OnPreConnect();

								NodeBranch branch = behaviourTree.ConnectBranch(parentNode, childNode);

								if (branch != null)
								{
									branch.bezier = new Bezier2D(_DragBezier);
									graphEditor.Repaint();
								}

								behaviourTree.CalculatePriority();
							}
						}
					}
					else
					{
						GenericMenu menu = new GenericMenu();

						Vector2 screenPoint = slotElement.LocalToScreen(e.localMousePosition);

						Vector2 graphPoint = graphEditor.graphView.ScreenToGraph(screenPoint);

						Rect buttonRect = new Rect(screenPoint, Vector2.zero);

						menu.AddItem(EditorContents.createComposite, false, () =>
						{
							graphEditor.ShowDragBranch();
							graphEditor._DragBranchEnable = true;

							CompositeBehaviourMenuWindow.instance.Init(graphPoint, buttonRect, OnSelectComposite, null, OnCancelSelectType);
						});

						if (!slotElement._IsParentSlot)
						{
							menu.AddItem(EditorContents.createAction, false, () =>
							{
								graphEditor.ShowDragBranch();
								graphEditor._DragBranchEnable = true;

								ActionBehaviourMenuWindow.instance.Init(graphPoint, buttonRect, OnSelectAction, null, OnCancelSelectType);
							});
						}

						if (slotElement.IsConnecting())
						{
							menu.AddSeparator("");
							menu.AddItem(slotElement.GetDisconnectContent(), false, slotElement.OnDisconnect);
						}

						menu.ShowAsContext();
					}
				}
			}

			protected override void OnEndDrag()
			{
				base.OnEndDrag();

				NodeLinkSlotElement slotElement = target as NodeLinkSlotElement;
				if (slotElement == null)
				{
					return;
				}

				BehaviourTreeGraphEditor graphEditor = slotElement.graphEditor;
				if (graphEditor != null)
				{
					graphEditor.EndDragBranch();
				}

				_Dragging = false;
			}

			void OnKeyDown(KeyDownEvent e)
			{
				if (!isActive || e.keyCode != KeyCode.Escape)
				{
					return;
				}

				EndDrag();
				e.StopPropagation();
			}

			void OnCancelSelectType()
			{
				NodeLinkSlotElement slotElement = target as NodeLinkSlotElement;
				if (slotElement == null)
				{
					return;
				}

				BehaviourTreeGraphEditor graphEditor = slotElement.graphEditor;
				graphEditor.EndDragBranch();
			}

			void OnSelectComposite(Vector2 pos, System.Type classType)
			{
				NodeLinkSlotElement slotElement = target as NodeLinkSlotElement;
				if (slotElement == null)
				{
					return;
				}

				BehaviourTreeGraphEditor graphEditor = slotElement.graphEditor;
				BehaviourTreeInternal behaviourTree = graphEditor.nodeGraph as BehaviourTreeInternal;

				Undo.IncrementCurrentGroup();
				int undoGroup = Undo.GetCurrentGroup();

				pos -= new Vector2(Node.defaultWidth * 0.5f, slotElement._IsParentSlot ? kDefaultNodeHeight : 0f);

				CompositeNode compositeNode = graphEditor.CreateComposite(pos, classType);

				slotElement.OnPreConnect();

				TreeNodeBase parentNode = slotElement._IsParentSlot ? compositeNode : slotElement.nodeEditor.treeNode;
				TreeNodeBase childNode = slotElement._IsParentSlot ? slotElement.nodeEditor.treeNode : compositeNode;

				NodeBranch branch = behaviourTree.ConnectBranch(parentNode, childNode);
				if (branch != null)
				{
					branch.bezier = new Bezier2D(_DragBezier);
					graphEditor.Repaint();
				}

				behaviourTree.CalculatePriority();

				Undo.CollapseUndoOperations(undoGroup);
			}

			void OnSelectAction(Vector2 pos, System.Type classType)
			{
				NodeLinkSlotElement slotElement = target as NodeLinkSlotElement;
				if (slotElement == null)
				{
					return;
				}

				BehaviourTreeGraphEditor graphEditor = slotElement.graphEditor;
				BehaviourTreeInternal behaviourTree = graphEditor.nodeGraph as BehaviourTreeInternal;

				Undo.IncrementCurrentGroup();
				int undoGroup = Undo.GetCurrentGroup();

				pos -= new Vector2(Node.defaultWidth * 0.5f, 0f);

				ActionNode actionNode = graphEditor.CreateAction(pos, classType);

				slotElement.OnPreConnect();

				NodeBranch branch = behaviourTree.ConnectBranch(slotElement.nodeEditor.treeNode, actionNode);
				if (branch != null)
				{
					branch.bezier = new Bezier2D(_DragBezier);
					graphEditor.Repaint();
				}

				behaviourTree.CalculatePriority();

				Undo.CollapseUndoOperations(undoGroup);
			}
		}
	}
}