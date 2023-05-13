//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

namespace ArborEditor.UIElements
{
	using Arbor;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	public sealed class GraphView : ImmediateModeElement
	{
		private const float k_ShowLogoTime = 3.0f;
		private const float k_FadeLogoTime = 1.0f;
		public const float k_ZoomMin = 10f;
		public const float k_ZoomMax = 100f;

		private static CustomGUI s_UnderlayGUI = new CustomGUI();
		public static event System.Action<NodeGraph, Rect> underlayGUI
		{
			add
			{
				s_UnderlayGUI.onGUI += value;
			}
			remove
			{
				s_UnderlayGUI.onGUI -= value;
			}
		}

		private static CustomGUI s_OverlayGUI = new CustomGUI();
		public static event System.Action<NodeGraph, Rect> overlayGUI
		{
			add
			{
				s_OverlayGUI.onGUI += value;
			}
			remove
			{
				s_OverlayGUI.onGUI -= value;
			}
		}

		private Rect _GraphViewExtents = new Rect(0, 0, 100, 100);
		private Rect _GraphExtents = new Rect(0, 0, 100, 100);

		public event System.Action onChangedGraphExtents;
		public event System.Action onChangedGraphPosition;
		private bool _IsCallingChangedGraphExtents;
		private bool _IsCallingChangedGraphPosition;

		private bool _EnableChangeScroll = true;
		private int _DisableDelayChangeScrollCount = 0;

		private double _FadeLogoBeginTime;

		private List<NodeElement> _NodeElements = new List<NodeElement>();

		public NodeGraphEditor graphEditor
		{
			get;
			internal set;
		}

		public Vector2 scrollOffset
		{
			get
			{
				return new Vector2(_HorizontalScroller.value, _VerticalScroller.value) + graphExtents.min;
			}
		}

		public Rect graphViewRect
		{
			get
			{
				return contentViewport.layout;
			}
		}

		public Rect graphViewport
		{
			get
			{
				return ElementToGraph(contentViewport, graphViewRect);
			}
		}

		public ITransform graphTransform
		{
			get
			{
				return m_ContentContainer.transform;
			}
		}

		public Vector3 graphPosition
		{
			get
			{
				return graphTransform.position;
			}
			set
			{
				var position = value;
				position.x = Mathf.Round(position.x);
				position.y = Mathf.Round(position.y);
				position.z = Mathf.Round(position.z);

				if (graphTransform.position != position)
				{
					graphTransform.position = position;
				}
			}
		}

		public float zoomLevel
		{
			get
			{
				return graphScale.x;
			}
			set
			{
				graphScale = new Vector3(value, value, 1f);
			}
		}

		public Vector3 graphScale
		{
			get
			{
				return graphTransform.scale;
			}
			set
			{
				graphTransform.scale = value;
			}
		}

		public Matrix4x4 graphMatrix
		{
			get
			{
				return graphTransform.matrix;
			}
		}

		public Rect graphExtents
		{
			get
			{
				return _GraphViewExtents;
			}
			internal set
			{
				if (_GraphViewExtents != value)
				{
					isSettedGraphExtents = true;

					using (var e = ChangeGraphExtentsEvent.GetPooled(_GraphViewExtents, value))
					{
						_GraphViewExtents = value;

						if (!_IsCallingChangedGraphExtents)
						{
							_IsCallingChangedGraphExtents = true;
							try
							{
								onChangedGraphExtents?.Invoke();
							}
							finally
							{
								_IsCallingChangedGraphExtents = false;
							}
						}

						if (!_IsCallingChangedGraphExtents)
						{
							_IsCallingChangedGraphExtents = true;
							try
							{
								e.target = this;
								SendEvent(e);
							}
							finally
							{
								_IsCallingChangedGraphExtents = false;
							}
						}
					}
				}
			}
		}

		public Rect graphExtentsRaw
		{
			get
			{
				return _GraphExtents;
			}
		}

		public Vector2 scrollPos
		{
			get
			{
				return CalcTransformToGraph(graphPosition);
			}
			private set
			{
				graphPosition = CalcGraphToTransform(value);
			}
		}

		public bool isLayoutSetup
		{
			get; private set;
		}

		private VisualElement m_ContentContainer;

		// Represents the visible part of contentContainer
		public VisualElement contentViewport
		{
			get; private set;
		}

		public VisualElement contentUnderlay
		{
			get; private set;
		}

		public VisualElement customUnderlayLayer
		{
			get; private set;
		}

		private Label _GraphLabelElement;
		private Label _GraphPlayStateElement;

		private GridBackground _GridBackground;

		private VisualElement _GroupNodeLayer;

		internal VisualElement dataBranchUnderlayLayer
		{
			get; private set;
		}

		internal VisualElement branchUnderlayLayer
		{
			get; private set;
		}

		private VisualElement _NodeLayer;

		private VisualElement _NodeCommentLayerAffectZoom;

		internal VisualElement branchOverlayLayer
		{
			get; private set;
		}

		internal VisualElement dataBranchOverlayLayer
		{
			get; private set;
		}

		public VisualElement contentOverlay
		{
			get; private set;
		}

		internal VisualElement highlightLayer
		{
			get; private set;
		}

		internal VisualElement popupLayer
		{
			get; private set;
		}

		private VisualElement _NodeCommentLayerNotAffectZoom;

		internal VisualElement nodeCommentLayer
		{
			get; private set;
		}

		public VisualElement customOverlayLayer
		{
			get; private set;
		}

		internal Image logoImage
		{
			get; private set;
		}

		private NotEditableElement _NotEditableElement;

		private VisualElement _CustomUnderlayGUI;
		private VisualElement _CustomOverlayGUI;

		private Scroller _HorizontalScroller;
		private Scroller _VerticalScroller;

		public override VisualElement contentContainer // Contains full content, potentially partially visible
		{
			get
			{
				return m_ContentContainer;
			}
		}

		private ZoomManipulator _ZoomManipulator;
		private PanManipulator _PanManipulator;

		private Vector2 _LastScrollerValue = Vector2.zero;

		private Vector2 _LocalMousePosition;

		public Vector2 mousePosition
		{
			get
			{
				return ElementToGraph(contentViewport, _LocalMousePosition);
			}
		}

		private AutoScrollElement _AutoScrollElement;

		private bool _IsAutoScroll = false;
		public bool autoScroll
		{
			get
			{
				return _IsAutoScroll;
			}
			set
			{
				if (_IsAutoScroll != value)
				{
					_IsAutoScroll = value;

					if (_IsAutoScroll)
					{
						if (_AutoScrollElement == null)
						{
							_AutoScrollElement = new AutoScrollElement(contentViewport, OnScroll);
							_AutoScrollElement.StretchToParentSize();
						}

						if (_AutoScrollElement.parent == null)
						{
							contentViewport.Add(_AutoScrollElement);
						}
					}
					else
					{
						if (_AutoScrollElement != null && _AutoScrollElement.parent != null)
						{
							_AutoScrollElement.RemoveFromHierarchy();
						}
					}
				}
			}
		}

		private Vector2 _LastViewportSize = Vector2.zero;
		private bool _IsInitialize = false;

		public bool isSettedGraphExtents
		{
			get;
			private set;
		}

		private FrameSelected _FrameSelected = new FrameSelected();
		private FrameSelected _FrameSelectedZoom = new FrameSelected()
		{
			stoppingDistance = 0.001f,
		};

		private UnityEditorBridge.UIElements.VisualElementCapture _Capture = null;
		
		internal GraphView()
		{
			pickingMode = PickingMode.Ignore;
			style.flexGrow = 1;
			style.overflow = Overflow.Hidden;

			contentViewport = new VisualElement() {
				name = "ContentViewport",
				focusable = true,
				style =
				{
					flexDirection = FlexDirection.Row,
					overflow = Overflow.Hidden,
				}
			};
			contentViewport.AddToClassList("graphview-background");
			contentViewport.StretchToParentSize();
			hierarchy.Add(contentViewport);

			contentUnderlay = new VisualElement()
			{
				name = "ContentUnderlay",
				style =
				{
					overflow = Overflow.Hidden,
				}
			};
			contentUnderlay.StretchToParentSize();
			contentViewport.Add(contentUnderlay);

			customUnderlayLayer = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
				focusable = false,
			};
			customUnderlayLayer.StretchToParentSize();
			contentUnderlay.Add(customUnderlayLayer);

			_GraphLabelElement = new Label();
			_GraphLabelElement.AddToClassList("graph-label");
			UIElementsUtility.SetBoldFont(_GraphLabelElement);

			contentUnderlay.Add(_GraphLabelElement);

			_GraphPlayStateElement = new Label();
			_GraphPlayStateElement.AddToClassList("graph-playstate-label");

			contentUnderlay.Add(_GraphPlayStateElement);

			// Basic content container; its constraints should be defined in the USS file
			m_ContentContainer = new VisualElement() {
				name = "ContentView",
				usageHints = UsageHints.GroupTransform,
				style =
				{
					position = Position.Absolute,
				}
			};

			UIElementsUtility.SetTransformOrigin(m_ContentContainer, 0f, 0f, 0f);

			contentViewport.Add(m_ContentContainer);

			_GroupNodeLayer = new VisualElement()
			{
				name = "GroupNodeLayer",
				style =
				{
					position = Position.Absolute,
				}
			};

			m_ContentContainer.Add(_GroupNodeLayer);

			dataBranchUnderlayLayer = new VisualElement()
			{
				name = "DataBranchUnderlayLayer",
				style =
				{
					position = Position.Absolute,
				}
			};

			m_ContentContainer.Add(dataBranchUnderlayLayer);

			branchUnderlayLayer = new VisualElement()
			{
				name = "BranchUnderlayLayer",
				style =
				{
					position = Position.Absolute,
				}
			};

			m_ContentContainer.Add(branchUnderlayLayer);

			_NodeLayer = new VisualElement()
			{
				name = "NodeLayer",
				style =
				{
					position = Position.Absolute
				}
			};

			m_ContentContainer.Add(_NodeLayer);

			_NodeCommentLayerAffectZoom = new VisualElement();
			m_ContentContainer.Add(_NodeCommentLayerAffectZoom);

			branchOverlayLayer = new VisualElement()
			{
				name = "BranchOverlayLayer",
				style =
				{
					position = Position.Absolute,
				}
			};

			m_ContentContainer.Add(branchOverlayLayer);

			dataBranchOverlayLayer = new VisualElement()
			{
				name = "DataBranchOverlayLayer",
				style =
				{
					position = Position.Absolute,
				}
			};

			m_ContentContainer.Add(dataBranchOverlayLayer);

			contentOverlay = new VisualElement()
			{
				name = "ContentOverlay",
				pickingMode = PickingMode.Ignore,
				style =
				{
					overflow = Overflow.Hidden,
				}
			};
			contentOverlay.StretchToParentSize();
			contentViewport.Add(contentOverlay);

			highlightLayer = new VisualElement()
			{
				name = "HighlightLayer",
			};
			contentOverlay.Add(highlightLayer);

			popupLayer = new VisualElement()
			{
				name = "PopupLayer",
			};
			contentOverlay.Add(popupLayer);

			_NodeCommentLayerNotAffectZoom = new VisualElement();
			contentOverlay.Add(_NodeCommentLayerNotAffectZoom);

			nodeCommentLayer = new VisualElement()
			{
				name = "NodeCommentLayer",
				style =
				{
					position = Position.Absolute,
				}
			};

			UpdateNodeCommentLayer();

			customOverlayLayer = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
				focusable = false,
			};
			customOverlayLayer.StretchToParentSize();
			contentOverlay.Add(customOverlayLayer);

