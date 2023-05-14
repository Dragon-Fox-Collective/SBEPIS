//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	public static class BuiltInStyles
	{
		// Built in
		public static readonly GUIStyle invisibleButton;
		public static readonly GUIStyle RLHeader;
		public static readonly GUIStyle RLBackground;
		public static readonly GUIStyle toolbarSearchField;
		public static readonly GUIStyle toolbarSearchFieldRaw;
		public static readonly GUIStyle shurikenDropDown;
		public static readonly GUIStyle largeButton;
		public static readonly GUIStyle lockButton;
		public static readonly GUIStyle countBadgeLarge;
		public static readonly GUIStyle colorPickerBox;
		public static readonly GUIStyle renameTextField;
		public static readonly GUIStyle parameterListHeader;
		public static readonly GUIStyle entryBackEven;
		public static readonly GUIStyle entryBackOdd;
		public static readonly GUIStyle toolbar;
		public static readonly GUIStyle largeDropDown;

		private static readonly GUISkin _EditorSkin;

		static BuiltInStyles()
		{
			_EditorSkin = GUI.skin;

			// Built in
			invisibleButton = GetEditorStyle("InvisibleButton");
			RLHeader = GetEditorStyle("RL Header");
			shurikenDropDown = GetEditorStyle("ShurikenDropDown");
			lockButton = GetEditorStyle("IN LockButton");
			colorPickerBox = GetEditorStyle("ColorPickerBox");
			renameTextField = GetEditorStyle("PR TextField");
			entryBackEven = new GUIStyle(GetEditorStyle("CN EntryBackEven"));
			entryBackEven.margin = new RectOffset();
			entryBackOdd = new GUIStyle(GetEditorStyle("CN EntryBackOdd"));
			entryBackOdd.margin = new RectOffset();

			RLBackground = new GUIStyle(GetEditorStyle("RL Background"))
			{
				stretchHeight = false,
			};

			GUIStyle toolbarSearchField = GetEditorStyle("ToolbarSeachTextField");
			toolbarSearchFieldRaw = toolbarSearchField;
			if (toolbarSearchField != null && toolbarSearchField != GUIStyle.none)
			{
				BuiltInStyles.toolbarSearchField = new GUIStyle(toolbarSearchField)
				{
					margin = new RectOffset(5, 4, 4, 5)
				};
			}

			GUIStyle countBadge = GetEditorStyle("CN CountBadge");
			countBadgeLarge = new GUIStyle(countBadge)
			{
				fixedHeight = 0f,
			};

			// Create LargeButton based on Button
			// (LargeButton became fixed height at Unity 2019.1.0a5)
			GUIStyle largeButton_ = GetEditorStyle("LargeButton");
			
			largeButton = new GUIStyle(GetEditorStyle("Button"))
			{
				font = largeButton_.font,
				fontSize = largeButton_.fontSize,
				padding = CopyOffset(largeButton_.padding),
				margin = CopyOffset(largeButton_.margin),
			};

			parameterListHeader = new GUIStyle(GetEditorStyle("RL Header"));
			parameterListHeader.fixedHeight = 0f;

			GUIStyle builtInToolbar = EditorStyles.toolbar;

			toolbar = new GUIStyle(builtInToolbar);
			toolbar.padding.left = 6;
			toolbar.padding.right = 6;

			largeDropDown = new GUIStyle(GetEditorStyle("DropDownButton"));
			largeDropDown.padding.left = Mathf.Max(largeDropDown.padding.left, largeDropDown.border.left + 2);
			largeDropDown.padding.right = Mathf.Max(largeDropDown.padding.right, largeDropDown.border.right + 1);
			largeDropDown.fixedHeight = Mathf.Max(largeDropDown.fixedHeight, 24);
			largeDropDown.fontSize = Mathf.Max(largeDropDown.fontSize, 12);
			largeDropDown.alignment = TextAnchor.MiddleCenter;
		}

		internal static GUIStyle ms_Error;
		internal static GUIStyle error
		{
			get
			{
				if (ms_Error == null)
				{
					ms_Error = new GUIStyle();
					ms_Error.name = "StyleNotFoundError";
				}
				return ms_Error;
			}
		}

		static RectOffset CopyOffset(RectOffset offset)
		{
			return new RectOffset(offset.left, offset.right, offset.top, offset.bottom);
		}

		static GUIStyle GetEditorStyle(string styleName)
		{
			GUIStyle s = _EditorSkin.FindStyle(styleName) ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);
			if (s == null)
			{
				Debug.LogError("Missing built-in guistyle " + styleName);
				s = error;
			}
			return s;
		}
	}
}