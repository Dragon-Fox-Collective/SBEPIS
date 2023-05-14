//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree
{
	internal static class BehaviourTreeScriptCreator
	{
		static readonly string _ActionBehaviourCSharpTemplatePath = @"BehaviourTree/C# Script-NewActionBehaviourScript";

		static readonly string _DecoratorCSharpTemplatePath = @"BehaviourTree/C# Script-NewDecoratorScript";

		static readonly string _ServiceCSharpTemplatePath = @"BehaviourTree/C# Script-NewServiceScript";

		[MenuItem("Assets/Create/Arbor/BehaviourTree/ActionBehaviour C# Script", false, 113)]
		public static void CreateCSharpScriptActionBehaviour()
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateScriptAsset>(), "NewActionBehaviourScript.cs", DefaultScriptIcon.CSharpIcon, _ActionBehaviourCSharpTemplatePath);
		}

		[MenuItem("Assets/Create/Arbor/BehaviourTree/Decorator C# Script", false, 114)]
		public static void CreateCSharpScriptDecorator()
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateScriptAsset>(), "NewDecoratorScript.cs", DefaultScriptIcon.CSharpIcon, _DecoratorCSharpTemplatePath);
		}

		[MenuItem("Assets/Create/Arbor/BehaviourTree/Service C# Script", false, 115)]
		public static void CreateCSharpScriptService()
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateScriptAsset>(), "NewServiceScript.cs", DefaultScriptIcon.CSharpIcon, _ServiceCSharpTemplatePath);
		}
	}
}
