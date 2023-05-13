//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	internal static class EditorExtensionMethods
	{
		internal static bool MainActionKeyForControl(this Event evt, int controlId)
		{
			if (EditorGUIUtility.keyboardControl != controlId)
			{
				return false;
			}

			bool anyModifiers = (evt.alt || evt.shift || evt.command || evt.control);

			// Block window maximize (on OSX ML, we need to show the menu as part of the KeyCode event, so we can't do the usual check)
			if (evt.type == EventType.KeyDown && evt.character == ' ' && !anyModifiers)
			{
				evt.Use();
				return false;
			}

			// Space or return is action key
			return evt.type == EventType.KeyDown &&
				(evt.keyCode == KeyCode.Space || evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter) &&
				!anyModifiers;
		}
	}
}