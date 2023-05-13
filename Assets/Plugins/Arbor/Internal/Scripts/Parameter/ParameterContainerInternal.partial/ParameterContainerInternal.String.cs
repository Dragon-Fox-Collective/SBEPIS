//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	using Internal;

	public partial class ParameterContainerInternal : ParameterContainerBase, ISerializationCallbackReceiver
	{
		[SerializeField]
		[HideInDocument]
		internal List<string> _StringParameters = new List<string>();

		#region String

		private bool SetString(Parameter parameter, string value)
		{
			return parameter != null && parameter.SetString(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// String型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the String type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetString(string name, string value)
		{
			Parameter parameter = GetParam(name);
			return SetString(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// String型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the String type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetString(int id, string value)
		{
			Parameter parameter = GetParam(id);
			return SetString(parameter, value);
		}

		private bool TryGetString(Parameter parameter, out string value)
		{
			if (parameter != null)
			{
				return parameter.TryGetString(out value);
			}

			value = string.Empty;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// String型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the String type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetString(string name, out string value)
		{
			Parameter parameter = GetParam(name);
			return TryGetString(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// String型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the String type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetString(int id, out string value)
		{
			Parameter parameter = GetParam(id);
			return TryGetString(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// String型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the String type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public string GetString(string name, string defaultValue = "")
		{
			string value = "";
			if (TryGetString(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// String型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the String type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public string GetString(int id, string defaultValue = "")
		{
			string value = "";
			if (TryGetString(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// String型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the String type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetString(string, out string)")]
		public bool GetString(string name, out string value)
		{
			return TryGetString(name, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// String型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the String type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetString(int, out string)")]
		public bool GetString(int id, out string value)
		{
			return TryGetString(id, out value);
		}

		#endregion // String
	}
}