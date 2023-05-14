//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	internal static class ArborFSMScriptCreator
	{
		static readonly string _StateBehaviourCSharpTemplatePath = @"C# Script-NewBehaviourScript";

		[MenuItem("Assets/Create/Arbor/StateBehaviour C# Script", false, 100)]
		public static void CreateCSharpScriptStateBehaviour()
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateScriptAsset>(), "NewStateBehaviourScript.cs", DefaultScriptIcon.CSharpIcon, _StateBehaviourCSharpTemplatePath);
		}
	}
}
