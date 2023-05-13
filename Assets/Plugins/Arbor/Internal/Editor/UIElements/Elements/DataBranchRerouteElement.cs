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

	internal sealed class DataBranchRerouteElement : VisualElement
	{
		private DataBranchRerouteNodeEditor _NodeEditor;

		private VisualElement _PinElement;
		private Color _PinColor = Color.white;

		private bool _On;
		private bool _IsDragHover;

		private ConnectManipulator _ConnectManipulator;

		public bool on
		{
			get
			{
				return _On;
			}
			set
			{
				if (_On != value)
				{
					_On = value;

					EnableInClassList("data-slot-on", _On);
					EnableInClassList("reroute-link-slot-on", _On);
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
					UpdatePinElementColor();
				}
			}
		}

		void UpdatePinElementColor()
		{
			if (_IsDragHover || _ConnectManipulator.isActive)
			{
				_PinElement.style.unityBackgroundImageTintColor = StyleKeyword.Null;
			}
			else
			{
				_PinElement.style.unityBackgroundImageTintColor = _PinColor;
			}
		}

		public Color pinColor
		{
			get
			{
				return _PinColor;
			}
		}

		public DataBranchRerouteElement(DataBranchRerouteNodeEditor nodeEditor)
		{
			_NodeEditor = nodeEditor;

			AddToClassList("data-slot");
			AddToClassList("reroute-link-slot");

			_PinElement = new VisualElement();
			_PinElement.AddToClassList("pin");
			Add(_PinElement);

			_ConnectManipulator = new ConnectManipulator();
			_ConnectManipulator.onChangedActive += isActive =>
			{
				EnableInClassList("data-slot-active", isActive);
				EnableInClassList("reroute-link-slot-active", isActive);
				UpdatePinElementColor();
			};
			this.AddManipulator(_ConnectManipulator);

			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

			var graphEditor = nodeEditor.graphEditor;
			if (graphEditor != null)
			{
				SetEnabled(graphEditor.editable);
			}

			var dataBranchRerouteNode = nodeEditor.dataBranchRerouteNode;
			RerouteSlot slot = dataBranchRerouteNode.link;

			SetSlot(slot);
			UpdateOn();
			UpdateType();
		}

		private bool _IsLayouted = true;

		void OnGeometryChanged(GeometryChangedEvent e)
		{
			VisualElement target = e.target as VisualElement;
			if (UIElementsUtility.IsVisible(target))
			{
				_IsLayouted = true;
				DoChangedPosition();
			}
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			var nodeElement = _NodeEditor.nodeElement;
			nodeElement.RegisterCallback<ChangeNodePositionEvent>(OnChangeNodePosition);
			nodeElement.RegisterCallback<RebuildElementEvent>(OnRebuildElement);
			nodeElement.RegisterCallback<UndoRedoPerformedEvent>(OnUndoRedoPerformed);

			var graphEditor = _NodeEditor.graphEditor;
			if (graphEditor != null)
			{
				graphEditor.onChangedEditable -= OnChangedEditable;
				graphEditor.onChangedEditable += OnChangedEditable;
			}

			DataSlotGUI.onHoverChanged -= OnHoverChanged;
			DataSlotGUI.onHoverChanged += OnHoverChanged;
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			var nodeElement = _NodeEditor.nodeElement;
			nodeElement.UnregisterCallback<ChangeNodePositionEvent>(OnChangeNodePosition);
			nodeElement.UnregisterCallback<RebuildElementEvent>(OnRebuildElement);
			nodeElement.UnregisterCallback<UndoRedoPerformedEvent>(OnUndoRedoPerformed);

			var graphEditor = _NodeEditor.graphEditor;
			if (graphEditor != null)
			{
				graphEditor.onChangedEditable -= OnChangedEditable;
			}

			DataSlotGUI.onHoverChanged -= OnHoverChanged;
		}

		private RerouteSlot _Slot;

		void SetSlot(RerouteSlot slot)
		{
			if (_Slot != slot)
			{
				if (_Slot != null)
				{
					_Slot.onConnectionChanged -= OnConnectionChanged;
				}

				_Slot = slot;

				if (_Slot != null)
				{
					_Slot.onConnectionChanged += OnConnectionChanged;
				}
			}
		}

		void OnConnectionChanged(bool connected)
		{
			UpdateOn();
		}

		void OnHoverChanged(DataSlot hoverSlot)
		{
			var dataBranchRerouteNode = _NodeEditor.dataBranchRerouteNode;
			RerouteSlot slot = dataBranchRerouteNode.link;

			isDragHover = slot == hoverSlot;
		}

		void UpdateOn()
		{
			var dataBranchRerouteNode = _NodeEditor.dataBranchRerouteNode;
			RerouteSlot slot = dataBranchRerouteNode.link;

			on = slot.outputBranchIDs.Count > 0;
		}

		void UpdateType()
		{
			var dataBranchRerouteNode = _NodeEditor.dataBranchRerouteNode;
			RerouteSlot slot = dataBranchRerouteNode.link;

			System.Type dataType = slot.connectableType;
			_PinElement.EnableInClassList("pin-array", dataType != null && DataSlotGUIUtility.IsList(dataType));

			tooltip = slot.connectableTypeName;

			var pinColor = EditorGUITools.GetTypeColor(dataType);
			if (_PinColor != pinColor)
			{
				_PinColor = pinColor;
				UpdatePinElementColor();
			}
		}

		void OnRebuildElement(RebuildElementEvent e)
		{
			SetSlot(_NodeEditor.dataBranchRerouteNode.link);
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
		}

		void OnChangedPosition()
		{
			var graphEditor = _NodeEditor.graphEditor;
			var dataBranchRerouteNode = _NodeEditor.dataBranchRerouteNode;

			bool isRepaint = false;

			GraphView graphView = graphEditor.graphView;

			Rect pinPos = graphView.ElementToGraph(this, contentRect);

			dataBranchRerouteNode.link.position = pinPos;

			Vector2 pinPosition = pinPos.center;
			Vector2 startPinPosition = pinPosition + dataBranchRerouteNode.direction * 8f;
			Vector2 endPinPosition = pinPosition - dataBranchRerouteNode.direction * 8f;

			IInputSlot inputSlot = dataBranchRerouteNode.link.inputSlot;

			DataBranch inputBranch = inputSlot.GetBranch();
			if (inputBranch != null)
			{
				if (inputBranch.lineBezier.endPosition != endPinPosition)
				{
					inputBranch.lineBezier.endPosition = endPinPosition;
					isRepaint = true;
				}
				Vector2 endControl = inputBranch.lineBezier.endPosition - dataBranchRerouteNode.direction * EditorGUITools.kBezierTangent;
				if (inputBranch.lineBezier.endControl != endControl)
				{
					inputBranch.lineBezier.endControl = endControl;
					isRepaint = true;
				}
			}

			IOutputSlot outputSlot = dataBranchRerouteNode.link.outputSlot;
			int outBranchCount = outputSlot.branchCount;
			for (int i = 0; i < outBranchCount; i++)
			{
				DataBranch outputBranch = outputSlot.GetBranch(i);
				if (outputBranch != null)
				{
					if (outputBranch.lineBezier.startPosition != startPinPosition)
					{
						outputBranch.lineBezier.startPosition = startPinPosition;
						isRepaint = true;
					}
					Vector2 startControl = outputBranch.lineBezier.startPosition + dataBranchRerouteNode.direction * EditorGUITools.kBezierTangent;
					if (outputBranch.lineBezier.startControl != startControl)
					{
						outputBranch.lineBezier.startControl = startControl;
						isRepaint = true;
					}
				}
			}

			if (isRepaint)
			{
				graphEditor.Repaint();
			}
		}

		public void DoChangedPosition()
		{
			if (_IsLayouted)
			{
				OnChangedPosition();
			}
		}

		sealed class ConnectManipulator : DragOnGraphManipulator
		{
			private Bezier2D _Bezier = new Bezier2D();

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
				if (graphView == null)
				{
					return;
				}

				UpdateMousePosition(graphView.mousePosition);
			}

			void UpdateBezier(Vector2 mousePosition)
			{
				DataBranchRerouteElement linkElement = target as DataBranchRerouteElement;
				if (linkElement == null)
				{
					return;
				}

				var nodeEditor = linkElement._NodeEditor;
				var graphEditor = nodeEditor.graphEditor;
				var graphView = graphEditor.graphView;
				var node = nodeEditor.dataBranchRerouteNode;

				Rect pinPos = graphView.ElementToGraph(linkElement, linkElement.contentRect);

				_Bezier.startPosition = pinPos.center;
				_Bezier.startControl = _Bezier.startPosition + node.direction * EditorGUITools.kBezierTangent;

				DataSlot hoverSlot = DataSlotGUI._HoverSlot;
				NodeEditor hoverNodeEditor = DataSlotGUI._HoverNodeEditor;
				Node hoverNode = hoverNodeEditor != null? hoverNodeEditor.node : null;
				Object hoverObj = DataSlotGUI._HoverObj;
				if (hoverSlot != null)
				{
					DataBranchRerouteNode hoverRerouteNode = hoverNode as DataBranchRerouteNode;
					if (hoverRerouteNode != null)
					{
						_Bezier.endPosition = hoverSlot.position.center;
						_Bezier.endControl = _Bezier.endPosition - hoverRerouteNode.direction * EditorGUITools.kBezierTangent;
					}
					else
					{
						if (ArborSettings.dataSlotShowMode == DataSlotShowMode.Inside)
						{
							_Bezier.endPosition = new Vector2(hoverNodeEditor.position.x, hoverSlot.position.center.y);
						}
						else
						{
							_Bezier.endPosition = new Vector2(hoverSlot.position.x + 8, hoverSlot.position.center.y);
						}
						_Bezier.endControl = _Bezier.endPosition - EditorGUITools.kBezierTangentOffset;
					}
				}
				else
				{
					_Bezier.endPosition = mousePosition;
					_Bezier.endControl = _Bezier.endPosition - EditorGUITools.kBezierTangentOffset;
				}
			}

			protected override void OnMouseDown(MouseDownEvent e)
			{
				DataBranchRerouteElement linkElement = target as DataBranchRerouteElement;
				if (linkElement == null)
				{
					return;
				}

				var nodeEditor = linkElement._NodeEditor;
				var graphEditor = nodeEditor.graphEditor;
				var graphView = graphEditor.graphView;
				var node = nodeEditor.dataBranchRerouteNode;
				var slot = node.link;

				DataSlotGUI.BeginDragSlot(node, slot, null);

				Vector2 graphMousePosition = graphView.ElementToGraph((e.currentTarget as VisualElement), e.localMousePosition);
				UpdateBezier(graphMousePosition);

				graphEditor.BeginDragDataBranch(node.nodeID);
				graphEditor.DragDataBranchBezier(_Bezier.startPosition, _Bezier.startControl, _Bezier.endPosition, _Bezier.endControl);

				e.StopPropagation();
			}

			void UpdateMousePosition(Vector2 graphMousePosition)
			{
				DataBranchRerouteElement linkElement = target as DataBranchRerouteElement;
				if (linkElement == null)
				{
					return;
				}

				var nodeEditor = linkElement._NodeEditor;
				var graphEditor = nodeEditor.graphEditor;
				var node = nodeEditor.dataBranchRerouteNode;
				var slot = node.link;

				DataSlotGUI.UpdateHoverSlot(graphEditor, nodeEditor, null, slot, graphMousePosition);

				UpdateBezier(graphMousePosition);

				graphEditor.DragDataBranchBezier(_Bezier.startPosition, _Bezier.startControl, _Bezier.endPosition, _Bezier.endControl);
			}

			protected override void OnMouseMove(MouseMoveEvent e)
			{
				DataBranchRerouteElement linkElement = target as DataBranchRerouteElement;
				if (linkElement == null)
				{
					return;
				}

				var nodeEditor = linkElement._NodeEditor;
				var graphEditor = nodeEditor.graphEditor;
				var graphView = graphEditor.graphView;
				
				DragAndDrop.PrepareStartDrag();

				Vector2 graphMousePosition = graphView.ElementToGraph((e.currentTarget as VisualElement), e.localMousePosition);
				UpdateMousePosition(graphMousePosition);
			}

			protected override void OnMouseUp(MouseUpEvent e)
			{
				DataBranchRerouteElement linkElement = target as DataBranchRerouteElement;
				if (linkElement == null)
				{
					return;
				}

				var nodeEditor = linkElement._NodeEditor;
				var graphEditor = nodeEditor.graphEditor;
				var graphView = graphEditor.graphView;
				var node = nodeEditor.dataBranchRerouteNode;
				var slot = node.link;

				Vector2 graphMousePosition = graphView.ElementToGraph((e.currentTarget as VisualElement), e.localMousePosition);
				UpdateBezier(graphMousePosition);

				DataBranch branch = null;

				DataSlot hoverSlot = DataSlotGUI._HoverSlot;
				NodeEditor hoverNodeEditor = DataSlotGUI._HoverNodeEditor;
				Object hoverObj = DataSlotGUI._HoverObj;

				if (hoverSlot == null)
				{
					GenericMenu menu = new GenericMenu();

					NodeGraph nodeGraph = node.nodeGraph;

					DataBranch currentBranch = branch;

					menu.AddItem(EditorContents.reroute, false, () =>
					{
						if (currentBranch != null)
						{
							graphEditor.DeleteDataBranch(currentBranch);
							currentBranch = null;
						}

						Undo.IncrementCurrentGroup();
						int undoGroup = Undo.GetCurrentGroup();

						graphMousePosition -= new Vector2(16f, 16f);

						DataBranchRerouteNode newRerouteNode = graphEditor.CreateDataBranchRerouteNode(EditorGUITools.SnapToGrid(graphMousePosition), slot.dataType);

						Undo.RecordObject(nodeGraph, "Reroute");

						RerouteSlot rerouteSlot = newRerouteNode.link;

						currentBranch = graphEditor.ConnectDataBranch(newRerouteNode.nodeID, null, rerouteSlot, node.nodeID, null, slot);
						if (currentBranch != null)
						{
							currentBranch.enabled = true;
							currentBranch.lineBezier = new Bezier2D(_Bezier);
						}

						Undo.CollapseUndoOperations(undoGroup);

						EditorUtility.SetDirty(nodeGraph);
					});

					menu.ShowAsContext();
				}
				else if (hoverSlot != null)
				{
					NodeGraph nodeGraph = node.nodeGraph;

					InputSlotBase inputSlot = hoverSlot as InputSlotBase;

					if (inputSlot != null)
					{
						branch = nodeGraph.GetDataBranchFromID(inputSlot.branchID);
					}
					else
					{
						RerouteSlot rerouteSlot = hoverSlot as RerouteSlot;
						if (rerouteSlot != null)
						{
							branch = nodeGraph.GetDataBranchFromID(rerouteSlot.inputBranchID);
						}
					}

					if (branch != null)
					{
						graphEditor.DeleteDataBranch(branch);
						branch = null;
					}

					branch = graphEditor.ConnectDataBranch(hoverNodeEditor.nodeID, hoverObj, hoverSlot, node.nodeID, null, slot);
					if (branch != null)
					{
						branch.lineBezier = new Bezier2D(_Bezier);
					}
				}

				if (branch != null)
				{
					branch.enabled = true;
				}
			}

			protected override void OnEndDrag()
			{
				base.OnEndDrag();

				DataBranchRerouteElement linkElement = target as DataBranchRerouteElement;
				if (linkElement == null)
				{
					return;
				}

				var nodeEditor = linkElement._NodeEditor;
				var graphEditor = nodeEditor.graphEditor;

				DataSlotGUI.EndDragSlot();
				graphEditor.EndDragDataBranch();

				DataSlotGUI.ClearHoverSlot();
			}

			void OnKeyDown(KeyDownEvent e)
			{
				DataBranchRerouteElement linkElement = target as DataBranchRerouteElement;
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