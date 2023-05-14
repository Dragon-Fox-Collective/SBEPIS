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
	using ArborEditor.UnityEditorBridge.UIElements;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class ChangeNodePositionEvent : EventBase<ChangeNodePositionEvent>
	{
		public Vector2 oldPosition
		{
			get;
			private set;
		}

		public Vector2 newPosition
		{
			get;
			private set;
		}

		public static ChangeNodePositionEvent GetPooled(Vector2 oldPosition, Vector2 newPosition)
		{
			var e = GetPooled();
			e.oldPosition = oldPosition;
			e.newPosition = newPosition;
			return e;
		}
	}

	internal sealed class NodeElement : VisualElement
	{
		public readonly NodeEditor nodeEditor;
		
		private VisualElement _ResizableElement;
		private VisualElement _InnerBackground;
		
		public VisualElement headerContainer
		{
			get;
			private set;
		}

		public VisualElement footerContainer
		{
			get;
			private set;
		}

		public VisualElement background
		{
			get;
			private set;
		}

		public VisualElement frame
		{
			get;
			private set;
		}
		
		private VisualElement _ContentContainer;

		public override VisualElement contentContainer
		{
			get
			{
				return _ContentContainer;
			}
		}

		public VisualElement overlayLayer
		{
			get;
			private set;
		}

		private Dictionary<ResizeDirection, VisualElement> _Resizers = new Dictionary<ResizeDirection, VisualElement>();

		private ResizeDirection _ResizeDirection;
		public ResizeDirection resizeDirection
		{
			get
			{
				return _ResizeDirection;
			}
			set
			{
				if (_ResizeDirection != value)
				{
					foreach (ResizeDirection direction in System.Enum.GetValues(typeof(ResizeDirection)))
					{
						if ((_ResizeDirection & direction) == direction)
						{
							if ((value & direction) == 0 && _Resizers.TryGetValue(direction, out var resizer))
							{
								resizer.RemoveFromHierarchy();
							}
						}
						else if ((value & direction) == direction)
						{
							if (!_Resizers.TryGetValue(direction, out var resizer))
							{
								resizer = new VisualElement()
								{
									style =
									{
										position = Position.Absolute,
									}
								};

								var style = resizer.style;

								switch (direction)
								{
									case ResizeDirection.Left:
										style.cursor = CusrorBridge.LoadCursor(MouseCursor.ResizeHorizontal);
										style.top = 0f;
										style.bottom = 0f;
										style.left = -ResizeDefaults.edgeSize;
										style.width = ResizeDefaults.edgeSize;
										break;
									case ResizeDirection.Right:
										style.cursor = CusrorBridge.LoadCursor(MouseCursor.ResizeHorizontal);
										style.top = 0f;
										style.bottom = 0f;
										style.right = -ResizeDefaults.edgeSize;
										style.width = ResizeDefaults.edgeSize;
										break;
									case ResizeDirection.Top:
										style.cursor = CusrorBridge.LoadCursor(MouseCursor.ResizeVertical);
										style.top = -ResizeDefaults.edgeSize;
										style.height = ResizeDefaults.edgeSize;
										style.right = 0;
										style.left = 0;
										break;
									case ResizeDirection.Bottom:
										style.cursor = CusrorBridge.LoadCursor(MouseCursor.ResizeVertical);
										style.bottom = -ResizeDefaults.edgeSize;
										style.height = ResizeDefaults.edgeSize;
										style.right = 0;
										style.left = 0;
										break;
									case ResizeDirection.TopLeft:
										style.cursor = CusrorBridge.LoadCursor(MouseCursor.ResizeUpLeft);
										style.top = -ResizeDefaults.edgeSize;
										style.left = -ResizeDefaults.edgeSize;
										style.width = ResizeDefaults.edgeSize;
										style.height = ResizeDefaults.edgeSize;
										break;
									case ResizeDirection.TopRight:
										style.cursor = CusrorBridge.LoadCursor(MouseCursor.ResizeUpRight);
										style.top = -ResizeDefaults.edgeSize;
										style.right = -ResizeDefaults.edgeSize;
										style.width = ResizeDefaults.edgeSize;
										style.height = ResizeDefaults.edgeSize;
										break;
									case ResizeDirection.BottomLeft:
										style.cursor = CusrorBridge.LoadCursor(MouseCursor.ResizeUpRight);
										style.bottom = -ResizeDefaults.edgeSize;
										style.left = -ResizeDefaults.edgeSize;
										style.width = ResizeDefaults.edgeSize;
										style.height = ResizeDefaults.edgeSize;
										break;
									case ResizeDirection.BottomRight:
										style.cursor = CusrorBridge.LoadCursor(MouseCursor.ResizeUpLeft);
										style.bottom = -ResizeDefaults.edgeSize;
										style.right = -ResizeDefaults.edgeSize;
										style.width = ResizeDefaults.edgeSize;
										style.height = ResizeDefaults.edgeSize;
										break;
								}

								resizer.AddManipulator(new NodeResizer(nodeEditor, direction));

								_Resizers.Add(direction, resizer);
							}

							_ResizableElement.Add(resizer);
						}
					}

					_ResizeDirection = value;
				}
			}
		}

		internal bool isLayouted
		{
			get;
			private set;
		}

		public Rect rectOnGraph
		{
			get
			{
				Rect layout = this.layout;
				layout.position += (Vector2)transform.position;
				return layout;
			}
		}

		public NodeElement(NodeEditor nodeEditor)
		{
			this.nodeEditor = nodeEditor;

			pickingMode = PickingMode.Position;
			usageHints = UsageHints.DynamicTransform;
			style.position = Position.Absolute;

			focusable = true;

			_InnerBackground = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
			};
			_InnerBackground.StretchToParentSize();
			_InnerBackground.AddToClassList("node-window");
			_InnerBackground.style.display = DisplayStyle.None;
			hierarchy.Add(_InnerBackground);

			_ResizableElement = new VisualElement();
			_ResizableElement.StretchToParentSize();
			hierarchy.Add(_ResizableElement);

			background = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
			};
			hierarchy.Add(background);

			headerContainer = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
			};
			hierarchy.Add(headerContainer);

			_ContentContainer = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
				style =
				{
					flexGrow = 1f,
				}
			};
			hierarchy.Add(_ContentContainer);

			footerContainer = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
			};
			hierarchy.Add(footerContainer);

			frame = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
			};
			hierarchy.Add(frame);

			overlayLayer = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
			};
			overlayLayer.StretchToParentSize();
			hierarchy.Add(overlayLayer);

			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			RegisterCallback<MouseEnterEvent>(OnMouseEnter);
			RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

			RegisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);

			this.AddManipulator(new NodeDragger());
			this.AddManipulator(new DragEventPropagation());
		}

		private GraphView _GraphView;

		void RegisterToGraphView()
		{
			_GraphView = GetFirstOfType<GraphView>();
			if (_GraphView != null)
			{
				_GraphView.RegisterNodeElement(this);
			}
		}

		void UnregisterFromGraphView()
		{
			if (_GraphView != null)
			{
				_GraphView.UnregisterNodeElement(this);
				_GraphView = null;
			}
		}

		void OnAttachToPanel(AttachToPanelEvent evt)
		{
			UnregisterFromGraphView();

			RegisterToGraphView();
		}

		void OnDetachFromPanel(DetachFromPanelEvent evt)
		{
			// Fixed an issue where ReleaseMouse was not taken when dropped onto a subgraph
			if (this.HasMouseCapture())
			{
				this.ReleaseMouse();

				GUIUtility.hotControl = 0;
			}

			UnregisterFromGraphView();
		}

		void OnGeometryChanged(GeometryChangedEvent evt)
		{
			isLayouted = true;

			if (resolvedStyle.display == DisplayStyle.Flex)
			{
				Node node = nodeEditor.node;
				if (node.nodeGraph == null)
				{
					return;
				}

				Rect newNodePosition = evt.newRect;
				newNodePosition.position = transform.position;
				if (nodeEditor.rect != newNodePosition)
				{
					nodeEditor.rect = newNodePosition;

					EditorUtility.SetDirty(node.nodeGraph);
				}
			}

			UpdatePosition();
		}

		void OnMouseDown(MouseDownEvent evt)
		{
			BringToFront();
		}

		void OnMouseEnter(MouseEnterEvent evt)
		{
			_InnerBackground.style.display = DisplayStyle.Flex;

			MarkDirtyRepaint();
		}

		void OnMouseLeave(MouseLeaveEvent evt)
		{
			_InnerBackground.style.display = DisplayStyle.None;

			MarkDirtyRepaint();
		}

		public Rect boundingBox
		{
			get;
			private set;
		}

		internal void UpdatePosition()
		{
			Node node = nodeEditor.node;

			Rect nodePosition = node.position;
			Vector2 oldPosition = transform.position;
			Vector2 newPosition = nodePosition.position;
			if (oldPosition != newPosition)
			{
				transform.position = newPosition;

				using (var e = ChangeNodePositionEvent.GetPooled(oldPosition, newPosition))
				{
					e.target = this;
					SendEvent(e);
				}
			}

			IResolvedStyle resolvedStyle = background.resolvedStyle;
			
			var backGroundStyle = _InnerBackground.style;
			backGroundStyle.left = resolvedStyle.left;
			backGroundStyle.top = resolvedStyle.top;
			backGroundStyle.right = resolvedStyle.right;
			backGroundStyle.bottom = resolvedStyle.bottom;

			style.width = nodePosition.width;
			if ((resizeDirection & (ResizeDirection.Top | ResizeDirection.Bottom)) != 0)
			{
				style.height = nodePosition.height;
			}

			if (this.resolvedStyle.display == DisplayStyle.Flex && _GraphView != null)
			{
				boundingBox = _GraphView.ElementToGraph(this, this.GetBoundingBox());
			}
		}
	}
}