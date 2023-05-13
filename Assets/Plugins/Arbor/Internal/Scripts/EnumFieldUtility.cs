//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// FlexibleEnumAnyやenum型Parameterのユーティリティクラス
	/// </summary>
#else
	/// <summary>
	/// Utility class of FlexibleEnumAny or enum type Parameter
	/// </summary>
#endif
	public static class EnumFieldUtility
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// enum型チェック
		/// </summary>
		/// <param name="enumType">enum型</param>
		/// <returns>enum型であればtrueを返す</returns>
#else
		/// <summary>
		/// Enum type check
		/// </summary>
		/// <param name="enumType">enum type</param>
		/// <returns>Return true if it is an enum type</returns>
#endif
		public static bool IsEnum(Type enumType)
		{
			return enumType != null && TypeUtility.IsEnum(enumType) && Enum.GetUnderlyingType(enumType) == typeof(int);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// System.FlagsAttributeを持つenum型チェック
		/// </summary>
		/// <param name="enumType">enum型</param>
		/// <returns>System.FlagsAttributeを持つenum型であればtrueを返す。</returns>
#else
		/// <summary>
		/// Enum type check with System.FlagsAttribute
		/// </summary>
		/// <param name="enumType">enum type</param>
		/// <returns>Returns true if it is an enum type with System.FlagsAttribute.</returns>
#endif
		public static bool IsEnumFlags(Type enumType)
		{
			return IsEnum(enumType) && AttributeHelper.HasAttribute<FlagsAttribute>(enumType);
		}

		sealed class EnumInfo
		{
			public Dictionary<int, Enum> intToEnum = new Dictionary<int, Enum>();
			public Dictionary<Enum, int> enumToInt = new Dictionary<Enum, int>();

			public Enum ToEnum(Type enumType, int value)
			{
				if (!intToEnum.TryGetValue(value, out var enumValue))
				{
					enumValue = (Enum)Enum.ToObject(enumType, value);
					intToEnum.Add(value, enumValue);
				}

				return enumValue;
			}

			public int ToInt(Enum value)
			{
				if (!enumToInt.TryGetValue(value, out var intValue))
				{
					intValue = Convert.ToInt32(value);
					enumToInt.Add(value, intValue);
				}

				return intValue;
			}
		}

		static Dictionary<Type, EnumInfo> s_EnumInfos = new Dictionary<Type, EnumInfo>();

#if ARBOR_DOC_JA
		/// <summary>
		/// int型をenum型に変換する。
		/// </summary>
		/// <param name="enumType">変換後のenum型</param>
		/// <param name="value">変換したいint型の値</param>
		/// <returns>変換したenum型の値。変換できなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Convert int type to enum type.
		/// </summary>
		/// <param name="enumType">Converted enum type</param>
		/// <param name="value">Int type value you want to convert</param>
		/// <returns>Converted enum type value. If conversion is not possible, null is returned.</returns>
#endif
		public static Enum ToEnum(Type enumType, int value)
		{
			if (!IsEnum(enumType))
			{
				return null;
			}

			if (!s_EnumInfos.TryGetValue(enumType, out var enumInfo))
			{
				enumInfo = new EnumInfo();
				s_EnumInfos.Add(enumType, enumInfo);
			}

			return enumInfo.ToEnum(enumType, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// enum型をint型に変換する。
		/// </summary>
		/// <param name="value">変換したいenum型の値</param>
		/// <returns>変換したint型の値</returns>
#else
		/// <summary>
		/// Convert enum type to int type.
		/// </summary>
		/// <param name="value">Enum type value you want to convert</param>
		/// <returns>Converted int type value</returns>
#endif
		public static int ToInt(Enum value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			var enumType = value.GetType();

			if (!s_EnumInfos.TryGetValue(enumType, out var enumInfo))
			{
				enumInfo = new EnumInfo();
				s_EnumInfos.Add(enumType, enumInfo);
			}

			return enumInfo.ToInt(value);
		}

		sealed class EnumInfo<T> where T : Enum
		{
			private static Dictionary<int, T> intToEnum = new Dictionary<int, T>();
			private static Dictionary<T, int> enumToInt = new Dictionary<T, int>();

			public static T ToEnum(int value)
			{
				if (!intToEnum.TryGetValue(value, out var enumValue))
				{
					enumValue = (T)Enum.ToObject(typeof(T), value);
					intToEnum.Add(value, enumValue);
				}

				return enumValue;
			}

			public static int ToInt(T value)
			{
				if (!enumToInt.TryGetValue(value, out var intValue))
				{
					intValue = Convert.ToInt32(value);
					enumToInt.Add(value, intValue);
				}

				return intValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// int型をenum型に変換する。
		/// </summary>
		/// <typeparam name="T">変換後のenum型</typeparam>
		/// <param name="value">変換したいint型の値</param>
		/// <returns>変換したenum型の値</returns>
#else
		/// <summary>
		/// Convert int type to enum type.
		/// </summary>
		/// <typeparam name="T">Converted enum type</typeparam>
		/// <param name="value">Int type value you want to convert</param>
		/// <returns>Converted enum type value.</returns>
#endif
		public static T ToEnum<T>(int value) where T : Enum
		{
			return EnumInfo<T>.ToEnum(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// enum型をint型に変換する。
		/// </summary>
		/// <typeparam name="T">変換するenum型</typeparam>
		/// <param name="value">変換したいenum型の値</param>
		/// <returns>変換したint型の値</returns>
#else
		/// <summary>
		/// Convert enum type to int type.
		/// </summary>
		/// <typeparam name="T">Enum type to convert</typeparam>
		/// <param name="value">Enum type value you want to convert</param>
		/// <returns>Converted int type value</returns>
#endif
		public static int ToInt<T>(T value) where T : Enum
		{
			return EnumInfo<T>.ToInt(value);
		}
	}
}