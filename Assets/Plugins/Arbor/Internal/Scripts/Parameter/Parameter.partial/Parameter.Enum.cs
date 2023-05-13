//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// このパラメータがEnum型であるかどうかを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns whether this parameter is of type Enum.
		/// </summary>
#endif
		public bool isEnum
		{
			get
			{
				return type == Type.Enum && EnumFieldUtility.IsEnum(referenceType);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型のint値。
		/// </summary>
#else
		/// <summary>
		/// int Value of Enum type.
		/// </summary>
#endif
		public int enumIntValue
		{
			get
			{
				int value;
				if (TryGetEnumInt(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetEnumInt(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Enum type.
		/// </summary>
#endif
		public System.Enum enumValue
		{
			get
			{
				System.Enum value;
				if (TryGetEnum(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetEnum(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Enum

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Enum type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetEnumInt(int value)
		{
			if (type == Type.Enum)
			{
				Internal_SetInt(value);
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		/// <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <param name="value">The value you get.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetEnumInt(out int value)
		{
			if (type == Type.Enum)
			{
				value = Internal_GetInt();
				return true;
			}

			value = 0;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public int GetEnumInt(int defaultValue = default(int))
		{
			int value;
			if (TryGetEnumInt(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Enum type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetEnum(System.Enum value)
		{
			if (type == Type.Enum && value != null)
			{
				System.Type enumType = value.GetType();
				if (EnumFieldUtility.IsEnum(enumType))
				{
					if (referenceType != enumType)
					{
						referenceType = enumType;
					}
					Internal_SetInt(EnumFieldUtility.ToInt(value));
					return true;
				}
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		/// <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <param name="value">The value you get.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetEnum(out System.Enum value)
		{
			if (isEnum)
			{
				System.Type enumType = referenceType;
				value = EnumFieldUtility.ToEnum(enumType, Internal_GetInt());
				return true;
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public System.Enum GetEnum(System.Enum defaultValue = null)
		{
			System.Enum value;
			if (TryGetEnum(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enumの値を設定
		/// </summary>
		/// <typeparam name="TEnum">設定するenumの型</typeparam>
		/// <param name="value">値</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set Enum value
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to set</typeparam>
		/// <param name="value">Value</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetEnum<TEnum>(TEnum value) where TEnum : System.Enum
		{
			System.Type enumType = typeof(TEnum);

			if (type == Type.Enum && EnumFieldUtility.IsEnum(enumType))
			{
				if (referenceType != enumType)
				{
					referenceType = enumType;
				}
				Internal_SetInt(EnumFieldUtility.ToInt(value));

				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">取得するenumの型</typeparam>
		/// <param name="value">取得する値。</param>
		/// <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <param name="value">The value you get.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetEnum<TEnum>(out TEnum value) where TEnum : System.Enum
		{
			System.Type enumType = typeof(TEnum);

			if (type == Type.Enum && EnumFieldUtility.IsEnum(enumType))
			{
				value = (TEnum)EnumFieldUtility.ToEnum(enumType, Internal_GetInt());
				return true;
			}

			value = default(TEnum);
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">取得するenumの型</typeparam>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public TEnum GetEnum<TEnum>(TEnum defaultValue) where TEnum : System.Enum
		{
			TEnum value;
			if (TryGetEnum<TEnum>(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">取得するenumの型</typeparam>
		/// <returns>パラメータの値。パラメータがない場合はdefault(TEnum)を返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <returns>The value of the parameter. If there is no parameter, it returns default(TEnum).</returns>
#endif
		public TEnum GetEnum<TEnum>() where TEnum : System.Enum
		{
			return GetEnum(default(TEnum));
		}

		#endregion // Enum
	}
}