//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	internal static class ArborScriptCreator
	{
		static readonly string _CalculatorCSharpTemplatePath = @"C# Script-NewCalculatorScript";

		[MenuItem("Assets/Create/Arbor/Calculator C# Script", false, 101)]
		public static void CreateCSharpScriptCalculator()
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateScriptAsset>(), "NewCalculatorScript.cs", DefaultScriptIcon.CSharpIcon, _CalculatorCSharpTemplatePath);
		}
	}
}
