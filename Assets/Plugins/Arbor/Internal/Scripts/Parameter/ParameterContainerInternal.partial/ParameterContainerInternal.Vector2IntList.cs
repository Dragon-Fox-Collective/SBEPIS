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
		internal List<Vector2IntListParameter> _Vector2IntListParameters = new List<Vector2IntListParameter>();

		#region Vector2IntList

		private bool SetVector2IntList(Parameter parameter, IList<Vector2Int> value)
		{
			return parameter != null && parameter.SetVector2IntList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2IntList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector2IntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector2IntList(string name, IList<Vector2Int> value)
		{
			Parameter parameter = GetParam(name);
			return SetVector2IntList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2IntList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector2IntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector2IntList(int id, IList<Vector2Int> value)
		{
			Parameter parameter = GetParam(id);
			return SetVector2IntList(parameter, value);
		}

		private bool TryGetVector2IntList(Parameter parameter, out IList<Vector2Int> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVector2IntList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2IntList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2IntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector2IntList(string name, out IList<Vector2Int> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVector2IntList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2IntList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2IntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector2IntList(int id, out IList<Vector2Int> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVector2IntList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2IntList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2IntList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Vector2Int> GetVector2IntList(int id)
		{
			IList<Vector2Int> value = null;
			if (TryGetVector2IntList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2IntList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2IntList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Vector2Int> GetVector2IntList(string name)
		{
			IList<Vector2Int> value = null;
			if (TryGetVector2IntList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // Vector2IntList
	}
}