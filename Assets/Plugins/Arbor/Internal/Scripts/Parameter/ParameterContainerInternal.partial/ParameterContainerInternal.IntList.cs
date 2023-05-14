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
		internal List<IntListParameter> _IntListParameters = new List<IntListParameter>();

		#region IntList

		private bool SetIntList(Parameter parameter, IList<int> value)
		{
			return parameter != null && parameter.SetIntList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IntList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the IntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetIntList(string name, IList<int> value)
		{
			Parameter parameter = GetParam(name);
			return SetIntList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IntList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the IntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetIntList(int id, IList<int> value)
		{
			Parameter parameter = GetParam(id);
			return SetIntList(parameter, value);
		}

		private bool TryGetIntList(Parameter parameter, out IList<int> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetIntList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IntList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the IntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetIntList(string name, out IList<int> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetIntList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IntList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the IntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetIntList(int id, out IList<int> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetIntList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// IntList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the IntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<int> GetIntList(int id)
		{
			IList<int> value = null;
			if (TryGetIntList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IntList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the IntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<int> GetIntList(string name)
		{
			IList<int> value = null;
			if (TryGetIntList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // IntList
	}
}