//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if !(ENABLE_IL2CPP || NET_STANDARD_2_0 || NETFX_CORE || ARBOR_DLL)
#define ENABLE_DELEGATE_CALL
#endif

using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

#if ENABLE_DELEGATE_CALL
using System.Reflection.Emit;
#endif

namespace ArborEditor
{
	public static class EnumUtility
	{
		private static class EnumInfo<TEnum> where TEnum : Enum
		{
			public static readonly TEnum[] values;
			public static readonly GUIContent[] contents;

			private static readonly IEqualityComparer<TEnum> s_Comparer;

#if ENABLE_DELEGATE_CALL
			private sealed class EnumComparer : IEqualityComparer<TEnum>
			{
				private delegate bool EqualsFunc(TEnum x, TEnum y);
				private delegate int GetHashCodeFunc(TEnum obj);

				private static readonly EqualsFunc s_EqualsFunc;
				private static readonly GetHashCodeFunc s_HashCodeFunc;

				static EnumComparer()
				{
					s_EqualsFunc = CreateEqualsFunc();
					s_HashCodeFunc = CreateGetHashCodeFunc();
				}

				static EqualsFunc CreateEqualsFunc()
				{
					DynamicMethod dm = new DynamicMethod("", typeof(bool), new[] { typeof(TEnum), typeof(TEnum) }, true);

					ILGenerator generator = dm.GetILGenerator();

					generator.Emit(OpCodes.Ldarg_0);
					generator.Emit(OpCodes.Ldarg_1);
					generator.Emit(OpCodes.Ceq);
					generator.Emit(OpCodes.Ret);

					return (EqualsFunc)dm.CreateDelegate(typeof(EqualsFunc));
				}

				static GetHashCodeFunc CreateGetHashCodeFunc()
				{
					DynamicMethod dm = new DynamicMethod("", typeof(int), new[] { typeof(TEnum) }, true);

					ILGenerator generator = dm.GetILGenerator();

					generator.Emit(OpCodes.Ldarg_0);
					generator.Emit(OpCodes.Ret);

					return (GetHashCodeFunc)dm.CreateDelegate(typeof(GetHashCodeFunc));
				}

				public bool Equals(TEnum x, TEnum y)
				{
					return s_EqualsFunc(x, y);
				}

				public int GetHashCode(TEnum obj)
				{
					return s_HashCodeFunc(obj);
				}
			}
#endif

			static EnumInfo()
			{
				Type enumType = typeof(TEnum);
				if (!enumType.IsEnum)
				{
					throw new ArgumentException("The type `" + enumType.Name + "' must be convertible to `enum' in order to use it as parameter `TEnum'", "TEnum");
				}

				List<FieldInfo> enumFields = EnumUtility.GetFields(enumType);

				values = enumFields.Select(f => (TEnum)Enum.Parse(enumType, f.Name)).ToArray();
				contents = enumFields.Select(f => GUIContentCaches.Get(f.Name)).ToArray();

#if ENABLE_DELEGATE_CALL
				try
				{
					s_Comparer = new EnumComparer();
				}
				catch
				{
					s_Comparer = EqualityComparer<TEnum>.Default;
				}
#else
				s_Comparer = EqualityComparer<TEnum>.Default;
#endif
			}

			public static int IndexOf(TEnum value)
			{
				for (int i = 0; i < values.Length; ++i)
				{
					if (s_Comparer.Equals(values[i], value))
					{
						return i;
					}
				}
				return -1;
			}
		}

		private static Dictionary<Type, List<FieldInfo>> s_FieldInfos = new Dictionary<Type, List<FieldInfo>>();

		internal static List<FieldInfo> GetFields(Type enumType)
		{
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("The type `" + enumType.Name + "' must be convertible to `enum' in order to use it as parameter `TEnum'", "TEnum");
			}

			List<FieldInfo> fields = null;
			if (!s_FieldInfos.TryGetValue(enumType, out fields))
			{
				fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public)
						.Where(f => !Arbor.AttributeHelper.HasAttribute<ObsoleteAttribute>(f))
						.OrderBy(f => f.MetadataToken).ToList();
				s_FieldInfos.Add(enumType, fields);
			}

			return fields;
		}

		public static TEnum[] GetValues<TEnum>() where TEnum : Enum
		{
			return EnumInfo<TEnum>.values;
		}

		public static GUIContent[] GetContents<TEnum>() where TEnum : Enum
		{
			return EnumInfo<TEnum>.contents;
		}

		public static TEnum GetValueFromIndex<TEnum>(int index) where TEnum : Enum
		{
			if (index < 0)
			{
				return default(TEnum);
			}
			return EnumInfo<TEnum>.values[index];
		}

		public static int GetIndexFromValue<TEnum>(TEnum value) where TEnum : Enum
		{
			return EnumInfo<TEnum>.IndexOf(value);
		}

		public static GUIContent GetContentFromValue<TEnum>(TEnum value) where TEnum : Enum
		{
			int index = GetIndexFromValue(value);
			return GetContents<TEnum>()[index];
		}

		public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : Enum
		{
			try
			{
				result = (TEnum)Enum.Parse(typeof(TEnum), value);
				return true;
			}
			catch
			{
				result = default(TEnum);
				return false;
			}
		}
	}
}
