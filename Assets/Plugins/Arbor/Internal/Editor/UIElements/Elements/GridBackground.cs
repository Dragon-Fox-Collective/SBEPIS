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
	internal sealed class GridBackground : ImmediateGUIElement
	{
		private VisualElement _Viewport;

		public GridBackground(VisualElement viewport)
		{
			_Viewport = viewport;
			style.position = Position.Absolute;
		}

		protected override void OnImmediateGUI()
		{
			var layout = _Viewport.layout;
			var rect = _Viewport.ChangeCoordinatesTo(this, layout);

			float scale = layout.size.magnitude / rect.size.magnitude;

			EditorGUITools.DrawGrid(rect, scale);
		}
	}
}