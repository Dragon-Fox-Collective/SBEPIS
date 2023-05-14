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
		internal List<BoundsIntListParameter> _BoundsIntListParameters = new List<BoundsIntListParameter>();

		#region BoundsIntList

		private bool SetBoundsIntList(Parameter parameter, IList<BoundsInt> value)
		{
			return parameter != null && parameter.SetBoundsIntList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the BoundsIntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetBoundsIntList(string name, IList<BoundsInt> value)
		{
			Parameter parameter = GetParam(name);
			return SetBoundsIntList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the BoundsIntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetBoundsIntList(int id, IList<BoundsInt> value)
		{
			Parameter parameter = GetParam(id);
			return SetBoundsIntList(parameter, value);
		}

		private bool TryGetBoundsIntList(Parameter parameter, out IList<BoundsInt> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetBoundsIntList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsIntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetBoundsIntList(string name, out IList<BoundsInt> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetBoundsIntList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsIntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetBoundsIntList(int id, out IList<BoundsInt> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetBoundsIntList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsIntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<BoundsInt> GetBoundsIntList(int id)
		{
			IList<BoundsInt> value = null;
			if (TryGetBoundsIntList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsIntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<BoundsInt> GetBoundsIntList(string name)
		{
			IList<BoundsInt> value = null;
			if (TryGetBoundsIntList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // BoundsIntList
	}
}