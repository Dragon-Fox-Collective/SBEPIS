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
		internal List<RectIntListParameter> _RectIntListParameters = new List<RectIntListParameter>();

		#region RectIntList

		private bool SetRectIntList(Parameter parameter, IList<RectInt> value)
		{
			return parameter != null && parameter.SetRectIntList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectIntList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the RectIntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRectIntList(string name, IList<RectInt> value)
		{
			Parameter parameter = GetParam(name);
			return SetRectIntList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectIntList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the RectIntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRectIntList(int id, IList<RectInt> value)
		{
			Parameter parameter = GetParam(id);
			return SetRectIntList(parameter, value);
		}

		private bool TryGetRectIntList(Parameter parameter, out IList<RectInt> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetRectIntList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectIntList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the RectIntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRectIntList(string name, out IList<RectInt> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetRectIntList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectIntList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the RectIntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRectIntList(int id, out IList<RectInt> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetRectIntList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// RectIntList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectIntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<RectInt> GetRectIntList(int id)
		{
			IList<RectInt> value = null;
			if (TryGetRectIntList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectIntList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectIntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<RectInt> GetRectIntList(string name)
		{
			IList<RectInt> value = null;
			if (TryGetRectIntList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // RectIntList
	}
}