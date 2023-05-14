//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ArborEditor
{
	internal static class ScriptsUtility
	{
		public readonly static ReadOnlyCollection<MonoScript> scripts;
		public readonly static ReadOnlyCollection<System.Type> scriptTypes;

		static ScriptsUtility()
		{
			var monoScripts = MonoImporter.GetAllRuntimeMonoScripts();
			if (monoScripts != null)
			{
				int scriptCount = monoScripts.Length;

				List<MonoScript> _scripts = new List<MonoScript>(scriptCount);
				HashSet<System.Type> _types = new HashSet<System.Type>();

				for (int i = 0; i < scriptCount; i++)
				{
					MonoScript script = monoScripts[i];
					if (script == null || script.hideFlags != 0)
					{
						continue;
					}

					System.Type classType = script.GetClass();
					if (classType == null)
					{
						continue;
					}

					_scripts.Add(script);
					_types.Add(classType);
				}

				scripts = _scripts.AsReadOnly();
				scriptTypes = new ReadOnlyCollection<System.Type>(_types.ToArray());
			}
			else
			{
				scripts = new ReadOnlyCollection<MonoScript>(new MonoScript[] { });
				scriptTypes = new ReadOnlyCollection<System.Type>(new System.Type[] { });
			}
		}
	}
}