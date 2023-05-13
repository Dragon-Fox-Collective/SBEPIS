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
	internal sealed class ContextClickManipulator : MouseManipulator
	{
		System.Action<ContextClickEvent> _ClickEvent;

		public ContextClickManipulator(System.Action<ContextClickEvent> clickEvent)
		{
			_ClickEvent = clickEvent;
			activators.Add(new ManipulatorActivationFilter { button = MouseButton.RightMouse });
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			{
				activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse, modifiers = EventModifiers.Control });
			}
		}

		protected override void RegisterCallbacksOnTarget()
		{
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			{
				target.RegisterCallback<MouseDownEvent>(OnMouseUpDownEvent);
			}
			else
			{
				target.RegisterCallback<MouseUpEvent>(OnMouseUpDownEvent);
			}
			target.RegisterCallback<ContextClickEvent>(OnContextClickEvent);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			{
				target.UnregisterCallback<MouseDownEvent>(OnMouseUpDownEvent);
			}
			else
			{
				target.UnregisterCallback<MouseUpEvent>(OnMouseUpDownEvent);
			}
			target.UnregisterCallback<ContextClickEvent>(OnContextClickEvent);
		}

		void OnMouseUpDownEvent(IMouseEvent evt)
		{
			EventBase e = evt as EventBase;

			IPanel panel = (e.target as VisualElement)?.panel;
			if (panel.GetCapturingElement(PointerId.mousePointerId) != null)
			{
				return;
			}

			if (!CanStartManipulation(evt))
			{
				return;
			}

			if (e.target is IMGUIContainer)
			{
				return;
			}

			using (ContextClickEvent cce = ContextClickEvent.GetPooled(evt))
			{
				target.SendEvent(cce);
			}

			e.StopPropagation();
			e.PreventDefault();
		}

		void OnContextClickEvent(ContextClickEvent evt)
		{
			IPanel panel = (evt.target as VisualElement)?.panel;
			if (panel.GetCapturingElement(PointerId.mousePointerId) != null)
			{
				return;
			}

			_ClickEvent?.Invoke(evt);
		}
	}
}