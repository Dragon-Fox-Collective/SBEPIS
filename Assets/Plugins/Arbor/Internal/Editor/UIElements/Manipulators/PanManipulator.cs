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
	internal sealed class PanManipulator : DragManipulator
	{
		private readonly float kWheelStep = 20f;

		private VisualElement m_CoordinatesTarget;
		private System.Action<Vector2> m_OnScroll;
		private Vector2 m_LastPosition;

		public PanManipulator(VisualElement coordinatesTarget, System.Action<Vector2> onScroll) : base(TrickleDownMode.Both)
		{
			m_CoordinatesTarget = coordinatesTarget;
			m_OnScroll = onScroll;

			ManipulatorActivationFilter filter1 = new ManipulatorActivationFilter();
			filter1.button = MouseButton.LeftMouse;
			filter1.modifiers = EventModifiers.Alt;
			activators.Add(filter1);

			ManipulatorActivationFilter filter2 = new ManipulatorActivationFilter();
			filter2.button = MouseButton.MiddleMouse;
			activators.Add(filter2);
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
			if (ArborSettings.GetMouseWheelMode(e.actionKey) != MouseWheelMode.Scroll)
			{
				return;
			}

			Vector2 delta = e.delta;
			if (Application.platform != RuntimePlatform.OSXEditor && e.shiftKey)
			{
				// swap x <- -> y
				(delta.x, delta.y) = (delta.y, delta.x);
			}
			m_OnScroll?.Invoke(delta * kWheelStep);
			e.StopPropagation();
		}

		protected override void OnMouseDown(MouseDownEvent e)
		{
			if (e.propagationPhase == PropagationPhase.TrickleDown && e.button != 2)
			{
				return;
			}

			m_LastPosition = ToCoordinatesTarget(e.currentTarget as VisualElement, e.localMousePosition);

			EditorGUIUtility.SetWantsMouseJumping(1);
			e.StopPropagation();
		}

		protected override void OnMouseMove(MouseMoveEvent e)
		{
			Vector2 currentPosition = ToCoordinatesTarget(e.currentTarget as VisualElement, e.localMousePosition);
			Vector2 delta = currentPosition - m_LastPosition;

			m_OnScroll?.Invoke(-delta);

			m_LastPosition = ToCoordinatesTarget(e.currentTarget as VisualElement, e.localMousePosition);
			
			e.StopPropagation();
		}

		protected override void OnEndDrag()
		{
			base.OnEndDrag();

			EditorGUIUtility.SetWantsMouseJumping(0);
		}
	}
}