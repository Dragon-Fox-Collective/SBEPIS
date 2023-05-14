//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	public sealed class LanguagePath : LanguagePathInternal
	{
		[MenuItem("Assets/Create/Arbor/Editor/LanguagePath", false, 124)]
		static void CreateLanguagePath()
		{
			LanguagePath languagePath = ScriptableObject.CreateInstance<LanguagePath>();
			ProjectWindowUtil.CreateAsset(languagePath, "LanguagePath.asset");
		}
	}
}