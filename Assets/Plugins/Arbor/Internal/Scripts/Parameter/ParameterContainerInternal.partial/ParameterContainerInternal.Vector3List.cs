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
		internal List<Vector3ListParameter> _Vector3ListParameters = new List<Vector3ListParameter>();

		#region Vector3List

		private bool SetVector3List(Parameter parameter, IList<Vector3> value)
		{
			return parameter != null && parameter.SetVector3List(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3List型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector3List type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector3List(string name, IList<Vector3> value)
		{
			Parameter parameter = GetParam(name);
			return SetVector3List(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3List型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector3List type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector3List(int id, IList<Vector3> value)
		{
			Parameter parameter = GetParam(id);
			return SetVector3List(parameter, value);
		}

		private bool TryGetVector3List(Parameter parameter, out IList<Vector3> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVector3List(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3List型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3List type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector3List(string name, out IList<Vector3> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVector3List(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3List型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3List type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector3List(int id, out IList<Vector3> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVector3List(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3List型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3List type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Vector3> GetVector3List(int id)
		{
			IList<Vector3> value = null;
			if (TryGetVector3List(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3List型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3List type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Vector3> GetVector3List(string name)
		{
			IList<Vector3> value = null;
			if (TryGetVector3List(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // Vector3List
	}
}