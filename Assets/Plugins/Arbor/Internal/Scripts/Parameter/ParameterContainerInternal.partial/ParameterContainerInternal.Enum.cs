//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public partial class ParameterContainerInternal : ParameterContainerBase, ISerializationCallbackReceiver
	{
		#region Enum

		private bool IsEnum(Parameter parameter)
		{
			return parameter != null && parameter.isEnum;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータがEnum型であるかどうかを返す。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>Enum型であればtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Enum type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>Returns true if it is an Enum type.</returns>
#endif
		public bool IsEnum(int id)
		{
			Parameter parameter = GetParam(id);
			return IsEnum(parameter);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータがEnum型であるかどうかを返す。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>Enum型であればtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Enum type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Returns true if it is an Enum type.</returns>
#endif
		public bool IsEnum(string name)
		{
			Parameter parameter = GetParam(name);
			return IsEnum(parameter);
		}

		private bool SetEnumInt(Parameter parameter, int value)
		{
			return parameter != null && parameter.SetEnumInt(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Enum type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetEnumInt(string name, int value)
		{
			Parameter parameter = GetParam(name);
			return SetEnumInt(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Enum type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetEnumInt(int id, int value)
		{
			Parameter parameter = GetParam(id);
			return SetEnumInt(parameter, value);
		}

		private bool TryGetEnumInt(Parameter parameter, out int value)
		{
			if (parameter != null)
			{
				return parameter.TryGetEnumInt(out value);
			}

			value = 0;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetEnumInt(string name, out int value)
		{
			Parameter parameter = GetParam(name);
			return TryGetEnumInt(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetEnumInt(int id, out int value)
		{
			Parameter parameter = GetParam(id);
			return TryGetEnumInt(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public int GetEnumInt(string name, int defaultValue = default(int))
		{
			int value;
			if (TryGetEnumInt(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public int GetEnumInt(int id, int defaultValue = default(int))
		{
			int value;
			if (TryGetEnumInt(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

		private bool SetEnum(Parameter parameter, System.Enum value)
		{
			return parameter != null && parameter.SetEnum(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Enum type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetEnum(string name, System.Enum value)
		{
			Parameter parameter = GetParam(name);
			return SetEnum(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Enum type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetEnum(int id, System.Enum value)
		{
			Parameter parameter = GetParam(id);
			return SetEnum(parameter, value);
		}

		private bool TryGetEnum(Parameter parameter, out System.Enum value)
		{
			if (parameter != null)
			{
				return parameter.TryGetEnum(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetEnum(string name, out System.Enum value)
		{
			Parameter parameter = GetParam(name);
			return TryGetEnum(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetEnum(int id, out System.Enum value)
		{
			Parameter parameter = GetParam(id);
			return TryGetEnum(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public System.Enum GetEnum(string name, System.Enum defaultValue = null)
		{
			System.Enum value = null;
			if (TryGetEnum(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public System.Enum GetEnum(int id, System.Enum defaultValue = null)
		{
			System.Enum value = null;
			if (TryGetEnum(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

		private bool SetEnum<TEnum>(Parameter parameter, TEnum value) where TEnum : System.Enum
		{
			return parameter != null && parameter.SetEnum<TEnum>(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を設定する。
		/// </summary>
		/// <typeparam name="TEnum">設定するenumの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to set</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetEnum<TEnum>(string name, TEnum value) where TEnum : System.Enum
		{
			Parameter parameter = GetParam(name);
			return SetEnum(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を設定する。
		/// </summary>
		/// <typeparam name="TEnum">設定するenumの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to set</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetEnum<TEnum>(int id, TEnum value) where TEnum : System.Enum
		{
			Parameter parameter = GetParam(id);
			return SetEnum(parameter, value);
		}

		private bool TryGetEnum<TEnum>(Parameter parameter, out TEnum value) where TEnum : System.Enum
		{
			if (parameter != null)
			{
				return parameter.TryGetEnum<TEnum>(out value);
			}

			value = default(TEnum);
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">取得するenumの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetEnum<TEnum>(string name, out TEnum value) where TEnum : System.Enum
		{
			Parameter parameter = GetParam(name);
			return TryGetEnum(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">取得するenumの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetEnum<TEnum>(int id, out TEnum value) where TEnum : System.Enum
		{
			Parameter parameter = GetParam(id);
			return TryGetEnum(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">取得するenumの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public TEnum GetEnum<TEnum>(string name, TEnum defaultValue) where TEnum : System.Enum
		{
			TEnum value;
			if (TryGetEnum<TEnum>(name, out value))
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
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefault(TEnum)を返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns default(TEnum).</returns>
#endif
		public TEnum GetEnum<TEnum>(string name) where TEnum : System.Enum
		{
			return GetEnum(name, default(TEnum));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">取得するenumの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public TEnum GetEnum<TEnum>(int id, TEnum defaultValue) where TEnum : System.Enum
		{
			TEnum value;
			if (TryGetEnum(id, out value))
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
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefault(TEnum)を返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns default(TEnum).</returns>
#endif
		public TEnum GetEnum<TEnum>(int id) where TEnum : System.Enum
		{
			return GetEnum(id, default(TEnum));
		}

		#endregion //Enum
	}
}