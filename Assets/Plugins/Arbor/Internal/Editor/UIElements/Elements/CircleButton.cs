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
	internal sealed class CircleButton : Button
	{
		public static readonly new string ussClassName = "circle-button";

		public CircleButton(System.Action clickEvent) : base(clickEvent)
		{
			RemoveFromClassList(Button.ussClassName);
			AddToClassList(ussClassName);
		}

		public override bool ContainsPoint(Vector2 localPoint)
		{
			float radius = paddingRect.width * 0.5f;

			return Vector2.Distance(paddingRect.center, localPoint) <= radius;
		}
	}
}