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
		internal List<int> _IntParameters = new List<int>();

		#region Int

		private bool SetInt(Parameter parameter, int value)
		{
			return parameter != null && parameter.SetInt(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetInt(string name, int value)
		{
			Parameter parameter = GetParam(name);
			return SetInt(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Int type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetInt(int id, int value)
		{
			Parameter parameter = GetParam(id);
			return SetInt(parameter, value);
		}

		private bool TryGetInt(Parameter parameter, out int value)
		{
			if (parameter != null)
			{
				return parameter.TryGetInt(out value);
			}

			value = 0;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetInt(string name, out int value)
		{
			Parameter parameter = GetParam(name);
			return TryGetInt(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Int type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetInt(int id, out int value)
		{
			Parameter parameter = GetParam(id);
			return TryGetInt(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Int type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public int GetInt(int id, int defaultValue = 0)
		{
			int value = 0;
			if (TryGetInt(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public int GetInt(string name, int defaultValue = 0)
		{
			int value = 0;
			if (TryGetInt(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetInt(string, out int)")]
		public bool GetInt(string name, out int value)
		{
			return TryGetInt(name, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Int type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetInt(int, out int)")]
		public bool GetInt(int id, out int value)
		{
			return TryGetInt(id, out value);
		}

		#endregion // Int
	}
}