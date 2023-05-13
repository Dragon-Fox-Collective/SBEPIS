//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	using Arbor;

	public struct ParameterTypeMenuItem
	{
		public readonly static ParameterTypeMenuItem[] menuItems;
		private readonly static GUIContent[] s_DisplayOptions;

		static ParameterTypeMenuItem()
		{
			List<ParameterTypeMenuItem> menuItems = new List<ParameterTypeMenuItem>();

			Parameter.Type[] parameterTypes = EnumUtility.GetValues<Parameter.Type>();

			var typeParameterType = typeof(Parameter.Type);
			foreach (var parameterType in parameterTypes)
			{
				string menuName = ParameterUtility.GetMenuName(parameterType);

				menuItems.Add(new ParameterTypeMenuItem() { menuName = menuName, type = parameterType });
			}

			menuItems.Sort((a, b) =>
			{
				string[] aNames = a.menuName.Split('/');
				string[] bNames = b.menuName.Split('/');
				int i = 0;

				while (i < aNames.Length && i < bNames.Length)
				{
					int compare = 0;
					if (i + 1 >= aNames.Length || i + 1 >= bNames.Length)
					{
						compare = bNames.Length - aNames.Length;
					}
					if (compare == 0)
					{
						compare = aNames[i].CompareTo(bNames[i]);
					}
					if (compare != 0)
					{
						return compare;
					}
					i++;
				}

				return bNames.Length - aNames.Length;
			});

			s_DisplayOptions = new GUIContent[menuItems.Count];
			for (int i = 0; i < menuItems.Count; i++)
			{
				s_DisplayOptions[i] = new GUIContent(menuItems[i].menuName);
			}

			ParameterTypeMenuItem.menuItems = menuItems.ToArray();
		}

		public static int GetIndex(Parameter.Type parameterType)
		{
			int selectedIndex = -1;
			for (int i = 0; i < menuItems.Length; i++)
			{
				var menuItem = menuItems[i];
				if (menuItem.type == parameterType)
				{
					selectedIndex = i;
					break;
				}
			}

			return selectedIndex;
		}

		public static Parameter.Type Popup(Rect rect, GUIContent label, Parameter.Type parameterType)
		{
			int selectedIndex = GetIndex(parameterType);

			EditorGUI.BeginChangeCheck();
			selectedIndex = EditorGUI.Popup(rect, label, selectedIndex, s_DisplayOptions);
			if (EditorGUI.EndChangeCheck() && selectedIndex >= 0)
			{
				parameterType = menuItems[selectedIndex].type;
			}

			return parameterType;
		}

		public Parameter.Type type;
		public string menuName;
	}
}