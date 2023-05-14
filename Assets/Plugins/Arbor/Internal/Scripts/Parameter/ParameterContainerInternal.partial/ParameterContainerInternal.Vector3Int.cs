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
		internal List<Vector3Int> _Vector3IntParameters = new List<Vector3Int>();

		#region Vector3Int

		private bool SetVector3Int(Parameter parameter, Vector3Int value)
		{
			return parameter != null && parameter.SetVector3Int(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector3Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector3Int(string name, Vector3Int value)
		{
			Parameter parameter = GetParam(name);
			return SetVector3Int(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector3Int type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector3Int(int id, Vector3Int value)
		{
			Parameter parameter = GetParam(id);
			return SetVector3Int(parameter, value);
		}

		private bool TryGetVector3Int(Parameter parameter, out Vector3Int value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVector3Int(out value);
			}

			value = Vector3Int.zero;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector3Int(string name, out Vector3Int value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVector3Int(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3Int type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector3Int(int id, out Vector3Int value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVector3Int(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector3Int GetVector3Int(string name, Vector3Int defaultValue)
		{
			Vector3Int value = Vector3Int.zero;
			if (TryGetVector3Int(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はVector3Int.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector3Int.zero.</returns>
#endif
		public Vector3Int GetVector3Int(string name)
		{
			return GetVector3Int(name, Vector3Int.zero);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3Int type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector3Int GetVector3Int(int id, Vector3Int defaultValue)
		{
			Vector3Int value = Vector3Int.zero;
			if (TryGetVector3Int(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はVector3Int.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3Int type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector3Int.zero.</returns>
#endif
		public Vector3Int GetVector3Int(int id)
		{
			return GetVector3Int(id, Vector3Int.zero);
		}

		#endregion //Vector3Int
	}
}