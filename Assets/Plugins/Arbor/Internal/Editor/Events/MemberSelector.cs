using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ArborEditor.Events
{
	using Arbor.Events;

	public static class MemberSelector
	{
		public static string kNoFunctionText => MemberPopupWindow.kNoFunctionText;

		private static readonly int s_MethodPopupHash = "ArborEditor.MemberSelector.s_MethodPopupHash".GetHashCode();

		public static MemberInfo PopupField(Rect position, int controlID, MemberInfo selected, System.Type type, bool hasFilter, MemberFilterFlags memberFlags, GUIContent selectedName)
		{
			using (new EditorGUI.DisabledGroupScope(EditorApplication.isCompiling))
			{
				selected = MemberPopupWindow.GetSelectedValueForControl(controlID, selected);

				Event current = Event.current;

				EventType eventType = current.GetTypeForControl(controlID);

				selectedName = selectedName ?? new GUIContent(selected != null ? ArborEventUtility.GetMemberName(selected) : MemberPopupWindow.kNoFunctionText);
				GUIStyle style = EditorStyles.popup;

				switch (eventType)
				{
					case EventType.MouseDown:
						if (current.button == 0 && position.Contains(current.mousePosition))
						{
							Rect buttonRect = GUIUtility.GUIToScreenRect(position);
							MemberPopupWindow.Open(buttonRect, controlID, selected, type, hasFilter, memberFlags);
							current.Use();
						}
						break;
					case EventType.Repaint:
						style.Draw(position, selectedName, controlID);
						break;
				}

				return selected;
			}
		}

		public static MemberInfo PopupField(Rect position, int controlID, MemberInfo selected, System.Type type, bool hasFilter, MemberFilterFlags memberFlags, GUIContent selectedName, GUIContent label)
		{
			position = EditorGUI.PrefixLabel(position, controlID, label);

			return PopupField(position, controlID, selected, type, hasFilter, memberFlags, selectedName);
		}

		public static MemberInfo PopupField(Rect position, MemberInfo selected, System.Type type, bool hasFilter, MemberFilterFlags memberFlags, GUIContent selectedName)
		{
			int controlID = GUIUtility.GetControlID(s_MethodPopupHash, FocusType.Passive, position);

			return PopupField(position, controlID, selected, type, hasFilter, memberFlags, selectedName, GUIContent.none);
		}
	}
}