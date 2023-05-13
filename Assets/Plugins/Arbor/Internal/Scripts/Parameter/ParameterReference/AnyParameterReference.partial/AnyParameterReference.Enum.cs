//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	public sealed partial class AnyParameterReference
	{
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
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetEnumInt();
				}

				return 0;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetEnumInt(value);
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
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetEnum();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetEnum(value);
				}
			}
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
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.SetEnum<TEnum>(value);
			}

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
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.GetEnum<TEnum>(defaultValue);
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
			return GetEnum<TEnum>(default(TEnum));
		}
	}
}