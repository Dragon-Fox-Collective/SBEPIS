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
		internal List<Vector4ListParameter> _Vector4ListParameters = new List<Vector4ListParameter>();

		#region Vector4List

		private bool SetVector4List(Parameter parameter, IList<Vector4> value)
		{
			return parameter != null && parameter.SetVector4List(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4List型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector4List type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector4List(string name, IList<Vector4> value)
		{
			Parameter parameter = GetParam(name);
			return SetVector4List(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4List型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector4List type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector4List(int id, IList<Vector4> value)
		{
			Parameter parameter = GetParam(id);
			return SetVector4List(parameter, value);
		}

		private bool TryGetVector4List(Parameter parameter, out IList<Vector4> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVector4List(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4List型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4List type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector4List(string name, out IList<Vector4> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVector4List(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4List型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4List type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector4List(int id, out IList<Vector4> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVector4List(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4List型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4List type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Vector4> GetVector4List(int id)
		{
			IList<Vector4> value = null;
			if (TryGetVector4List(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4List型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4List type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Vector4> GetVector4List(string name)
		{
			IList<Vector4> value = null;
			if (TryGetVector4List(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // Vector4List
	}
}