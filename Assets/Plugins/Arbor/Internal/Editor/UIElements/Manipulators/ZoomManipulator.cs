//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	internal sealed class ZoomManipulator : DragManipulator
	{
		private VisualElement m_CoordinatesTarget;
		private System.Action<Vector2, float> m_OnZoom;
		private Vector2 m_Start;
		private Vector2 m_Last;
		private Vector2 m_ZoomCenter;

		public float zoomStep
		{
			get;
			set;
		}

		public ZoomManipulator(VisualElement coordinatesTarget, System.Action<Vector2, float> onZoom) : base(TrickleDownMode.TrickleDown)
		{
			m_CoordinatesTarget = coordinatesTarget;
			m_OnZoom = onZoom;
			zoomStep = 0.01f;

			ManipulatorActivationFilter filter = new ManipulatorActivationFilter();
			filter.button = MouseButton.RightMouse;
			filter.modifiers = EventModifiers.Alt;
			activators.Add(filter);
		}

		protected override void RegisterCallbacksOnTarget()
		{
			base.RegisterCallbacksOnTarget();
			target.RegisterCallback<WheelEvent>(OnScroll, TrickleDown.TrickleDown);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.UnregisterCallbacksFromTarget();
			target.UnregisterCallback<WheelEvent>(OnScroll, TrickleDown.TrickleDown);
		}

		Vector2 ToCoordinatesTarget(VisualElement source, Vector2 position)
		{
			return source.ChangeCoordinatesTo(m_CoordinatesTarget, position);
		}

		private void OnScroll(WheelEvent e)
		{
			if (ArborSettings.GetMouseWheelMode(e.actionKey) != MouseWheelMode.Zoom)
			{
				return;
			}

			Vector2 zoomCenter = ToCoordinatesTarget(e.currentTarget as VisualElement, e.localMousePosition);
			m_OnZoom?.Invoke(zoomCenter, -e.delta.y * zoomStep);
			e.StopPropagation();
		}

		protected override void OnMouseDown(MouseDownEvent e)
		{
			m_Start = m_Last = e.localMousePosition;
			m_ZoomCenter = ToCoordinatesTarget(target, m_Start);
			e.StopPropagation();
		}

		protected override void OnMouseMove(MouseMoveEvent e)
		{
			Vector2 vector2 = e.localMousePosition - m_Last;
			m_OnZoom?.Invoke(m_ZoomCenter, (vector2.x + vector2.y) * zoomStep);
			e.StopPropagation();
			m_Last = e.localMousePosition;
		}
	}
}