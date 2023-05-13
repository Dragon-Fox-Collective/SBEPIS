//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor.Extensions;

	public static class PopupWindowUtility
	{
		public static void Show(Rect activatorRect, PopupWindowContent windowContent, bool onGUI)
		{
			try
			{
				PopupWindow.Show(activatorRect, windowContent);
			}
			catch (System.Exception ex)
			{
				if (onGUI || !UnityEditorBridge.GUIUtilityBridge.ShouldRethrowException(ex))
				{
					throw;
				}
			}
		}
	}
}