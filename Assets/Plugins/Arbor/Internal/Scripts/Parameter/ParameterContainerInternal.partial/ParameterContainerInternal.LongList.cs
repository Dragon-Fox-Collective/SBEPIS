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
		internal List<LongListParameter> _LongListParameters = new List<LongListParameter>();

		#region LongList

		private bool SetLongList(Parameter parameter, IList<long> value)
		{
			return parameter != null && parameter.SetLongList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LongList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the LongList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetLongList(string name, IList<long> value)
		{
			Parameter parameter = GetParam(name);
			return SetLongList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LongList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the LongList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetLongList(int id, IList<long> value)
		{
			Parameter parameter = GetParam(id);
			return SetLongList(parameter, value);
		}

		private bool TryGetLongList(Parameter parameter, out IList<long> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetLongList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LongList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the LongList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetLongList(string name, out IList<long> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetLongList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LongList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the LongList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetLongList(int id, out IList<long> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetLongList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// LongList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the LongList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<long> GetLongList(int id)
		{
			IList<long> value = null;
			if (TryGetLongList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LongList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the LongList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<long> GetLongList(string name)
		{
			IList<long> value = null;
			if (TryGetLongList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // LongList
	}
}