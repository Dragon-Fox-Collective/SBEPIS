//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ArborEditor
{
	using Arbor;

	public static class AssemblyHelper
	{
		private static Dictionary<string, Type> s_Types = null;

		public static Type GetTypeByName(string name)
		{
			if (s_Types == null)
			{
				s_Types = new Dictionary<string, Type>();

				var assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int assemblyIndex = 0; assemblyIndex < assemblies.Length; assemblyIndex++)
				{
					Assembly assembly = assemblies[assemblyIndex];
					var types = TypeUtility.GetTypesFromAssembly(assembly);
					for (int typeIndex = 0; typeIndex < types.Length; typeIndex++)
					{
						Type t = types[typeIndex];
						s_Types[t.FullName] = t;
					}
				}
			}

			Type type = null;
			if (s_Types.TryGetValue(name, out type))
			{
				return type;
			}

			return null;
		}
	}
}