//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	using Arbor;
	using Arbor.Serialization;

	public static class VariableEditorUtility
	{
		internal class VariableTypeInfo
		{
			public System.Type variableType
			{
				get;
				private set;
			}

			public System.Type dataType
			{
				get;
				private set;
			}

			public bool isList
			{
				get;
				private set;
			}

			public System.Type valueType
			{
				get;
				private set;
			}

			public string menuName
			{
				get;
				private set;
			}

			public bool isSerializable
			{
				get;
				private set;
			}

			public VariableTypeInfo(System.Type variableType, System.Type dataType, string menuName)
			{
				this.variableType = variableType;
				this.dataType = dataType;
				this.menuName = menuName;

				this.isList = TypeUtility.IsGeneric(dataType, typeof(IList<>));
				this.valueType = this.isList ? TypeUtility.GetGenericArguments(dataType)[0] : dataType;
				this.isSerializable = SerializationUtility.IsSerializableFieldType(variableType);
			}
		}

		internal static List<VariableTypeInfo> s_VariableTypeInfos = new List<VariableTypeInfo>();
		internal static List<VariableTypeInfo> s_VariableListTypeInfos = new List<VariableTypeInfo>();

		static int CompareVariableTypeInfo(VariableTypeInfo a, VariableTypeInfo b)
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
		}

		static VariableEditorUtility()
		{
			var scriptTypes = ScriptsUtility.scriptTypes;
			for (int scriptIndex = 0; scriptIndex < scriptTypes.Count; scriptIndex++)
			{
				System.Type classType = scriptTypes[scriptIndex];
				if (classType == null)
				{
					continue;
				}

				if (classType.IsSubclassOf(typeof(VariableBase)))
				{
					System.Type dataType = VariableBase.GetDataType(classType);
					if (dataType == null)
					{
						continue;
					}

					string menuName = GetVariableMenu(classType);
					if (string.IsNullOrEmpty(menuName))
					{
						continue;
					}

					s_VariableTypeInfos.Add(new VariableTypeInfo(classType, dataType, menuName));
				}
				else if (classType.IsSubclassOf(typeof(VariableListBase)))
				{
					System.Type dataType = VariableListBase.GetDataType(classType);
					if (dataType == null)
					{
						continue;
					}

					string menuName = GetVariableListMenu(classType);
					if (string.IsNullOrEmpty(menuName))
					{
						continue;
					}

					s_VariableListTypeInfos.Add(new VariableTypeInfo(classType, dataType, menuName));
				}
			}

			s_VariableTypeInfos.Sort(CompareVariableTypeInfo);
			s_VariableListTypeInfos.Sort(CompareVariableTypeInfo);
		}

		public static bool IsVariableList(System.Type classType)
		{
			System.Type baseType = classType;
			while (baseType != null)
			{
				if (TypeUtility.IsGeneric(baseType, typeof(VariableList<>)))
				{
					return true;
				}
				baseType = baseType.BaseType;
			}

			return false;
		}

		public static string GetVariableName(System.Type classType)
		{
			System.Type dataType = VariableBase.GetDataType(classType);
			return TypeUtility.GetTypeName(dataType);
		}

		public static System.Type GetVariableTypeFromDataType(System.Type dataType)
		{
			for (int infoIndex = 0; infoIndex < s_VariableTypeInfos.Count; infoIndex++)
			{
				VariableTypeInfo variableInfo = s_VariableTypeInfos[infoIndex];
				if (variableInfo.dataType == dataType)
				{
					return variableInfo.variableType;
				}
			}

			return null;
		}

		public static string GetVariableMenu(System.Type classType)
		{
			AddVariableMenu addVariableMenu = AttributeHelper.GetAttribute<AddVariableMenu>(classType);

			string menuName = null;
			if (addVariableMenu != null)
			{
				menuName = addVariableMenu.menuName;
			}
			else
			{
				menuName = GetVariableName(classType);
			}

			if (string.IsNullOrEmpty(menuName))
			{
				return null;
			}

			return menuName;
		}

		public static void GenerateVariableMenus(GenericMenu genericMenu, System.Type currentDataType, bool inVariableGroup, System.Action<System.Type> onSelect)
		{
			for (int infoIndex = 0; infoIndex < s_VariableTypeInfos.Count; infoIndex++)
			{
				VariableTypeInfo variableInfo = s_VariableTypeInfos[infoIndex];
				string menuName = variableInfo.menuName;

				if (inVariableGroup)
				{
					menuName = "Variable/" + menuName;
				}

				if (variableInfo.isSerializable)
				{
					System.Type variableType = variableInfo.variableType;
					genericMenu.AddItem(GUIContentCaches.Get(menuName), variableInfo.dataType == currentDataType, () =>
					{
						onSelect(variableType);
					});
				}
				else
				{
					genericMenu.AddDisabledItem(GUIContentCaches.Get(menuName + " : not serializable"));
				}
			}
		}

		public static string GetVariableListName(System.Type classType)
		{
			System.Type dataType = VariableListBase.GetDataType(classType);

			System.Type valueType = TypeUtility.GetGenericArguments(dataType)[0];

			return TypeUtility.GetTypeName(valueType) + " List";
		}

		public static System.Type GetVariableListTypeFromDataType(System.Type dataType)
		{
			for (int infoIndex = 0; infoIndex < s_VariableListTypeInfos.Count; infoIndex++)
			{
				VariableTypeInfo variableInfo = s_VariableListTypeInfos[infoIndex];
				if (variableInfo.dataType == dataType)
				{
					return variableInfo.variableType;
				}
			}

			return null;
		}

		public static string GetVariableListMenu(System.Type classType)
		{
			AddVariableMenu addVariableMenu = AttributeHelper.GetAttribute<AddVariableMenu>(classType);

			string menuName = null;
			if (addVariableMenu != null)
			{
				menuName = addVariableMenu.menuName;
			}
			else
			{
				menuName = GetVariableListName(classType);
			}

			if (string.IsNullOrEmpty(menuName))
			{
				return null;
			}

			return menuName;
		}

		public static void GenerateVariableListMenus(GenericMenu genericMenu, System.Type currentDataType, bool inVariableGroup, System.Action<System.Type> onSelect)
		{
			for (int infoIndxe = 0; infoIndxe < s_VariableListTypeInfos.Count; infoIndxe++)
			{
				VariableTypeInfo variableInfo = s_VariableListTypeInfos[infoIndxe];
				string menuName = variableInfo.menuName;

				if (inVariableGroup)
				{
					menuName = "VariableList/" + menuName;
				}

				if (variableInfo.isSerializable)
				{
					System.Type variableType = variableInfo.variableType;
					genericMenu.AddItem(GUIContentCaches.Get(menuName), variableInfo.dataType == currentDataType, () =>
					{
						onSelect(variableType);
					});
				}
				else
				{
					genericMenu.AddDisabledItem(GUIContentCaches.Get(menuName + " : not serializable"));
				}
			}
		}
	}
}