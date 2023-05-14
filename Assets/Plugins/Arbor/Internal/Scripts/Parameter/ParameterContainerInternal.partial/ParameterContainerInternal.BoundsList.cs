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
		internal List<BoundsListParameter> _BoundsListParameters = new List<BoundsListParameter>();

		#region BoundsList

		private bool SetBoundsList(Parameter parameter, IList<Bounds> value)
		{
			return parameter != null && parameter.SetBoundsList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the BoundsList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetBoundsList(string name, IList<Bounds> value)
		{
			Parameter parameter = GetParam(name);
			return SetBoundsList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the BoundsList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetBoundsList(int id, IList<Bounds> value)
		{
			Parameter parameter = GetParam(id);
			return SetBoundsList(parameter, value);
		}

		private bool TryGetBoundsList(Parameter parameter, out IList<Bounds> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetBoundsList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetBoundsList(string name, out IList<Bounds> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetBoundsList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetBoundsList(int id, out IList<Bounds> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetBoundsList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Bounds> GetBoundsList(int id)
		{
			IList<Bounds> value = null;
			if (TryGetBoundsList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Bounds> GetBoundsList(string name)
		{
			IList<Bounds> value = null;
			if (TryGetBoundsList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // BoundsList
	}
}