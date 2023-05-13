//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace ArborEditor.UIElements
{
	internal sealed class DropdownButton : ToolbarDropdown
	{
		public DropdownButton(System.Action clickEvent) : base(clickEvent)
		{
			style.flexDirection = FlexDirection.Row;

			RemoveFromClassList(ToolbarDropdown.ussClassName);
			AddToClassList(Button.ussClassName);
		}
	}
}