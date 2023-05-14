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
		internal List<Vector3> _Vector3Parameters = new List<Vector3>();

		#region Vector3

		private bool SetVector3(Parameter parameter, Vector3 value)
		{
			return parameter != null && parameter.SetVector3(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector3 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector3(string name, Vector3 value)
		{
			Parameter parameter = GetParam(name);
			return SetVector3(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector3 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector3(int id, Vector3 value)
		{
			Parameter parameter = GetParam(id);
			return SetVector3(parameter, value);
		}

		private bool TryGetVector3(Parameter parameter, out Vector3 value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVector3(out value);
			}

			value = Vector3.zero;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector3(string name, out Vector3 value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVector3(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector3(int id, out Vector3 value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVector3(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector3 GetVector3(string name, Vector3 defaultValue)
		{
			Vector3 value = Vector3.zero;
			if (TryGetVector3(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はVector3.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector3.zero.</returns>
#endif
		public Vector3 GetVector3(string name)
		{
			return GetVector3(name, Vector3.zero);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector3 GetVector3(int id, Vector3 defaultValue)
		{
			Vector3 value = Vector3.zero;
			if (TryGetVector3(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はVector3.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector3.zero.</returns>
#endif
		public Vector3 GetVector3(int id)
		{
			return GetVector3(id, Vector3.zero);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetVector3(string, out Vector3)")]
		public bool GetVector3(string name, out Vector3 value)
		{
			return TryGetVector3(name, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetVector3(int, out Vector3)")]
		public bool GetVector3(int id, out Vector3 value)
		{
			return TryGetVector3(id, out value);
		}

		#endregion //Vector3
	}
}