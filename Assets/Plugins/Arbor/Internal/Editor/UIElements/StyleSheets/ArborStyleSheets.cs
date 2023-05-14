//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	public static class ArborStyleSheets
	{
		private static bool s_RetryLoadStyleSheet = false;
		private static List<VisualElement> s_Targets = new List<VisualElement>();

		private static List<string> s_LoadPathes = new List<string>();
		private static List<StyleSheet> s_StyleSheets = new List<StyleSheet>();

		private static bool s_CheckBuiltInStyle = true;

		static readonly CustomStyleProperty<Color> s_ColorsInspectorTitlebarBackgroundProperty = new CustomStyleProperty<Color>("--unity-colors-inspector_titlebar-background");

		static ArborStyleSheets()
		{
			s_LoadPathes.Add("StyleSheets/ArborEditorWindow/ArborEditorWindow");
			var themePath = EditorGUIUtility.isProSkin ? "StyleSheets/ArborEditorWindow/ArborEditorWindowDark" : "StyleSheets/ArborEditorWindow/ArborEditorWindowLight";
			s_LoadPathes.Add(themePath);

			Load();
		}

		public static void Setup(VisualElement target)
		{
			if (s_StyleSheets.Count != 0 && !s_RetryLoadStyleSheet)
			{
				SetStyleSheets(target);
			}
			else
			{
				s_Targets.Add(target);
			}
		}

		static void SetStyleSheets(VisualElement target)
		{
			for (int i = 0; i < s_StyleSheets.Count; i++)
			{
				target.styleSheets.Add(s_StyleSheets[i]);
			}

			if (s_CheckBuiltInStyle)
			{
				target.RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
			}
		}

		static void Load()
		{
			int loadCount = 0;
			for (int i = 0; i < s_LoadPathes.Count; i++)
			{
				var path = s_LoadPathes[i];
				var styleSheet = EditorResources.Load<StyleSheet>(path, ".uss");
				if (styleSheet != null)
				{
					s_StyleSheets.Add(styleSheet);
					loadCount++;
				}
				else if (!s_RetryLoadStyleSheet)
				{
					// Simultaneous editing of uss and source code fails to load uss.
					// Retry with delayCall.
					s_RetryLoadStyleSheet = true;
					EditorApplication.delayCall += Load;
					break;
				}
			}

			s_LoadPathes.RemoveRange(0, loadCount);

			if (s_LoadPathes.Count == 0)
			{
				foreach (var target in s_Targets)
				{
					SetStyleSheets(target);
				}

				s_Targets.Clear();
			}
		}

		static void OnCustomStyleResolved(CustomStyleResolvedEvent e)
		{
			var rootVisualElement = e.target as VisualElement;
			bool hasBuiltinVariables = rootVisualElement.customStyle.TryGetValue(s_ColorsInspectorTitlebarBackgroundProperty, out var backgroundColor);
			if (!hasBuiltinVariables)
			{
				string commonStyleSheet = "StyleSheets/BuiltInVariables/" + (EditorGUIUtility.isProSkin ? "dark" : "light");
				var styleSheet = EditorResources.Load<StyleSheet>(commonStyleSheet, ".uss");
				if (styleSheet != null)
				{
					rootVisualElement.styleSheets.Add(styleSheet);
					s_StyleSheets.Add(styleSheet);
				}
			}

			rootVisualElement.UnregisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
			s_CheckBuiltInStyle = false;
		}
	}
}