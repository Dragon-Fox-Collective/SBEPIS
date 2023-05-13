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
		internal List<Vector3IntListParameter> _Vector3IntListParameters = new List<Vector3IntListParameter>();

		#region Vector3IntList

		private bool SetVector3IntList(Parameter parameter, IList<Vector3Int> value)
		{
			return parameter != null && parameter.SetVector3IntList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3IntList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector3IntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector3IntList(string name, IList<Vector3Int> value)
		{
			Parameter parameter = GetParam(name);
			return SetVector3IntList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3IntList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector3IntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector3IntList(int id, IList<Vector3Int> value)
		{
			Parameter parameter = GetParam(id);
			return SetVector3IntList(parameter, value);
		}

		private bool TryGetVector3IntList(Parameter parameter, out IList<Vector3Int> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVector3IntList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3IntList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3IntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector3IntList(string name, out IList<Vector3Int> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVector3IntList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3IntList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3IntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector3IntList(int id, out IList<Vector3Int> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVector3IntList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3IntList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3IntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Vector3Int> GetVector3IntList(int id)
		{
			IList<Vector3Int> value = null;
			if (TryGetVector3IntList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3IntList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3IntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Vector3Int> GetVector3IntList(string name)
		{
			IList<Vector3Int> value = null;
			if (TryGetVector3IntList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // Vector3IntList
	}
}