//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	using Internal;
	using Arbor.Extensions;

	public partial class ParameterContainerInternal : ParameterContainerBase, ISerializationCallbackReceiver
	{
		[SerializeField]
		[HideInDocument]
		internal List<RectInt> _RectIntParameters = new List<RectInt>();

		#region RectInt

		private bool SetRectInt(Parameter parameter, RectInt value)
		{
			return parameter != null && parameter.SetRectInt(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the RectInt type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRectInt(string name, RectInt value)
		{
			Parameter parameter = GetParam(name);
			return SetRectInt(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the RectInt type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRectInt(int id, RectInt value)
		{
			Parameter parameter = GetParam(id);
			return SetRectInt(parameter, value);
		}

		private bool TryGetRectInt(Parameter parameter, out RectInt value)
		{
			if (parameter != null)
			{
				return parameter.TryGetRectInt(out value);
			}

			value = RectIntExtensions.zero;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the RectInt type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRectInt(string name, out RectInt value)
		{
			Parameter parameter = GetParam(name);
			return TryGetRectInt(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the RectInt type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRectInt(int id, out RectInt value)
		{
			Parameter parameter = GetParam(id);
			return TryGetRectInt(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectInt type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public RectInt GetRectInt(string name, RectInt defaultValue)
		{
			RectInt value = RectIntExtensions.zero;
			if (TryGetRectInt(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はRectInt.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectInt type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns RectInt.zero.</returns>
#endif
		public RectInt GetRectInt(string name)
		{
			return GetRectInt(name, RectIntExtensions.zero);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectInt type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public RectInt GetRectInt(int id, RectInt defaultValue)
		{
			RectInt value = RectIntExtensions.zero;
			if (TryGetRectInt(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はRectInt.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectInt type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns RectInt.zero.</returns>
#endif
		public RectInt GetRectInt(int id)
		{
			return GetRectInt(id, RectIntExtensions.zero);
		}

		#endregion //RectInt
	}
}