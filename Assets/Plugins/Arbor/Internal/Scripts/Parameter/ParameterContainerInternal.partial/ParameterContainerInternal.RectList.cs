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
		internal List<RectListParameter> _RectListParameters = new List<RectListParameter>();

		#region RectList

		private bool SetRectList(Parameter parameter, IList<Rect> value)
		{
			return parameter != null && parameter.SetRectList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the RectList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRectList(string name, IList<Rect> value)
		{
			Parameter parameter = GetParam(name);
			return SetRectList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the RectList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRectList(int id, IList<Rect> value)
		{
			Parameter parameter = GetParam(id);
			return SetRectList(parameter, value);
		}

		private bool TryGetRectList(Parameter parameter, out IList<Rect> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetRectList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the RectList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRectList(string name, out IList<Rect> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetRectList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the RectList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRectList(int id, out IList<Rect> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetRectList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// RectList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Rect> GetRectList(int id)
		{
			IList<Rect> value = null;
			if (TryGetRectList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Rect> GetRectList(string name)
		{
			IList<Rect> value = null;
			if (TryGetRectList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // RectList
	}
}