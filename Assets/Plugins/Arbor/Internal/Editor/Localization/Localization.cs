//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor
{
	public static class Localization
	{
		public static GUIContent GetTextContent(string key)
		{
			return LanguageManager.GetTextContent(ArborSettings.currentLanguage, key);
		}

		public static string GetWord(string key)
		{
			return LanguageManager.GetWord(ArborSettings.currentLanguage, key);
		}
	}
}
