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
		internal List<Vector2ListParameter> _Vector2ListParameters = new List<Vector2ListParameter>();

		#region Vector2List

		private bool SetVector2List(Parameter parameter, IList<Vector2> value)
		{
			return parameter != null && parameter.SetVector2List(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2List型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector2List type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector2List(string name, IList<Vector2> value)
		{
			Parameter parameter = GetParam(name);
			return SetVector2List(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2List型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector2List type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVector2List(int id, IList<Vector2> value)
		{
			Parameter parameter = GetParam(id);
			return SetVector2List(parameter, value);
		}

		private bool TryGetVector2List(Parameter parameter, out IList<Vector2> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVector2List(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2List型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2List type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector2List(string name, out IList<Vector2> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVector2List(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2List型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2List type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVector2List(int id, out IList<Vector2> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVector2List(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2List型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2List type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Vector2> GetVector2List(int id)
		{
			IList<Vector2> value = null;
			if (TryGetVector2List(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2List型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2List type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Vector2> GetVector2List(string name)
		{
			IList<Vector2> value = null;
			if (TryGetVector2List(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // Vector2List
	}
}