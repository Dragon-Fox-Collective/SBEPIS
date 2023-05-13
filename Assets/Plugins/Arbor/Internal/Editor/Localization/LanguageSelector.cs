using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;
	internal static class LanguageSelector
	{
		private static GUIContent[] _LanguageLabels;
		private static SystemLanguage _LastLanguage;

		static readonly int s_LanguageModeCount = 3;

		static LanguageSelector()
		{
			LanguageManager.onRebuild += () =>
			{
				_LanguageLabels = null;
			};
		}

		static GUIContent[] GetLanguageLabels()
		{
			bool changed = _LastLanguage != ArborSettings.currentLanguage;
			_LastLanguage = ArborSettings.currentLanguage;

			int languageCount = LanguageManager.languageCount;

			if (_LanguageLabels == null || _LanguageLabels.Length != languageCount + s_LanguageModeCount)
			{
				_LanguageLabels = new GUIContent[languageCount + s_LanguageModeCount];

				changed = true;
			}

			if (changed)
			{
				_LanguageLabels[0] = GUIContentCaches.Get(Localization.GetWord("System") + "(" + Localization.GetWord(LanguageManager.GetSystemLanguage().ToString()) + ")");
				_LanguageLabels[1] = GUIContentCaches.Get(Localization.GetWord("UnityEditor") + "(" + Localization.GetWord(LanguageManager.GetEditorLanguage().ToString()) + ")");

				// separator
				_LanguageLabels[s_LanguageModeCount - 1] = GUIContentCaches.Get("");

				for (int i = 0; i < languageCount; i++)
				{
					SystemLanguage language = LanguageManager.GetLanguageAt(i);
					_LanguageLabels[i + s_LanguageModeCount] = Localization.GetTextContent(language.ToString());
				}
			}

			return _LanguageLabels;
		}

		static int GetSelectedIndex()
		{
			int selectIndex = 0;

			switch (ArborSettings.languageMode)
			{
				case LanguageMode.Custom:
					{
						int languageCount = LanguageManager.languageCount;
						for (int i = 0; i < languageCount; i++)
						{
							SystemLanguage language = LanguageManager.GetLanguageAt(i);
							if (language == ArborSettings.language)
							{
								selectIndex = i + s_LanguageModeCount;
								break;
							}
						}
					}
					break;
				case LanguageMode.System:
					selectIndex = 0;
					break;
				case LanguageMode.UnityEditor:
					selectIndex = 1;
					break;
			}

			return selectIndex;
		}

		static void SetSelectIndex(int selectIndex)
		{
			if (selectIndex == 0)
			{
				ArborSettings.languageMode = LanguageMode.System;
			}
			else if (selectIndex == 1)
			{
				ArborSettings.languageMode = LanguageMode.UnityEditor;
			}
			else if (selectIndex == s_LanguageModeCount - 1)
			{
				// separator
			}
			else
			{
				ArborSettings.languageMode = LanguageMode.Custom;
				ArborSettings.language = LanguageManager.GetLanguageAt(selectIndex - s_LanguageModeCount);
			}
		}

		static void OnSelectPopupMenu(object userData, string[] options, int selected)
		{
			SetSelectIndex(selected);
		}

		public static void DisplayLanguagePopup(Rect position)
		{
			EditorUtility.DisplayCustomMenu(position, GetLanguageLabels(), GetSelectedIndex(), OnSelectPopupMenu, null);
		}
		
		public static void LanguagePopup(Rect position, GUIContent label, GUIStyle style)
		{
			using (new ProfilerScope("LanguagePopup"))
			{
				var languageLabels = GetLanguageLabels();
				
				int selectIndex = GetSelectedIndex();

				EditorGUI.BeginChangeCheck();
				selectIndex = EditorGUI.Popup(position, label, selectIndex, languageLabels, style);
				if (EditorGUI.EndChangeCheck())
				{
					SetSelectIndex(selectIndex);
				}
			}
		}

		public static GUIContent GetCurrentLanguageLabel()
		{
			var contents = GetLanguageLabels();
			int selectedIndex = GetSelectedIndex();
			return contents[selectedIndex];
		}
	}
}