			Texture2D logoTex = Icons.logo;
			float logoWidth = 256f;
			float logoScale = logoWidth / logoTex.width;
			float logoHeight = logoTex.height * logoScale;

			logoImage = new Image()
			{
				pickingMode = PickingMode.Ignore,
				image = logoTex,
				scaleMode = ScaleMode.ScaleToFit,
				style =
				{
					position = Position.Absolute,
					top = 0,
					left = 0,
					width = logoWidth,
					height = logoHeight,
				}
			};

			IMGUIContainer commandContainer = new IMGUIContainer(OnCommandGUI)
			{
				pickingMode = PickingMode.Ignore,
				style =
				{
					width = 0f,
					height = 0f,
				}
			};
			contentViewport.Add(commandContainer);

			_HorizontalScroller = new Scroller(0f, 100f,
				(value) =>
				{
					if (_LastScrollerValue.x != value)
					{
						OnChangedScrollValue();
					}
					_LastScrollerValue.x = value;
				},
				SliderDirection.Horizontal
				)
			{
				name = "HorizontalScroller",
				viewDataKey = "HorizontalScroller",
				style =
				{
					position = Position.Absolute,
					left = 0f,
					bottom = 0f,
					right = 17f,
				}
			};
			_HorizontalScroller.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedHorizontalScroller);
			hierarchy.Add(_HorizontalScroller);

			_VerticalScroller = new Scroller(0f, 100f,
					(value) =>
					{
						if (_LastScrollerValue.y != value)
						{
							OnChangedScrollValue();
						}
						_LastScrollerValue.y = value;
					},
					SliderDirection.Vertical
					)
			{
				name = "VerticalScroller",
				viewDataKey = "VerticalScroller",
				style =
				{
					position = Position.Absolute,
					top = 0f,
					bottom = 17f,
					right = 0f,
				}
			};
			_VerticalScroller.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedVerticalScroller);
			hierarchy.Add(_VerticalScroller);

			_ZoomManipulator = new ZoomManipulator(m_ContentContainer, OnZoom);
			contentViewport.AddManipulator(_ZoomManipulator);

			_PanManipulator = new PanManipulator(m_ContentContainer, OnScroll);
			contentViewport.AddManipulator(_PanManipulator);

			RectangleSelector rectangleSelector = new RectangleSelector(this);
			contentViewport.AddManipulator(rectangleSelector);

			ContextClickManipulator contextClick = new ContextClickManipulator(OnContextClick);
			contentViewport.AddManipulator(contextClick);

			contentViewport.RegisterCallback<MouseCaptureEvent>(OnMouseCapture, TrickleDown.TrickleDown);

			contentViewport.RegisterCallback<MouseMoveEvent>(OnMouseMove, TrickleDown.TrickleDown);
			contentViewport.RegisterCallback<DragUpdatedEvent>(OnMouseMove, TrickleDown.TrickleDown);

			contentViewport.RegisterCallback<DragEnterEvent>(OnDragEnter);
			contentViewport.RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
			contentViewport.RegisterCallback<DragPerformEvent>(OnDragPerform);
			contentViewport.RegisterCallback<DragLeaveEvent>(OnDragLeave);
			contentViewport.RegisterCallback<DragExitedEvent>(OnDragExited, TrickleDown.TrickleDown);

			contentViewport.RegisterCallback<ValidateCommandEvent>(OnValidateCommand);
			contentViewport.RegisterCallback<ExecuteCommandEvent>(OnExecuteCommand);

			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
			contentViewport.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

			EnableCustomUnderlay(s_UnderlayGUI.hasGUI);
			EnableCustomOverlay(s_OverlayGUI.hasGUI);
		}

		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			s_UnderlayGUI.onChanged += EnableCustomUnderlay;
			s_OverlayGUI.onChanged += EnableCustomOverlay;
		}

		private void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			s_UnderlayGUI.onChanged -= EnableCustomUnderlay;
			s_OverlayGUI.onChanged -= EnableCustomOverlay;
		}

		private void OnGeometryChanged(GeometryChangedEvent e)
		{
			if (layout.width == 0 || layout.height == 0)
			{
				// Do not scroll and update until the size is confirmed.
				return;
			}

			if (!_IsLayoutedVerticalScroller || !_IsLayoutedHorizontalScroller)
			{
				return;
			}

			UpdateView();
		}

		void OnMouseCapture(MouseCaptureEvent e)
		{
			var evtTarget = e.target as VisualElement;
			evtTarget?.RegisterCallback<MouseMoveEvent>(OnMouseMove);
			evtTarget?.RegisterCallback<MouseCaptureOutEvent>(OnMouseCaptureOut);
		}

		void OnMouseCaptureOut(MouseCaptureOutEvent e)
		{
			var evtTarget = e.target as VisualElement;
			evtTarget?.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
		}

		public Vector2 GetMousePosition(VisualElement element)
		{
			return contentViewport.ChangeCoordinatesTo(element, _LocalMousePosition);
		}

		internal void OnMouseMove(IMouseEvent e)
		{
			EventBase eventBase = e as EventBase;
			VisualElement target = eventBase.currentTarget as VisualElement;
			_LocalMousePosition = target.ChangeCoordinatesTo(contentViewport, e.localMousePosition);
		}

		void OnDragEnter(DragEnterEvent e)
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			if (graphEditor != null)
			{
				graphEditor.OnDragEnter();
			}
		}

		void OnDragUpdated(DragUpdatedEvent e)
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			if (graphEditor != null)
			{
				graphEditor.OnDragUpdated();
			}
		}

		void OnDragPerform(DragPerformEvent e)
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			if (graphEditor != null)
			{
				graphEditor.OnDragPerform(ElementToGraph(e.currentTarget as VisualElement, e.localMousePosition));
			}
		}

		void OnDragLeave(DragLeaveEvent e)
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			if (graphEditor != null)
			{
				graphEditor.OnDragLeave();
			}
		}

		void OnDragExited(DragExitedEvent e)
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			if (graphEditor != null)
			{
				graphEditor.OnDragExited();
			}
		}

		void OnContextClick(ContextClickEvent e)
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			if (graphEditor != null)
			{
				VisualElement currentTarget = e.currentTarget as VisualElement;
				Vector2 graphPosition = ElementToGraph(currentTarget, e.localMousePosition);
				Vector2 screenPosition = currentTarget.LocalToScreen(e.localMousePosition);
				if (graphEditor.OnContextMenu(new MousePosition(graphPosition, screenPosition)))
				{
					e.StopPropagation();
				}
			}
		}

		void OnCommandGUI()
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			if (graphEditor == null)
			{
				return;
			}

			Event current = Event.current;
			if (current.type == EventType.Repaint)
			{
				graphEditor.OnRepainted();
			}

			switch (current.type)
			{
				case EventType.ValidateCommand:
					{
						if (graphEditor.OnValidateCommand(current.commandName))
						{
							current.Use();
						}
					}
					break;
				case EventType.ExecuteCommand:
					{
						if (graphEditor.OnExecuteCommand(current.commandName, mousePosition))
						{
							current.Use();
						}
					}
					break;
			}
		}

		void OnValidateCommand(ValidateCommandEvent e)
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			if (graphEditor != null)
			{
				if (graphEditor.OnValidateCommand(e.commandName))
				{
					e.StopPropagation();
					e.imguiEvent?.Use();
				}
			}
		}

		void OnExecuteCommand(ExecuteCommandEvent e)
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			if (graphEditor != null)
			{
				if (graphEditor.OnExecuteCommand(e.commandName, mousePosition))
				{
					e.StopPropagation();
					e.imguiEvent?.Use();
				}
			}
		}

		private bool _IsLayoutedVerticalScroller = false;
		private bool _IsLayoutedHorizontalScroller = false;

		private void OnGeometryChangedVerticalScroller(GeometryChangedEvent e)
		{
			contentViewport.style.right = e.newRect.width;
			_HorizontalScroller.style.right = e.newRect.width;

			_IsLayoutedVerticalScroller = true;
		}

		private void OnGeometryChangedHorizontalScroller(GeometryChangedEvent e)
		{
			contentViewport.style.bottom = e.newRect.height;
			_VerticalScroller.style.bottom = e.newRect.height;

			_IsLayoutedHorizontalScroller = true;
		}

		internal void UpdateNodeCommentLayer()
		{
			if (ArborSettings.nodeCommentAffectsZoom)
			{
				_NodeCommentLayerAffectZoom.Add(nodeCommentLayer);
			}
			else
			{
				_NodeCommentLayerNotAffectZoom.Add(nodeCommentLayer);
			}
		}

		void SetScrollOffset(Vector2 value, bool updateTransform, bool endFrameSelected)
		{
			Vector2 tmpValue = value;

			bool changed = false;

			value -= graphExtents.min;
			value.x = Mathf.Clamp(value.x, _HorizontalScroller.lowValue, _HorizontalScroller.highValue);
			value.y = Mathf.Clamp(value.y, _VerticalScroller.lowValue, _VerticalScroller.highValue);

			bool EnableChangeScroll = _EnableChangeScroll;
			_EnableChangeScroll = false;

			if (_HorizontalScroller.value != value.x)
			{
				Slider slider = _HorizontalScroller.slider;
				var newValue = Mathf.Clamp(value.x, slider.lowValue, slider.highValue);

				if (slider.value != newValue)
				{
					_DisableDelayChangeScrollCount++;

					_HorizontalScroller.value = value.x;
					changed = true;
				}
			}

			if (_VerticalScroller.value != value.y)
			{
				Slider slider = _VerticalScroller.slider;
				var newValue = Mathf.Clamp(value.y, slider.lowValue, slider.highValue);

				if (slider.value != newValue)
				{
					_DisableDelayChangeScrollCount++;

					_VerticalScroller.value = value.y;
					changed = true;
				}
			}

			if (tmpValue != scrollOffset)
			{
				changed = true;
			}

			_EnableChangeScroll = EnableChangeScroll;

			if (changed && updateTransform)
			{
				UpdateContentViewTransform(endFrameSelected);
			}
		}

		void UpdateContentViewTransform(Vector2 scrollOffset, bool endFrameSelected)
		{
			// Adjust contentContainer's position
			SetScroll(scrollOffset, false, endFrameSelected);

			MarkDirtyRepaint();
		}

		void UpdateContentViewTransform(bool endFrameSelected)
		{
			UpdateContentViewTransform(scrollOffset, endFrameSelected);
		}

		void OnChangedScrollValue()
		{
			if (_DisableDelayChangeScrollCount == 0)
			{
				if (_EnableChangeScroll)
				{

					UpdateContentViewTransform(true);
				}
			}
			else
			{
				_DisableDelayChangeScrollCount--;
			}
		}

		internal VisualElement GetLayer(bool isWindow)
		{
			return isWindow ? _NodeLayer : _GroupNodeLayer;
		}

		private HashSet<NodeElement> _WaitingLayoutNodeElements = new HashSet<NodeElement>();

		internal void RegisterNodeElement(NodeElement nodeElement)
		{
			_WaitingLayoutNodeElements.Add(nodeElement);
			_NodeElements.Add(nodeElement);

			nodeElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedNodeElement);
		}

		internal void UnregisterNodeElement(NodeElement nodeElement)
		{
			nodeElement.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedNodeElement);

			_WaitingLayoutNodeElements.Remove(nodeElement);
			_NodeElements.Remove(nodeElement);
		}

		private void OnGeometryChangedNodeElement(GeometryChangedEvent e)
		{
			var nodeElement = e.target as NodeElement;
			if (nodeElement == null)
				return;

			_WaitingLayoutNodeElements.Remove(nodeElement);
			if (_WaitingLayoutNodeElements.Count != 0)
			{
				return;
			}

			graphEditor.UpdateLayout();
			graphEditor.Repaint();
		}

		public Vector2 GraphToElement(VisualElement dest,Vector2 point)
		{
			if (m_ContentContainer != dest)
			{
				point = m_ContentContainer.ChangeCoordinatesTo(dest, point);
			}
			return point;
		}

		public Rect GraphToElement(VisualElement dest, Rect rect)
		{
			if (m_ContentContainer != dest)
			{
				rect = m_ContentContainer.ChangeCoordinatesTo(dest, rect);
			}
			return rect;
		}

		public Vector2 ElementToGraph(VisualElement dest, Vector2 point)
		{
			if (m_ContentContainer != dest)
			{
				point = dest.ChangeCoordinatesTo(m_ContentContainer, point);
			}
			return point;
		}

		public Rect ElementToGraph(VisualElement dest, Rect rect)
		{
			if (m_ContentContainer != dest)
			{
				rect = dest.ChangeCoordinatesTo(m_ContentContainer, rect);
			}
			return rect;
		}

		public Vector2 GUIToGraph(Vector2 position)
		{
			return m_ContentContainer.GUIToLocal(position);
		}

		public Rect GUIToGraph(Rect rect)
		{
			return m_ContentContainer.GUIToLocal(rect);
		}

		public Vector2 GraphToGUI(Vector2 position)
		{
			return m_ContentContainer.LocalToGUI(position);
		}

		public Rect GraphToGUI(Rect rect)
		{
			return m_ContentContainer.LocalToGUI(rect);
		}

		public Vector2 ScreenToGraph(Vector2 position)
		{
			return m_ContentContainer.ScreenToLocal(position);
		}

		public Rect ScreenToGraph(Rect rect)
		{
			return m_ContentContainer.ScreenToLocal(rect);
		}

		public Vector2 GraphToScreen(Vector2 position)
		{
			return m_ContentContainer.LocalToScreen(position);
		}

		public Rect GraphToScreen(Rect rect)
		{
			return m_ContentContainer.LocalToScreen(rect);
		}

		Vector2 CalcTransformToGraph(Vector2 value)
		{
			return -value / zoomLevel;
		}

		Vector2 CalcGraphToTransform(Vector2 value)
		{
			return -value * zoomLevel;
		}

		internal void OnZoom(Vector2 zoomCenter, float zoomDelta)
		{
			SetZoom(zoomCenter, zoomLevel * (1f + zoomDelta), true);
		}

		internal void OnScroll(Vector2 delta)
		{
			SetScroll(scrollPos + delta, true, true);
		}

		protected override void ImmediateRepaint()
		{
			if (_PanManipulator.isActive)
			{
				EditorGUIUtility.AddCursorRect(contentViewport.layout, MouseCursor.Pan);
			}
			else if (_ZoomManipulator.isActive)
			{
				EditorGUIUtility.AddCursorRect(contentViewport.layout, MouseCursor.Zoom);
			}
		}

		private Rect CalculateGraphExtents()
		{
			if (_NodeElements.Count > 0)
			{
				Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
				Vector2 max = new Vector2(float.MinValue, float.MinValue);

				for (int i = 0, count = _NodeElements.Count; i < count; i++)
				{
					var element = _NodeElements[i] as NodeElement;
					if (element == null)
						continue;

					Rect nodePosition = element.rectOnGraph;

					min = Vector2.Min(min, nodePosition.min);
					max = Vector2.Max(max, nodePosition.max);
				}

				return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
			}
			else
			{
				return new Rect(0, 0, 1, 1);
			}
		}

		private void UpdateGraphExtents()
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			if (graphEditor == null)
			{
				return;
			}

			Rect extents = CalculateGraphExtents();
			_GraphExtents = extents;

			Rect graphPosition = graphViewport;

			extents.xMin -= graphPosition.width * 0.6f;
			extents.xMax += graphPosition.width * 0.6f;
			extents.yMin -= graphPosition.height * 0.6f;
			extents.yMax += graphPosition.height * 0.6f;

			extents.xMin = (int)extents.xMin;
			extents.xMax = (int)extents.xMax;
			extents.yMin = (int)extents.yMin;
			extents.yMax = (int)extents.yMax;

			if (graphEditor.isDragNodes)
			{
				if (graphPosition.xMin < extents.xMin)
				{
					extents.xMin = graphPosition.xMin;
				}
				if (extents.xMax < graphPosition.xMax)
				{
					extents.xMax = graphPosition.xMax;
				}

				if (graphPosition.yMin < extents.yMin)
				{
					extents.yMin = graphPosition.yMin;
				}
				if (extents.yMax < graphPosition.yMax)
				{
					extents.yMax = graphPosition.yMax;
				}
			}

			if (graphExtents != extents)
			{
				this.graphExtents = extents;
			}
		}

		public event System.Action onPostLayout;

		void UpdateView(bool updateScroll = true)
		{
			bool enableChangeScroll = _EnableChangeScroll;
			_EnableChangeScroll = false;

			Vector2 oldScrollOffset = scrollOffset;			
			Rect lastGraphExtents = this.graphExtents;

			UpdateGraphExtents();

			Rect graphExtents = this.graphExtents;
			Rect viewportLayout = graphViewport;

			if (graphExtents.width > Mathf.Epsilon)
			{
				_HorizontalScroller.Adjust(viewportLayout.width / graphExtents.width);
			}
			if (graphExtents.height > Mathf.Epsilon)
			{
				_VerticalScroller.Adjust(viewportLayout.height / graphExtents.height);
			}

			_HorizontalScroller.lowValue = 0;
			_HorizontalScroller.highValue = Mathf.Max(graphExtents.width - viewportLayout.width, 1);
			
			_VerticalScroller.lowValue = 0;
			_VerticalScroller.highValue = Mathf.Max(graphExtents.height - viewportLayout.height, 1);

			_EnableChangeScroll = enableChangeScroll;

			bool changeExtents = graphExtents != lastGraphExtents;
			bool changeViewport = _LastViewportSize != viewportLayout.size;
			if (updateScroll && (!_IsInitialize || changeExtents || changeViewport))
			{
				SetScrollOffset(oldScrollOffset, true, false);
			}

			_IsInitialize = true;
			_LastViewportSize = viewportLayout.size;

			if (!isLayoutSetup)
			{
				isLayoutSetup = true;
				onPostLayout?.Invoke();
			}
		}

		internal bool DirtyGraphExtents()
		{
			if (_WaitingLayoutNodeElements.Count > 0)
			{
				return false;
			}

			isLayoutSetup = false;
			UpdateLayout();

			return true;
		}

		void UpdateLayout(bool updateScroll = true)
		{
			if (!_IsInitialize)
			{
				return;
			}

			UpdateView(updateScroll);
		}

		public event System.Action onChangedScroll;

		internal void SetScroll(Vector2 position, bool updateView, bool endFrameSelected)
		{
			if (!isLayoutSetup)
			{
				return;
			}

			if (endFrameSelected)
			{
				EndFrameSelected();
			}

			Rect extents = graphExtents;
			Rect viewport = graphViewport;
			position.x = Mathf.Clamp(position.x, extents.xMin, extents.xMax - viewport.width);
			position.y = Mathf.Clamp(position.y, extents.yMin, extents.yMax - viewport.height);

			Vector2 oldGraphPosaition = scrollPos;

			scrollPos = position;

			if (updateView)
			{
				SetScrollOffset(position, false, false);
			}

			onChangedScroll?.Invoke();

			Vector2 newGraphPosition = position;
			if (oldGraphPosaition != newGraphPosition)
			{
				using (var e = ChangeGraphScrollEvent.GetPooled(oldGraphPosaition, newGraphPosition))
				{
					if (!_IsCallingChangedGraphPosition)
					{
						_IsCallingChangedGraphPosition = true;
						try
						{
							onChangedGraphPosition?.Invoke();
						}
						finally
						{
							_IsCallingChangedGraphPosition = false;
						}
					}

					if (!_IsCallingChangedGraphPosition)
					{
						_IsCallingChangedGraphPosition = true;
						try
						{
							e.target = this;
							SendEvent(e);
						}
						finally
						{
							_IsCallingChangedGraphPosition = false;
						}
					}
				}
			}
		}

		internal void SetZoom(Vector2 zoomCenter, float zoomLevel, bool endFrameSelected, bool updateScroll = true)
		{
			Vector3 oldlPosition = graphPosition;
			float oldZoomLevel = this.zoomLevel;

			zoomCenter = GraphToElement(m_ContentContainer, zoomCenter);

			float zoomMin = k_ZoomMin / 100f;
			float zoomMax = k_ZoomMax / 100f;

			float newZoomLevel = Mathf.Clamp(zoomLevel, zoomMin, zoomMax);

			Vector3 newPosition = oldlPosition + (Vector3)(zoomCenter * oldZoomLevel) - (Vector3)(zoomCenter * newZoomLevel);

			bool changed = false;

			if (oldlPosition != newPosition)
			{
				graphPosition = newPosition;
				changed = true;
			}
			if (oldZoomLevel != newZoomLevel)
			{
				this.zoomLevel = newZoomLevel;
				changed = true;
			}

			if (changed)
			{
				Vector2 scrollPos = this.scrollPos;

				UpdateLayout(updateScroll);

				if (updateScroll)
				{
					SetScroll(scrollPos, true, endFrameSelected);
				}
			}
		}

		internal void SetShowGridBackground(bool showGrid)
		{
			if (showGrid)
			{
				if (_GridBackground == null)
				{
					_GridBackground = new GridBackground(contentViewport)
					{
						name = "GridBackground"
					};
				}

				if (_GridBackground.parent == null)
				{
					m_ContentContainer.Insert(0, _GridBackground);
				}
			}
			else
			{
				_GridBackground?.RemoveFromHierarchy();
			}
		}

		internal void FrameSelected(Vector2 frameSelectTarget)
		{
			_FrameSelected.Begin(frameSelectTarget);
			_FrameSelectedZoom.Begin(Vector2.one);
		}

		void EndFrameSelected()
		{
			_FrameSelected.End();
			_FrameSelectedZoom.End();
		}

		internal bool UpdateFrameSelected()
		{
			bool repaint = false;

			if (_FrameSelectedZoom.isPlaying)
			{
				float zoomScale = _FrameSelectedZoom.Update(graphScale, Vector2.zero).x;

				SetZoom(graphViewport.center, zoomScale, false, !_FrameSelected.isPlaying);

				repaint = true;
			}

			if (_FrameSelected.isPlaying)
			{
				Vector2 scrollPos = _FrameSelected.Update(this.scrollPos, -graphViewport.size * 0.5f);

				SetScroll(scrollPos, true, false);

				repaint = true;
			}

			return repaint;
		}

		internal void Update()
		{
			var graphEditor = this.graphEditor;
			if (graphEditor == null || graphEditor.nodeGraph == null)
			{
				return;
			}

			UpdateNotEditable();
			UpdateGraphLabel();
			UpdatePlayState();
			UpdateLogo();
		}

		void UpdateNotEditable()
		{
			if (graphEditor.editable)
			{
				if (_NotEditableElement != null && _NotEditableElement.parent != null)
				{
					_NotEditableElement.RemoveFromHierarchy();
				}
			}
			else
			{
				if (_NotEditableElement == null)
				{
					_NotEditableElement = new NotEditableElement();
					_NotEditableElement.StretchToParentSize();
				}
				if (_NotEditableElement.parent == null)
				{
					contentOverlay.Add(_NotEditableElement);
				}
			}
		}

		void UpdateGraphLabel()
		{
			_GraphLabelElement.text = graphEditor.GetGraphLabel().text;
		}

		void UpdatePlayState()
		{
			string playStateLabel = null;

			if (Application.isPlaying && graphEditor.HasPlayState())
			{
				PlayState playState = graphEditor.GetPlayState();

				switch (playState)
				{
					case PlayState.Stopping:
						playStateLabel = Localization.GetTextContent("PlayState.Stopping").text;
						break;
					case PlayState.Playing:
						playStateLabel = Localization.GetTextContent("PlayState.Playing").text;
						break;
					case PlayState.Pausing:
						playStateLabel = Localization.GetTextContent("PlayState.Pausing").text;
						break;
					case PlayState.InactivePausing:
						playStateLabel = Localization.GetTextContent("PlayState.InactivePausing").text;
						break;
				}
			}

			_GraphPlayStateElement.text = playStateLabel;
		}

		internal void OnChangedShowLogo(bool forceFade = false)
		{
			switch (ArborSettings.showLogo)
			{
				case LogoShowMode.Hidden:
					logoImage.RemoveFromHierarchy();
					break;
				case LogoShowMode.FadeOut:
					if (forceFade)
					{
						logoImage.tintColor = new Color(1f, 1f, 1f, 0.5f);
						if (!contentOverlay.Contains(logoImage))
						{
							contentOverlay.Add(logoImage);
						}
						_FadeLogoBeginTime = EditorApplication.timeSinceStartup;
					}
					break;
				case LogoShowMode.AlwaysShow:
					logoImage.tintColor = new Color(1f, 1f, 1f, 0.5f);
					if (!contentOverlay.Contains(logoImage))
					{
						contentOverlay.Add(logoImage);
					}
					break;
			}
		}

		private void UpdateLogo()
		{
			if (ArborSettings.showLogo != LogoShowMode.FadeOut || logoImage.parent == null)
			{
				return;
			}

			float t = (float)(EditorApplication.timeSinceStartup - (_FadeLogoBeginTime + k_ShowLogoTime)) / k_FadeLogoTime;
			if (t >= 1.0f)
			{
				logoImage.RemoveFromHierarchy();
			}
			else
			{
				float alpha = Mathf.Lerp(0.5f, 0f, Mathf.Clamp01(t));

				logoImage.tintColor = new Color(1f, 1f, 1f, alpha);
			}
		}

		void EnableCustomUnderlay(bool enable)
		{
			if (enable)
			{
				if (_CustomUnderlayGUI == null)
				{
					System.Action onGUIHandler = () =>
					{
						Rect rect = _CustomUnderlayGUI.contentRect;
						if (RectUtility.IsNaN(rect))
						{
							return;
						}

						if (graphEditor != null)
						{
							s_UnderlayGUI.OnGUI(graphEditor.nodeGraph, rect);
						}
					};
					var imguiContainer = new IMGUIContainer(onGUIHandler)
					{
						name = "CustomUnderlayGUI",
						pickingMode = PickingMode.Ignore,
						focusable = false,
					};
					_CustomUnderlayGUI = imguiContainer;
				}
				_CustomUnderlayGUI.StretchToParentSize();
				if (_CustomUnderlayGUI.parent == null)
				{
					customUnderlayLayer.Insert(0, _CustomUnderlayGUI);
				}
			}
			else
			{
				if (_CustomUnderlayGUI != null && _CustomUnderlayGUI.parent != null)
				{
					_CustomUnderlayGUI.RemoveFromHierarchy();
				}
			}
		}

		void EnableCustomOverlay(bool enable)
		{
			if (enable)
			{
				if (_CustomOverlayGUI == null)
				{
					System.Action onGUIHandler = () =>
					{
						Rect rect = _CustomOverlayGUI.contentRect;
						if (RectUtility.IsNaN(rect))
						{
							return;
						}

						if (graphEditor != null)
						{
							s_OverlayGUI.OnGUI(graphEditor.nodeGraph, rect);
						}
					};
					var imguiContainer = new IMGUIContainer(onGUIHandler)
					{
						name = "CustomOverlayGUI",
						pickingMode = PickingMode.Ignore,
						focusable = false,
					};
					imguiContainer.StretchToParentSize();
					_CustomOverlayGUI = imguiContainer;
				}
				if (_CustomOverlayGUI.parent == null)
				{
					customOverlayLayer.Insert(0, _CustomOverlayGUI);
				}
			}
			else
			{
				if (_CustomOverlayGUI != null && _CustomOverlayGUI.parent != null)
				{
					_CustomOverlayGUI.RemoveFromHierarchy();
				}
			}
		}

		internal void OnDestroy()
		{
			if (_Capture != null)
			{
				Object.DestroyImmediate(_Capture);
				_Capture = null;
			}
		}

		internal void Capture()
		{
			var graphEditor = this.graphEditor;

			graphEditor.ClearInvsibleNodes();

			Rect graphCaptureExtents = new RectOffset(100, 100, 100, 100).Add(graphExtentsRaw);

			if (graphCaptureExtents.width < 500)
			{
				float center = graphCaptureExtents.center.x;
				graphCaptureExtents.xMin = center - 250;
				graphCaptureExtents.xMax = center + 250;
			}
			if (graphCaptureExtents.height < 500)
			{
				float center = graphCaptureExtents.center.y;
				graphCaptureExtents.yMin = center - 250;
				graphCaptureExtents.yMax = center + 250;
			}

			graphCaptureExtents.x = Mathf.Floor(graphCaptureExtents.x);
			graphCaptureExtents.width = Mathf.Floor(graphCaptureExtents.width);
			graphCaptureExtents.y = Mathf.Floor(graphCaptureExtents.y);
			graphCaptureExtents.height = Mathf.Floor(graphCaptureExtents.height);

			int maxTextureSize = SystemInfo.maxTextureSize;
			if (graphCaptureExtents.width <= maxTextureSize && graphCaptureExtents.height <= maxTextureSize)
			{
				VisualElement target = m_ContentContainer;

				ITransform transform = target.transform;

				Vector3 oldPosition = transform.position;
				Quaternion oldRotation = transform.rotation;
				Vector3 oldScale = transform.scale;

				transform.rotation = Quaternion.identity;
				transform.scale = Vector3.one;
				transform.position = -graphCaptureExtents.position;

				Rect oldGraphExtents = this.graphExtents;
				this.graphExtents = graphCaptureExtents;

				graphCaptureExtents.position = Vector2.zero;

				var logoImage = this.logoImage;

				Color oldColor = logoImage.tintColor;
				VisualElement oldLogoParent = logoImage.parent;

				logoImage.tintColor = new Color(1f, 1f, 1f, 0.5f);
				if (oldLogoParent == null)
				{
					contentOverlay.Add(logoImage);
				}

				if (_Capture == null)
				{
					_Capture = ScriptableObject.CreateInstance<UnityEditorBridge.UIElements.VisualElementCapture>();
				}

				if (_Capture.Capture(contentViewport, graphCaptureExtents))
				{
					string path = EditorUtility.SaveFilePanel("Save", ArborEditorCache.captureDirectory, graphEditor.nodeGraph.graphName, "png");
					if (!string.IsNullOrEmpty(path))
					{
						ArborEditorCache.captureDirectory = System.IO.Path.GetDirectoryName(path);
					}
					_Capture.SaveImage(path, true);
					_Capture.DestroyImage();
				}

				logoImage.tintColor = oldColor;
				if (oldLogoParent == null)
				{
					logoImage.RemoveFromHierarchy();
				}

				transform.position = oldPosition;
				transform.rotation = oldRotation;
				transform.scale = oldScale;

				this.graphExtents = oldGraphExtents;
			}
			else
			{
				Debug.LogError("Screenshot failed : Graph size is too large.");
			}
		}

		internal void BeginSelection(bool clear)
		{
			graphEditor.BeginSelection(clear);
		}

		internal void SelectNodesInRect(Rect rect, bool actionKey, bool shiftKey)
		{
			graphEditor.SelectNodesInRect(rect, actionKey, shiftKey);
		}

		internal void EndSelection()
		{
			graphEditor.EndSelection();
		}

		internal void CancelSelection()
		{
			graphEditor.CancelSelection();
		}
	}
}