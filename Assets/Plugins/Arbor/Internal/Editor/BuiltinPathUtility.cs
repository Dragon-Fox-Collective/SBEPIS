//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace ArborEditor
{
	using Arbor;

	public static class BuiltinPathUtility
	{
		private static List<BuiltinPath> _Paths = new List<BuiltinPath>();

		static BuiltinPathUtility()
		{
			System.Type builtInPathType = typeof(BuiltinPath);

			Assembly builtInPathAssembly = TypeUtility.GetAssembly(builtInPathType);

			var loadedAssemblies = UnityEditorBridge.EditorAssembliesBridge.loadedAssemblies;
			for (int i = 0; i < loadedAssemblies.Length; i++)
			{
				Assembly assembly = loadedAssemblies[i];
				if (!TypeUtility.IsReferenceable(assembly, builtInPathAssembly))
				{
					continue;
				}
				try
				{
					var attributes = AttributeHelper.GetAttributes<BuiltinPath>(assembly);
					for (int attrIndex = 0; attrIndex < attributes.Length; attrIndex++)
					{
						BuiltinPath builtinPath = attributes[attrIndex] as BuiltinPath;
						_Paths.Add(builtinPath);
					}
				}
				catch (System.Exception ex)
				{
					// An exception occurred in Unity 2019.3.0b1.
					// Issue Tracker : https://issuetracker.unity3d.com/issues/scripting-missingmethodexception-errors-are-thrown-on-selecting-object-after-updating-the-api
					Debug.LogWarning("There is a problem with the Unity version referenced by the Assembly. : " + assembly.FullName);
					Debug.LogException(ex);
				}
			}
		}

		public static string GetBuiltinPath(System.Type classType)
		{
			int count = _Paths.Count;
			for (int i = 0; i < count; ++i)
			{
				BuiltinPath builtinPath = _Paths[i];
				if (classType == builtinPath.type || classType.IsSubclassOf(builtinPath.type))
				{
					return builtinPath.path;
				}
			}

			return "components/";
		}
	}
}