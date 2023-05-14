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
		internal List<BoundsInt> _BoundsIntParameters = new List<BoundsInt>();

		#region BoundsInt

		private bool SetBoundsInt(Parameter parameter, BoundsInt value)
		{
			return parameter != null && parameter.SetBoundsInt(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsInt型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the BoundsInt type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetBoundsInt(string name, BoundsInt value)
		{
			Parameter parameter = GetParam(name);
			return SetBoundsInt(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsInt型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the BoundsInt type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetBoundsInt(int id, BoundsInt value)
		{
			Parameter parameter = GetParam(id);
			return SetBoundsInt(parameter, value);
		}

		private bool TryGetBoundsInt(Parameter parameter, out BoundsInt value)
		{
			if (parameter != null)
			{
				return parameter.TryGetBoundsInt(out value);
			}

			value = BoundsIntExtensions.zero;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsInt型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsInt type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetBoundsInt(string name, out BoundsInt value)
		{
			Parameter parameter = GetParam(name);
			return TryGetBoundsInt(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsInt型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsInt type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetBoundsInt(int id, out BoundsInt value)
		{
			Parameter parameter = GetParam(id);
			return TryGetBoundsInt(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsInt型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsInt type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public BoundsInt GetBoundsInt(string name, BoundsInt defaultValue)
		{
			BoundsInt value = BoundsIntExtensions.zero;
			if (TryGetBoundsInt(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsInt型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はBoundsInt.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsInt type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns BoundsInt.zero.</returns>
#endif
		public BoundsInt GetBoundsInt(string name)
		{
			return GetBoundsInt(name, BoundsIntExtensions.zero);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsInt型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsInt type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public BoundsInt GetBoundsInt(int id, BoundsInt defaultValue)
		{
			BoundsInt value = BoundsIntExtensions.zero;
			if (TryGetBoundsInt(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsInt型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はBoundsInt.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsInt type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns BoundsInt.zero.</returns>
#endif
		public BoundsInt GetBoundsInt(int id)
		{
			return GetBoundsInt(id, BoundsIntExtensions.zero);
		}

		#endregion //BoundsInt
	}
}