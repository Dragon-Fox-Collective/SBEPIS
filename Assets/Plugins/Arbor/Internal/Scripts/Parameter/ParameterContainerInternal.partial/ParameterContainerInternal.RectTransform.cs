//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public partial class ParameterContainerInternal : ParameterContainerBase, ISerializationCallbackReceiver
	{
		#region RectTransform

		private bool SetRectTransform(Parameter parameter, RectTransform value)
		{
			return parameter != null && parameter.SetRectTransform(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectTransform型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the RectTransform type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRectTransform(string name, RectTransform value)
		{
			Parameter parameter = GetParam(name);
			return SetRectTransform(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectTransform型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the RectTransform type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRectTransform(int id, RectTransform value)
		{
			Parameter parameter = GetParam(id);
			return SetRectTransform(parameter, value);
		}

		private bool TryGetRectTransform(Parameter parameter, out RectTransform value)
		{
			if (parameter != null)
			{
				return parameter.TryGetRectTransform(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectTransform型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the RectTransform type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRectTransform(string name, out RectTransform value)
		{
			Parameter parameter = GetParam(name);
			return TryGetRectTransform(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectTransform型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the RectTransform type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRectTransform(int id, out RectTransform value)
		{
			Parameter parameter = GetParam(id);
			return TryGetRectTransform(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectTransform型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectTransform type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public RectTransform GetRectTransform(string name, RectTransform defaultValue = null)
		{
			RectTransform value = null;
			if (TryGetRectTransform(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectTransform型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectTransform type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public RectTransform GetRectTransform(int id, RectTransform defaultValue = null)
		{
			RectTransform value = null;
			if (TryGetRectTransform(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectTransform型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the RectTransform type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetRectTransform(string, out RectTransform)")]
		public bool GetRectTransform(string name, out RectTransform value)
		{
			return TryGetRectTransform(name, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectTransform型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the RectTransform type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetRectTransform(int, out RectTransform)")]
		public bool GetRectTransform(int id, out RectTransform value)
		{
			return TryGetRectTransform(id, out value);
		}

		#endregion //RectTransform
	}
}