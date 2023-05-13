//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	public static class DefaultScriptIcon
	{
		public static readonly Texture2D CSharpIcon;
		public static readonly Texture2D JavaScriptIcon;
		public static readonly Texture2D BooScriptIcon;
		public static readonly Texture2D ScriptableObjectIcon;

		static DefaultScriptIcon()
		{
			CSharpIcon = EditorGUIUtility.FindTexture("cs Script Icon");
			JavaScriptIcon = EditorGUIUtility.FindTexture("js Script Icon");
			BooScriptIcon = EditorGUIUtility.FindTexture("boo Script Icon");
			ScriptableObjectIcon = EditorGUIUtility.FindTexture("ScriptableObject Icon");
		}

		public static bool IsDefaultScriptIcon(Texture icon)
		{
			return icon == CSharpIcon || icon == JavaScriptIcon || icon == BooScriptIcon || icon == ScriptableObjectIcon;
		}
	}
}