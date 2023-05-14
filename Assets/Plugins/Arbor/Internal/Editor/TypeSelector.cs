using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	public static class TypeSelector
	{
		private static readonly int s_TypePopupHash = "ArborEditor.TypeSelector.s_TypePopupHash".GetHashCode();

		public static Type PopupField(Rect position, Type selected, GUIContent label, IDefinableType definableType)
		{
			return PopupField(position, selected, label, definableType, false, (TypeFilterFlags)0);
		}

		public static Type PopupField(Rect position, Type selected, GUIContent label, IDefinableType definableType, bool hasFilter, TypeFilterFlags filterFlags)
		{
			int controlID = GUIUtility.GetControlID(s_TypePopupHash, FocusType.Keyboard, position);

			return PopupField(position, controlID, selected, label, definableType, hasFilter, filterFlags);
		}

		public static Type PopupField(Rect position, Type selected, GUIContent label, IDefinableType definableType, bool hasFilter, TypeFilterFlags filterFlags, GUIContent selectedName)
		{
			int controlID = GUIUtility.GetControlID(s_TypePopupHash, FocusType.Keyboard, position);

			return PopupField(position, controlID, selected, label, definableType, hasFilter, filterFlags, selectedName);
		}

		public static Type PopupField(Rect position, int controlID, Type selected, GUIContent label, IDefinableType definableType, bool hasFilter, TypeFilterFlags filterFlags)
		{
			position = EditorGUI.PrefixLabel(position, controlID, label);

			return PopupField(position, controlID, selected, definableType, hasFilter, filterFlags);
		}

		public static Type PopupField(Rect position, int controlID, Type selected, GUIContent label, IDefinableType definableType, bool hasFilter, TypeFilterFlags filterFlags, GUIContent selectedName)
		{
			position = EditorGUI.PrefixLabel(position, controlID, label);

			return PopupField(position, controlID, selected, definableType, hasFilter, filterFlags, selectedName);
		}

		public static Type PopupField(Rect position, int controlID, Type selected, IDefinableType definableType, bool hasFilter, TypeFilterFlags filterFlags)
		{
			return PopupField(position, controlID, selected, definableType, hasFilter, filterFlags, null);
		}

		public static Type PopupField(Rect position, int controlID, Type selected, IDefinableType definableType, bool hasFilter, TypeFilterFlags filterFlags, GUIContent selectedName)
		{
			using (new EditorGUI.DisabledGroupScope(EditorApplication.isCompiling))
			{
				selected = TypePopupWindow.GetSelectedValueForControl(controlID, selected);

				Event current = Event.current;

				EventType eventType = current.GetTypeForControl(controlID);

				selectedName = selectedName ?? new GUIContent(selected != null ? TypeUtility.GetTypeName(selected) : TypePopupWindow.kNoneText);
				GUIStyle style = EditorStyles.popup;

				switch (eventType)
				{
					case EventType.MouseDown:
						if (current.button == 0 && position.Contains(current.mousePosition))
						{
							Rect buttonRect = GUIUtility.GUIToScreenRect(position);
							TypePopupWindow.Open(buttonRect, controlID, selected, definableType, hasFilter, filterFlags);
							current.Use();
						}
						break;
					case EventType.KeyDown:
						if (current.MainActionKeyForControl(controlID))
						{
							Rect buttonRect = GUIUtility.GUIToScreenRect(position);
							TypePopupWindow.Open(buttonRect, controlID, selected, definableType, hasFilter, filterFlags);
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
	}
}