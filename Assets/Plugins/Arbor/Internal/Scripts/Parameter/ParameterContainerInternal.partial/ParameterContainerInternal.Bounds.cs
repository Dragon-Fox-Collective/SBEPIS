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
		internal List<Bounds> _BoundsParameters = new List<Bounds>();

		#region Bounds

		private bool SetBounds(Parameter parameter, Bounds value)
		{
			return parameter != null && parameter.SetBounds(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Bounds type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetBounds(string name, Bounds value)
		{
			Parameter parameter = GetParam(name);
			return SetBounds(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Bounds type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetBounds(int id, Bounds value)
		{
			Parameter parameter = GetParam(id);
			return SetBounds(parameter, value);
		}

		private bool TryGetBounds(Parameter parameter, out Bounds value)
		{
			if (parameter != null)
			{
				return parameter.TryGetBounds(out value);
			}

			value = new Bounds();
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Bounds type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetBounds(string name, out Bounds value)
		{
			Parameter parameter = GetParam(name);
			return TryGetBounds(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Bounds type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetBounds(int id, out Bounds value)
		{
			Parameter parameter = GetParam(id);
			return TryGetBounds(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Bounds type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Bounds GetBounds(string name, Bounds defaultValue)
		{
			Bounds value = new Bounds();
			if (TryGetBounds(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合は0 Boundsを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Bounds type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns 0 bounds.</returns>
#endif
		public Bounds GetBounds(string name)
		{
			return GetBounds(name, new Bounds());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Bounds type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Bounds GetBounds(int id, Bounds defaultValue)
		{
			Bounds value = new Bounds();
			if (TryGetBounds(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合は0 Boundsを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Bounds type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns 0 bounds.</returns>
#endif
		public Bounds GetBounds(int id)
		{
			return GetBounds(id, new Bounds());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Bounds type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetBounds(string, out Bounds)")]
		public bool GetBounds(string name, out Bounds value)
		{
			return TryGetBounds(name, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Bounds type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetBounds(int, out Bounds)")]
		public bool GetBounds(int id, out Bounds value)
		{
			return TryGetBounds(id, out value);
		}

		#endregion // Bounds
	}
}