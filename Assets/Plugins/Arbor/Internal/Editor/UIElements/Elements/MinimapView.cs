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

	internal sealed class MinimapTransformEvent : EventBase<MinimapTransformEvent>
	{
	}

	public enum MinimapLayer
	{
		None,
		Underlay,
		Middle,
		Overlay,
	}

	public sealed class MinimapView : VisualElement
	{
		static readonly float s_MinimapMinGraphExtents = 1000f;

		private ArborEditorWindow _Window;

		private bool _IsLayouted;

		public bool isSettedTransform
		{
			get;
			private set;
		}

		private Matrix4x4 _GraphToMinimapTransform = Matrix4x4.identity;
		private Matrix4x4 _MinimapToGraphTransform = Matrix4x4.identity;

		private VisualElement _ContentContainer;

		private VisualElement _ViewportElement;
		private VisualElement _ViewportCross;

		public override VisualElement contentContainer
		{
			get
			{
				return _ContentContainer;
			}
		}

		internal VisualElement nodeUnderlayLayer
		{
			get;
			private set;
		}

		internal VisualElement dataBranchLayer
		{
			get;
			private set;
		}

		internal VisualElement branchLayer
		{
			get;
			private set;
		}

		internal VisualElement nodeMiddleLayer
		{
			get;
			private set;
		}

		internal VisualElement nodeOverlayLayer
		{
			get;
			private set;
		}

		public MinimapView(ArborEditorWindow window)
		{
			AddToClassList("minimap");

			_Window = window;

			style.overflow = Overflow.Hidden;

			_ContentContainer = new VisualElement();
			_ContentContainer.StretchToParentSize();
			hierarchy.Add(_ContentContainer);

			nodeUnderlayLayer = new VisualElement()
			{
				style =
				{
					position = Position.Absolute
				}
			};
			_ContentContainer.Add(nodeUnderlayLayer);

			dataBranchLayer = new VisualElement()
			{
				style =
				{
					position = Position.Absolute,
				}
			};
			_ContentContainer.Add(dataBranchLayer);

			branchLayer = new VisualElement()
			{
				style =
				{
					position = Position.Absolute,
				}
			};
			_ContentContainer.Add(branchLayer);

			nodeMiddleLayer = new VisualElement()
			{
				style =
				{
					position = Position.Absolute
				}
			};
			_ContentContainer.Add(nodeMiddleLayer);

			nodeOverlayLayer = new VisualElement()
			{
				style =
				{
					position = Position.Absolute
				}
			};
			_ContentContainer.Add(nodeOverlayLayer);

			_ViewportElement = new VisualElement();
			_ViewportElement.AddToClassList("viewport");
			hierarchy.Add(_ViewportElement);

			_ViewportCross = new VisualElement();
			_ViewportCross.AddToClassList("cross");
			_ViewportElement.Add(_ViewportCross);

			_ViewportElement.AddManipulator(new ViewportManipulator(this));

			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
		}

		public VisualElement GetNodeLayer(MinimapLayer minimapLayer)
		{
			switch (minimapLayer)
			{
				case MinimapLayer.None:
					return null;
				case MinimapLayer.Underlay:
					return nodeUnderlayLayer;
				case MinimapLayer.Middle:
					return nodeMiddleLayer;
				case MinimapLayer.Overlay:
					return nodeOverlayLayer;
			}

			return null;
		}

		void OnGeometryChanged(GeometryChangedEvent e)
		{
			_IsLayouted = true;

			UpdateMinimapTransfrom();
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			var graphView = _Window.graphView;
			graphView.RegisterCallback<ChangeGraphExtentsEvent>(OnChangeGraphExtents);
			graphView.RegisterCallback<ChangeGraphScrollEvent>(OnChangeGraphScroll);
			graphView.contentViewport.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedGraphViewport);
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			var graphView = _Window.graphView;
			graphView.UnregisterCallback<ChangeGraphExtentsEvent>(OnChangeGraphExtents);
			graphView.UnregisterCallback<ChangeGraphScrollEvent>(OnChangeGraphScroll);
			graphView.contentViewport.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedGraphViewport);
		}

		void OnChangeGraphExtents(ChangeGraphExtentsEvent e)
		{
			UpdateMinimapTransfrom();
		}

		void OnChangeGraphScroll(ChangeGraphScrollEvent e)
		{
			UpdateViewportElement();
		}

		void OnGeometryChangedGraphViewport(GeometryChangedEvent e)
		{
			UpdateViewportElement();
		}

		internal void UpdateMinimapTransfrom()
		{
			if (!_IsLayouted)
			{
				return;
			}

			NodeGraphEditor graphEditor = _Window.graphEditor;
			if (graphEditor == null || graphEditor.nodeGraph == null)
			{
				return;
			}

			GraphView graphView = graphEditor.graphView;
			if (!graphView.isSettedGraphExtents)
			{
				return;
			}
			
			Rect minimapRect = contentRect;

			Rect extents = new RectOffset(100, 100, 100, 100).Add(graphView.graphExtentsRaw);

			Vector2 center = extents.center;
			if (extents.width < s_MinimapMinGraphExtents)
			{
				extents.xMin = center.x - s_MinimapMinGraphExtents * 0.5f;
				extents.xMax = center.x + s_MinimapMinGraphExtents * 0.5f;
			}
			if (extents.height < s_MinimapMinGraphExtents)
			{
				extents.yMin = center.y - s_MinimapMinGraphExtents * 0.5f;
				extents.yMax = center.y + s_MinimapMinGraphExtents * 0.5f;
			}

			_GraphToMinimapTransform = CalcTransform(minimapRect, extents);
			_MinimapToGraphTransform = _GraphToMinimapTransform.inverse;

			isSettedTransform = true;

			UpdateViewportElement();

			using (var e = MinimapTransformEvent.GetPooled())
			{
				e.target = this;
				SendEvent(e);
			}
		}

		static Matrix4x4 CalcTransform(Rect minimapRect, Rect graphExtents)
		{
			// aspect fit
			float widthRatio = minimapRect.width / graphExtents.width;
			float heightRatio = minimapRect.height / graphExtents.height;

			float zoomLevel = widthRatio > heightRatio ? heightRatio : widthRatio;
			float resizedWidth = graphExtents.width * zoomLevel;
			float resizedHeight = graphExtents.height * zoomLevel;

			Rect viewportRect = new Rect(minimapRect.center.x - resizedWidth * 0.5f, minimapRect.center.y - resizedHeight * 0.5f, resizedWidth, resizedHeight);

			Vector3 minimapScale = new Vector3(viewportRect.width / graphExtents.width, viewportRect.height / graphExtents.height, 1f);
			return Matrix4x4.TRS(viewportRect.position, Quaternion.identity, minimapScale) * Matrix4x4.TRS(-graphExtents.position, Quaternion.identity, Vector3.one);
		}

		void UpdateViewportElement()
		{
			var graphView = _Window.graphView;
			Rect viewportRect = GraphToMinimap(graphView.graphViewport);

			_ViewportElement.SetLayout(viewportRect);

			if (viewportRect.width > 15f && viewportRect.height >= 15f)
			{
				_ViewportCross.style.display = StyleKeyword.Null;
			}
			else
			{
				_ViewportCross.style.display = DisplayStyle.None;
			}
		}

		public Vector2 GraphToMinimap(Vector2 point)
		{
			return _GraphToMinimapTransform.MultiplyPoint(point);
		}

		public Vector2 MinimapToGraph(Vector2 point)
		{
			return _MinimapToGraphTransform.MultiplyPoint(point);
		}

		public Rect GraphToMinimap(Rect rect)
		{
			rect.min = GraphToMinimap(rect.min);
			rect.max = GraphToMinimap(rect.max);

			return rect;
		}

		public Rect MinimapToGraph(Rect rect)
		{
			rect.min = MinimapToGraph(rect.min);
			rect.max = MinimapToGraph(rect.max);

			return rect;
		}

		public Bezier2D GraphToMinimap(Bezier2D bezier)
		{
			if (bezier == null)
			{
				return null;
			}

			Bezier2D newBezier = new Bezier2D(
				GraphToMinimap(bezier.startPosition),
				GraphToMinimap(bezier.startControl),
				GraphToMinimap(bezier.endPosition),
				GraphToMinimap(bezier.endControl)
			);

			return newBezier;
		}

		public Bezier2D MinimapToGraph(Bezier2D bezier)
		{
			if (bezier == null)
			{
				return null;
			}

			Bezier2D newBezier = new Bezier2D(
				MinimapToGraph(bezier.startPosition),
				MinimapToGraph(bezier.startControl),
				MinimapToGraph(bezier.endPosition),
				MinimapToGraph(bezier.endControl)
			);

			return newBezier;
		}

		sealed class ViewportManipulator : DragManipulator
		{
			private MinimapView _MinimapView;
			private Vector2 _BeginPosition;

			public ViewportManipulator(MinimapView minimapView)
			{
				_MinimapView = minimapView;
				activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse });
			}

			protected override void OnMouseDown(MouseDownEvent e)
			{
				_BeginPosition = e.localMousePosition;

				e.StopPropagation();
			}

			protected override void OnMouseMove(MouseMoveEvent e)
			{
				Vector2 scrollPos = e.localMousePosition - _BeginPosition + target.layout.position;

				var graphView = _MinimapView._Window.graphView;

				scrollPos = _MinimapView.MinimapToGraph(scrollPos);
				scrollPos.x = (int)scrollPos.x;
				scrollPos.y = (int)scrollPos.y;

				graphView.SetScroll(scrollPos, true, true);
			}
		}
	}
}