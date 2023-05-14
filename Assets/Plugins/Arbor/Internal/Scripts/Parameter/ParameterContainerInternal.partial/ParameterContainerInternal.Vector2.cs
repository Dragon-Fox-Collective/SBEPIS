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
		internal List<Vector2> _Vector2Parameters = new List<Vector2>();

		#region Vector2

		private bool SetVector2(Parameter parameter, Vector2 value)
		{
			return parameter != null && parameter.SetVector2(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector2 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector2(string name, Vector2 value)
		{
			Parameter parameter = GetParam(name);
			return SetVector2(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector2 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector2(int id, Vector2 value)
		{
			Parameter parameter = GetParam(id);
			return SetVector2(parameter, value);
		}

		private bool TryGetVector2(Parameter parameter, out Vector2 value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVector2(out value);
			}

			value = Vector2.zero;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector2(string name, out Vector2 value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVector2(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector2(int id, out Vector2 value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVector2(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector2 GetVector2(string name, Vector2 defaultValue)
		{
			Vector2 value = Vector2.zero;
			if (TryGetVector2(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はVector2.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector2.zero.</returns>
#endif
		public Vector2 GetVector2(string name)
		{
			return GetVector2(name, Vector2.zero);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector2 GetVector2(int id, Vector2 defaultValue)
		{
			Vector2 value = Vector2.zero;
			if (TryGetVector2(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はVector2.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector2.zero.</returns>
#endif
		public Vector2 GetVector2(int id)
		{
			return GetVector2(id, Vector2.zero);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetVector2(string, out Vector2)")]
		public bool GetVector2(string name, out Vector2 value)
		{
			return TryGetVector2(name, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetVector2(int, out Vector2)")]
		public bool GetVector2(int id, out Vector2 value)
		{
			return TryGetVector2(id, out value);
		}

		#endregion //Vector2
	}
}