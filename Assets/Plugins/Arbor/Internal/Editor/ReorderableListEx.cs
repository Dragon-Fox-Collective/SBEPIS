//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace ArborEditor
{
	public sealed class ReorderableListEx : ReorderableList
	{
		static readonly RectOffset s_EntryBackPadding = new RectOffset(2, 2, 0, 0);

		public new ElementCallbackDelegate drawElementCallback;

		public ReorderableListEx(IList elements, Type elementType) : base(elements, elementType)
		{
			Initialize();
		}

		public ReorderableListEx(IList elements, Type elementType, bool draggable, bool displayHeader, bool displayAddButton, bool displayRemoveButton) : base(elements, elementType, draggable, displayHeader, displayAddButton, displayRemoveButton)
		{
			Initialize();
		}

		public ReorderableListEx(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
		{
			Initialize();
		}

		public ReorderableListEx(SerializedObject serializedObject, SerializedProperty elements, bool draggable, bool displayHeader, bool displayAddButton, bool displayRemoveButton) : base(serializedObject, elements, draggable, displayHeader, displayAddButton, displayRemoveButton)
		{
			Initialize();
		}

		void Initialize()
		{
			base.drawElementCallback = DrawElement;
			drawElementBackgroundCallback = DrawElementBackground;
		}

		void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
#if !(UNITY_2020_2_OR_NEWER && !ARBOR_DLL)
			if (!IsVisible(rect))
			{
				return;
			}
#endif

			drawElementCallback?.Invoke(rect, index, isActive, isFocused);
		}

		void DrawElementBackground(Rect rect, int index, bool isActive, bool isFocused)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}

#if !(UNITY_2020_2_OR_NEWER && !ARBOR_DLL)
			if (!IsVisible(rect))
			{
				return;
			}
#endif

			if (isActive)
			{
				defaultBehaviours.DrawElementBackground(rect, index, isActive, isFocused, false);
			}
			else
			{
				rect = s_EntryBackPadding.Remove(rect);

				bool even = ((index + 1) % 2 == 0);

				GUIStyle style = even ? BuiltInStyles.entryBackEven : BuiltInStyles.entryBackOdd;

				Color backgroundColor = GUI.backgroundColor;
				GUI.backgroundColor *= new Color(0.95f, 0.95f, 0.95f, 1.0f);
				style.Draw(rect, GUIContent.none, false, false, false, isFocused);
				GUI.backgroundColor = backgroundColor;
			}
		}

#if !(UNITY_2020_2_OR_NEWER && !ARBOR_DLL)
		private bool _IsSettedVisibleRect = false;
		private Rect _VisibleRect;

		private bool IsVisible(Rect rect)
		{
			return !_IsSettedVisibleRect || _VisibleRect.Overlaps(rect);
		}
#endif

		public void DoLayoutList(Rect visibleRect)
		{
#if UNITY_2020_2_OR_NEWER && !ARBOR_DLL
			GUILayout.BeginVertical();

			Rect listRect = GUILayoutUtility.GetRect(0, GetHeight(), GUILayout.ExpandWidth(true));

			visibleRect.y -= listRect.y;
			base.DoList(listRect, visibleRect);
			
			GUILayout.EndVertical();
#else
			_IsSettedVisibleRect = true;
			_VisibleRect = visibleRect;
			try
			{
				base.DoLayoutList();
			}
			finally
			{
				_IsSettedVisibleRect = false;
			}
#endif
		}
	}
}