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
		internal List<Vector2Int> _Vector2IntParameters = new List<Vector2Int>();

		#region Vector2Int

		private bool SetVector2Int(Parameter parameter, Vector2Int value)
		{
			return parameter != null && parameter.SetVector2Int(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Int型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector2Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector2Int(string name, Vector2Int value)
		{
			Parameter parameter = GetParam(name);
			return SetVector2Int(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Int型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector2Int type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector2Int(int id, Vector2Int value)
		{
			Parameter parameter = GetParam(id);
			return SetVector2Int(parameter, value);
		}

		private bool TryGetVector2Int(Parameter parameter, out Vector2Int value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVector2Int(out value);
			}

			value = Vector2Int.zero;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Int型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector2Int(string name, out Vector2Int value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVector2Int(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Int型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2Int type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector2Int(int id, out Vector2Int value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVector2Int(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Int型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector2Int GetVector2Int(string name, Vector2Int defaultValue)
		{
			Vector2Int value = Vector2Int.zero;
			if (TryGetVector2Int(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Int型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はVector2Int.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector2Int.zero.</returns>
#endif
		public Vector2Int GetVector2Int(string name)
		{
			return GetVector2Int(name, Vector2Int.zero);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Int型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2Int type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector2Int GetVector2Int(int id, Vector2Int defaultValue)
		{
			Vector2Int value = Vector2Int.zero;
			if (TryGetVector2Int(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Int型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はVector2Int.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2Int type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector2Int.zero.</returns>
#endif
		public Vector2Int GetVector2Int(int id)
		{
			return GetVector2Int(id, Vector2Int.zero);
		}

		#endregion //Vector2Int
	}
}