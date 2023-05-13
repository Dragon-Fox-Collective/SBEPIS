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
	internal sealed class MouseDownButton : VisualElement
	{
		public System.Action onDown = null;
		public MouseDownButton(System.Action onDown)
		{
			this.onDown = onDown;

			AddToClassList("unity-button");

			RegisterCallback<MouseDownEvent>(OnMouseDown);
		}

		void OnMouseDown(MouseDownEvent e)
		{
			onDown?.Invoke();

			e.StopPropagation();
		}
	}
}