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
		internal List<BoolListParameter> _BoolListParameters = new List<BoolListParameter>();

		#region BoolList

		private bool SetBoolList(Parameter parameter, IList<bool> value)
		{
			return parameter != null && parameter.SetBoolList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoolList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the BoolList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetBoolList(string name, IList<bool> value)
		{
			Parameter parameter = GetParam(name);
			return SetBoolList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoolList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the BoolList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetBoolList(int id, IList<bool> value)
		{
			Parameter parameter = GetParam(id);
			return SetBoolList(parameter, value);
		}

		private bool TryGetBoolList(Parameter parameter, out IList<bool> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetBoolList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoolList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the BoolList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetBoolList(string name, out IList<bool> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetBoolList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoolList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the BoolList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetBoolList(int id, out IList<bool> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetBoolList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// BoolList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoolList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<bool> GetBoolList(int id)
		{
			IList<bool> value = null;
			if (TryGetBoolList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoolList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoolList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<bool> GetBoolList(string name)
		{
			IList<bool> value = null;
			if (TryGetBoolList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // BoolList
	}
}