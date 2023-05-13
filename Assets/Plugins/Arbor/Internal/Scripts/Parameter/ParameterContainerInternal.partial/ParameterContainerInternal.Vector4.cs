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
		internal List<Vector4> _Vector4Parameters = new List<Vector4>();

		#region Vector4

		private bool SetVector4(Parameter parameter, Vector4 value)
		{
			return parameter != null && parameter.SetVector4(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector4 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector4(string name, Vector4 value)
		{
			Parameter parameter = GetParam(name);
			return SetVector4(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector4 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector4(int id, Vector4 value)
		{
			Parameter parameter = GetParam(id);
			return SetVector4(parameter, value);
		}

		private bool TryGetVector4(Parameter parameter, out Vector4 value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVector4(out value);
			}

			value = Vector4.zero;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector4(string name, out Vector4 value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVector4(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector4(int id, out Vector4 value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVector4(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector4 GetVector4(string name, Vector4 defaultValue)
		{
			Vector4 value = Vector4.zero;
			if (TryGetVector4(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はVector4.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4 type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector4.zero.</returns>
#endif
		public Vector4 GetVector4(string name)
		{
			return GetVector4(name, Vector4.zero);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector4 GetVector4(int id, Vector4 defaultValue)
		{
			Vector4 value = Vector4.zero;
			if (TryGetVector4(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はVector4.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4 type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector4.zero.</returns>
#endif
		public Vector4 GetVector4(int id)
		{
			return GetVector4(id, Vector4.zero);
		}

		#endregion //Vector4
	}
}