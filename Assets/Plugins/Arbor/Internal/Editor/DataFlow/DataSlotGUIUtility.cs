//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;

namespace ArborEditor
{
	using Arbor;

	internal static class DataSlotGUIUtility
	{
		public static bool IsList(System.Type type)
		{
			return IListInfo.GetIList(type) != null;
		}

		public static System.Type ElementType(System.Type type)
		{
			System.Type listType = IListInfo.GetIList(type);
			if (listType == null)
			{
				return type;
			}

			if (TypeUtility.IsGeneric(listType, typeof(IList<>)))
			{
				return TypeUtility.GetGenericArguments(listType)[0];
			}
			else
			{
				return typeof(object);
			}
		}

		private sealed class IListInfo
		{
			private readonly System.Type interfaceIListOfT;
			private readonly System.Type interfaceIList;

			private IListInfo(System.Type type)
			{
				if (TypeUtility.IsGeneric(type, typeof(IList<>)))
				{
					interfaceIListOfT = type;
					return;
				}

				if (type == typeof(IList))
				{
					interfaceIList = type;
					return;
				}

				System.Type[] interfaces = type.GetInterfaces();
				if (interfaces != null)
				{
					for (int i = 0; i < interfaces.Length; i++)
					{
						var inter = interfaces[i];
						if (interfaceIListOfT == null)
						{
							System.Type interfaceIListOfT = Internal_GetIListOfT(inter);
							if (interfaceIListOfT != null)
							{
								this.interfaceIListOfT = interfaceIListOfT;
							}
						}

						if (interfaceIList == null)
						{
							System.Type interfaceIList = Internal_GetIList(inter);
							if (interfaceIList != null)
							{
								this.interfaceIList = interfaceIList;
							}
						}

						if (interfaceIListOfT != null && interfaceIList != null)
						{
							break;
						}
					}
				}

				if (interfaceIListOfT == null || interfaceIList == null)
				{
					if (type.BaseType != null)
					{
						var baseInfo = FindOrCreate(type.BaseType);
						if (interfaceIListOfT == null)
						{
							interfaceIListOfT = baseInfo.interfaceIListOfT;
						}
						if (interfaceIList == null)
						{
							interfaceIList = baseInfo.interfaceIList;
						}
					}
				}
			}

			private static Dictionary<System.Type, IListInfo> s_TypeInfos = new Dictionary<System.Type, IListInfo>();

			static IListInfo FindOrCreate(System.Type type)
			{
				if (!s_TypeInfos.TryGetValue(type, out var typeInfo))
				{
					typeInfo = new IListInfo(type);
					s_TypeInfos.Add(type, typeInfo);
				}
				return typeInfo;
			}

			static System.Type Internal_GetIListOfT(System.Type type)
			{
				if (TypeUtility.IsGeneric(type, typeof(IList<>)))
				{
					return type;
				}

				var typeInfo = FindOrCreate(type);
				return typeInfo.interfaceIListOfT;
			}

			static System.Type Internal_GetIList(System.Type type)
			{
				if (type == typeof(IList))
				{
					return type;
				}

				var typeInfo = FindOrCreate(type);
				return typeInfo.interfaceIList;
			}

			public static System.Type GetIList(System.Type type)
			{
				var typeInfo = FindOrCreate(type);
				if (typeInfo.interfaceIListOfT != null)
				{
					return typeInfo.interfaceIListOfT;
				}
				return typeInfo.interfaceIList;
			}
		}
	}
}