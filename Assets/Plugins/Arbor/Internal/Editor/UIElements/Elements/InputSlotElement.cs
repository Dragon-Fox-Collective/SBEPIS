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

	internal sealed class InputSlotElement : VisualElement
	{
		private BehaviourEditorGUI _BehaviourEditorGUI;

		private DataSlotGUI _SlotGUI;

		private InputSlotBase _InputSlot;

		public DataSlotGUI slotGUI
		{
			get
			{
				return _SlotGUI;
			}
			set
			{
				if (_SlotGUI != value)
				{
					_SlotGUI = value;

					SetInputSlot(_SlotGUI?.slot as InputSlotBase);
				}
			}
		}

		void SetInputSlot(InputSlotBase inputSlot)
		{
			if (_InputSlot != inputSlot)
			{
				if (_InputSlot != null)
				{
					_InputSlot.onConnectionChanged -= OnConnectionChanged;
					_InputSlot.onConnectableTypeChanged -= OnConnectableTypeChanged;
				}

				_InputSlot = inputSlot;

				if (_InputSlot != null)
				{
					_InputSlot.onConnectionChanged += OnConnectionChanged;
					_InputSlot.onConnectableTypeChanged += OnConnectableTypeChanged;

					OnHoverChanged(DataSlotGUI._HoverSlot);
					UpdateType();
					UpdateOn();
				}
			}
		}

		private PolyLineElement _ConnectLines;
		private VisualElement _PinElement;
		private Label _Label;

		private ConnectManipulator _ConnectManipulator;

		private Color _SlotColor = Color.white;

		private bool _On;
		private bool _IsDragHover;

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

					_ConnectLines.visible = value;

					EnableInClassList("data-slot-on", _On);
					EnableInClassList("data-link-slot-on", _On);
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
					_Label.EnableInClassList("content-label-active", _IsDragHover);
					_PinElement.EnableInClassList("pin-active", _IsDragHover);
					UpdateSlotColor();
				}
			}
		}

		public string label
		{
			get
			{
				return _Label.text;
			}
			set
			{
				_Label.text = value;
			}
		}

		public InputSlotElement()
		{
			AddToClassList("data-slot");
			AddToClassList("data-link-slot");
			AddToClassList("input-slot");

			_ConnectLines = new PolyLineElement()
			{
				edgeWidth = 8f,
				visible = false,
			};
			Add(_ConnectLines);

			_PinElement = new VisualElement();
			_PinElement.AddToClassList("pin");
			_PinElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedPin);
			Add(_PinElement);

			_Label = new Label();
			_Label.AddToClassList("content-label");
			Add(_Label);

			_ConnectManipulator = new ConnectManipulator();
			_ConnectManipulator.onChangedActive += isActive =>
			{
				EnableInClassList("data-slot-active", isActive);
				EnableInClassList("data-link-slot-active", isActive);
				UpdateSlotColor();
			};
			this.AddManipulator(_ConnectManipulator);

			this.AddManipulator(new ContextClickManipulator(OnContextClick));

			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
		}

		public void Set(BehaviourEditorGUI behaviourEditorGUI)
		{
			_BehaviourEditorGUI = behaviourEditorGUI;
		}

		private bool _IsLayouted = false;

		void OnContextClick(ContextClickEvent e)
		{
			var inputSlot = slotGUI.slot as InputSlotBase;
			var branch = inputSlot.branch;

			var graphEditor = _BehaviourEditorGUI.nodeEditor.graphEditor;

			GenericMenu menu = new GenericMenu();

			if (branch != null)
			{
				menu.AddItem(EditorContents.disconnect, false, () =>
				{
					graphEditor.DeleteDataBranch(branch);
					branch = null;
				});
			}
			else
			{
				menu.AddDisabledItem(EditorContents.disconnect);
			}

			menu.ShowAsContext();

			e.StopPropagation();
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

		void OnGeometryChangedPin(GeometryChangedEvent e)
		{
			Vector2 pinPos = e.newRect.center;
			Vector2 endPos = new Vector2(paddingRect.x, paddingRect.center.y);

			_ConnectLines.SetPoints(pinPos, endPos);
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			var nodeEditor = _BehaviourEditorGUI.nodeEditor;
			var nodeElement = nodeEditor.nodeElement;
			nodeElement.RegisterCallback<ChangeNodePositionEvent>(OnChangeNodePosition);
			nodeElement.RegisterCallback<RebuildElementEvent>(OnRebuildElement);
			nodeElement.RegisterCallback<UndoRedoPerformedEvent>(OnUndoRedoPerformed);

			DataSlotGUI.onHoverChanged -= OnHoverChanged;
			DataSlotGUI.onHoverChanged += OnHoverChanged;

			SetInputSlot(slotGUI?.slot as InputSlotBase);
		}

		void OnHoverChanged(DataSlot hoverSlot)
		{
			var slot = slotGUI.slot;
			isDragHover = slot == hoverSlot;
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			var nodeEditor = _BehaviourEditorGUI.nodeEditor;
			var nodeElement = nodeEditor.nodeElement;
			nodeElement.UnregisterCallback<ChangeNodePositionEvent>(OnChangeNodePosition);
			nodeElement.UnregisterCallback<RebuildElementEvent>(OnRebuildElement);
			nodeElement.UnregisterCallback<UndoRedoPerformedEvent>(OnUndoRedoPerformed);

			DataSlotGUI.onHoverChanged -= OnHoverChanged;

			SetInputSlot(null);
		}

		void OnRebuildElement(RebuildElementEvent e)
		{
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

		void OnChangedPosition()
		{
			var slot = slotGUI.slot;
			var inputSlot = slot as InputSlotBase;

			var graphEditor = _BehaviourEditorGUI.nodeEditor.graphEditor;
			var graphView = graphEditor != null? graphEditor.graphView : null;

			if (graphView != null)
			{
				slot.position = graphView.ElementToGraph(parent, layout);
			}

			var branch = inputSlot.branch;
			if (branch == null)
			{
				return;
			}

			var pinPos = new Vector2(paddingRect.x, paddingRect.center.y);
			pinPos = graphView.ElementToGraph(this, pinPos);
			if (ArborSettings.dataSlotShowMode == DataSlotShowMode.Inside)
			{
				pinPos.x = _BehaviourEditorGUI.nodeEditor.position.x;
			}
			var pinControlPos = pinPos - EditorGUITools.kBezierTangentOffset;

			branch.lineBezier.endPosition = pinPos;
			branch.lineBezier.endControl = pinControlPos;
		}

		internal void DoChangedPosition()
		{
			if (_IsLayouted)
			{
				OnChangedPosition();
			}
		}

		void OnConnectableTypeChanged()
		{
			EditorApplication.delayCall += UpdateType;
		}

		void OnConnectionChanged(bool connected)
		{
			UpdateOn();
		}

		public void UpdateOn()
		{
			var slot = slotGUI.slot;

			var inputSlot = slot as InputSlotBase;
			on = inputSlot.branch != null;
		}

		void UpdateType()
		{
			var slot = slotGUI.slot;

			System.Type dataType = slot.connectableType;
			_PinElement.EnableInClassList("pin-array", dataType != null && DataSlotGUIUtility.IsList(dataType));

			tooltip = slot.connectableTypeName;

			var slotColor = EditorGUITools.GetTypeColor(dataType);
			if (_SlotColor != slotColor)
			{
				_SlotColor = slotColor;
				UpdateSlotColor();
			}
		}

		static readonly CustomStyleProperty<Color> s_ColorsPinBackgroundPressedProperty = new CustomStyleProperty<Color>("--colors-pin-background-pressed");

		void UpdateSlotColor()
		{
			Color slotColor = _SlotColor;

			bool isDragging = _ConnectManipulator.isActive;

			if (_IsDragHover || isDragging)
			{
				_PinElement.style.unityBackgroundImageTintColor = StyleKeyword.Null;
				if (customStyle.TryGetValue(s_ColorsPinBackgroundPressedProperty, out var styleColor))
				{
					_ConnectLines.lineColor = styleColor;
				}
			}
			else
			{
				_PinElement.style.unityBackgroundImageTintColor = slotColor;
				_ConnectLines.lineColor = slotColor;
			}

			if (isDragging)
			{
				style.unityBackgroundImageTintColor = StyleKeyword.Null;
			}
			else
			{
				style.unityBackgroundImageTintColor = slotColor = EditorGUITools.GetSlotBackgroundColor(slotColor, false, _On);
			}	
		}

		sealed class ConnectManipulator : DragOnGraphManipulator
		{
			Bezier2D _Bezier = new Bezier2D();

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
				if (graphView == null)
				{
					return;
				}

				UpdateMousePosition(graphView.mousePosition);
			}

			void UpdateBezier(Vector2 graphMousePosition)
			{
				InputSlotElement slotElement = target as InputSlotElement;
				if (slotElement == null)
				{
					return;
				}

				var behaviourEditorGUI = slotElement._BehaviourEditorGUI;
				var nodeEditor = behaviourEditorGUI.nodeEditor;
				var graphEditor = nodeEditor.graphEditor;
				var graphView = graphEditor.graphView;
				var node = nodeEditor.node;

				var slot = slotElement.slotGUI.slot;

				Rect position = slotElement.paddingRect;
				Vector2 pinPos = graphView.ElementToGraph(slotElement, new Vector2(position.x, position.center.y));
				if (ArborSettings.dataSlotShowMode == DataSlotShowMode.Inside)
				{
					pinPos.x = nodeEditor.position.x;
				}
				Vector2 pinControlPos = pinPos - EditorGUITools.kBezierTangentOffset;

				Vector2 targetPos = graphMousePosition;
				Vector2 targetControlPos = targetPos + EditorGUITools.kBezierTangentOffset;

				var hoverNodeEditor = DataSlotGUI._HoverNodeEditor;
				var hoverNode = hoverNodeEditor != null? hoverNodeEditor.node : null;
				var hoverSlot = DataSlotGUI._HoverSlot;
				if (hoverSlot != null)
				{
					DataBranchRerouteNode hoverRerouteNode = hoverNode as DataBranchRerouteNode;
					if (hoverRerouteNode != null)
					{
						targetPos = hoverSlot.position.center;
						targetControlPos = targetPos + hoverRerouteNode.direction * EditorGUITools.kBezierTangent;
					}
					else
					{
						targetPos = new Vector2(hoverNodeEditor.rect.xMax, hoverSlot.position.center.y);
						targetControlPos = targetPos + EditorGUITools.kBezierTangentOffset;
					}
				}

				_Bezier.startPosition = targetPos;
				_Bezier.startControl = targetControlPos;
				_Bezier.endPosition = pinPos;
				_Bezier.endControl = pinControlPos;
			}

			void UpdateMousePosition(Vector2 graphMousePosition)
			{
				InputSlotElement slotElement = target as InputSlotElement;
				if (slotElement == null)
				{
					return;
				}

				var behaviourEditorGUI = slotElement._BehaviourEditorGUI;
				var nodeEditor = behaviourEditorGUI.nodeEditor;
				var graphEditor = nodeEditor.graphEditor;

				var slotGUI = slotElement.slotGUI;

				DataSlotGUI.UpdateHoverSlot(graphEditor, nodeEditor, slotGUI.targetObject, slotGUI.slot, graphMousePosition);

				UpdateBezier(graphMousePosition);

				graphEditor.DragDataBranchBezier(_Bezier.startPosition, _Bezier.startControl, _Bezier.endPosition, _Bezier.endControl);
			}

			protected override void OnMouseDown(MouseDownEvent e)
			{
				InputSlotElement slotElement = target as InputSlotElement;
				if (slotElement == null)
				{
					return;
				}

				var behaviourEditorGUI = slotElement._BehaviourEditorGUI;
				var nodeEditor = behaviourEditorGUI.nodeEditor;
				var graphEditor = nodeEditor.graphEditor;
				var graphView = graphEditor.graphView;
				var node = nodeEditor.node;

				var slotGUI = slotElement.slotGUI;
				var slot = slotGUI.slot;

				var inputSlot = slot as InputSlotBase;

				var branch = inputSlot.branch;

				DataSlotGUI.BeginDragSlot(node, slot, behaviourEditorGUI.behaviourObj);

				if (branch != null)
				{
					branch.enabled = false;
				}

				graphEditor.BeginDragDataBranch(node.nodeID);

				Vector2 graphMousePosition = graphView.ElementToGraph((e.currentTarget as VisualElement), e.localMousePosition);
				UpdateMousePosition(graphMousePosition);

				e.StopPropagation();
			}

			protected override void OnMouseMove(MouseMoveEvent e)
			{
				InputSlotElement slotElement = target as InputSlotElement;
				if (slotElement == null)
				{
					return;
				}

				DragAndDrop.PrepareStartDrag();

				var behaviourEditorGUI = slotElement._BehaviourEditorGUI;
				var nodeEditor = behaviourEditorGUI.nodeEditor;
				var graphEditor = nodeEditor.graphEditor;
				var graphView = graphEditor.graphView;
				
				Vector2 graphMousePosition = graphView.ElementToGraph((e.currentTarget as VisualElement), e.localMousePosition);
				UpdateMousePosition(graphMousePosition);
			}

			protected override void OnMouseUp(MouseUpEvent e)
			{
				InputSlotElement slotElement = target as InputSlotElement;
				if (slotElement == null)
				{
					return;
				}

				var behaviourEditorGUI = slotElement._BehaviourEditorGUI;
				var nodeEditor = behaviourEditorGUI.nodeEditor;
				var graphEditor = nodeEditor.graphEditor;
				var graphView = graphEditor.graphView;
				
				var slotGUI = slotElement.slotGUI;
				var nodeGraph = graphEditor.nodeGraph;
				var node = nodeEditor.node;
				var targetObject = slotGUI.targetObject;
				var slot = slotGUI.slot;

				var inputSlot = slot as InputSlotBase;

				var branch = inputSlot.branch;

				Vector2 graphMousePosition = graphView.ElementToGraph((e.currentTarget as VisualElement), e.localMousePosition);
				UpdateBezier(graphMousePosition);

				var hoverSlot = DataSlotGUI._HoverSlot;
				if (hoverSlot != null)
				{
					var hoverNodeEditor = DataSlotGUI._HoverNodeEditor;
					var hoverObj = DataSlotGUI._HoverObj;

					if (branch != null)
					{
						graphEditor.DeleteDataBranch(branch);
						branch = null;
					}

					branch = graphEditor.ConnectDataBranch(node.nodeID, targetObject, inputSlot, hoverNodeEditor.nodeID, hoverObj, hoverSlot);
					if (branch != null)
					{
						branch.lineBezier = new Bezier2D(_Bezier);
					}
				}
				else
				{
					GenericMenu menu = new GenericMenu();

					DataBranch currentBranch = branch;
					Bezier2D lineBezier = _Bezier;

					if (inputSlot.connectableType != null || inputSlot.GetConstraint() == null)
					{
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

							DataBranchRerouteNode newRerouteNode = graphEditor.CreateDataBranchRerouteNode(EditorGUITools.SnapToGrid(graphMousePosition), inputSlot.connectableType);

							Undo.RecordObject(nodeGraph, "Reroute");

							RerouteSlot rerouteSlot = newRerouteNode.link;

							currentBranch = graphEditor.ConnectDataBranch(node.nodeID, targetObject, inputSlot, newRerouteNode.nodeID, null, rerouteSlot);
							if (currentBranch != null)
							{
								currentBranch.enabled = true;
								currentBranch.lineBezier = lineBezier;
							}

							Undo.CollapseUndoOperations(undoGroup);

							EditorUtility.SetDirty(nodeGraph);
						});
					}
					else
					{
						menu.AddDisabledItem(EditorContents.reroute);
					}

					if (currentBranch != null)
					{
						menu.AddSeparator("");
						menu.AddItem(EditorContents.disconnect, false, () =>
						{
							graphEditor.DeleteDataBranch(currentBranch);
						});
					}

					menu.ShowAsContext();
				}
			}

			protected override void OnEndDrag()
			{
				base.OnEndDrag();

				InputSlotElement slotElement = target as InputSlotElement;
				if (slotElement == null)
				{
					return;
				}

				DataSlotGUI.EndDragSlot();

				var behaviourEditorGUI = slotElement._BehaviourEditorGUI;
				var nodeEditor = behaviourEditorGUI.nodeEditor;
				var graphEditor = nodeEditor.graphEditor;

				var slot = slotElement.slotGUI.slot;
				var inputSlot = slot as InputSlotBase;

				var branch = inputSlot.branch;

				graphEditor.EndDragDataBranch();

				if (branch != null)
				{
					branch.enabled = true;
				}

				DataSlotGUI.ClearHoverSlot();
			}

			void OnKeyDown(KeyDownEvent e)
			{
				InputSlotElement slotElement = target as InputSlotElement;
				if (slotElement == null)
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