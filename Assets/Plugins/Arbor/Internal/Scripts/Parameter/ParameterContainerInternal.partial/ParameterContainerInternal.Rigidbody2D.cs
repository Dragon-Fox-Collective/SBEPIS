//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public partial class ParameterContainerInternal : ParameterContainerBase, ISerializationCallbackReceiver
	{
		#region Rigidbody2D

		private bool SetRigidbody2D(Parameter parameter, Rigidbody2D value)
		{
			return parameter != null && parameter.SetRigidbody2D(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody2D型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Rigidbody2D type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRigidbody2D(string name, Rigidbody2D value)
		{
			Parameter parameter = GetParam(name);
			return SetRigidbody2D(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody2D型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Rigidbody2D type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRigidbody2D(int id, Rigidbody2D value)
		{
			Parameter parameter = GetParam(id);
			return SetRigidbody2D(parameter, value);
		}

		private bool TryGetRigidbody2D(Parameter parameter, out Rigidbody2D value)
		{
			if (parameter != null)
			{
				return parameter.TryGetRigidbody2D(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody2D型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody2D type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRigidbody2D(string name, out Rigidbody2D value)
		{
			Parameter parameter = GetParam(name);
			return TryGetRigidbody2D(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody2D型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody2D type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRigidbody2D(int id, out Rigidbody2D value)
		{
			Parameter parameter = GetParam(id);
			return TryGetRigidbody2D(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody2D型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody2D type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public Rigidbody2D GetRigidbody2D(string name, Rigidbody2D defaultValue = null)
		{
			Rigidbody2D value = null;
			if (TryGetRigidbody2D(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody2D型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody2D type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public Rigidbody2D GetRigidbody2D(int id, Rigidbody2D defaultValue = null)
		{
			Rigidbody2D value = null;
			if (TryGetRigidbody2D(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody2D型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody2D type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetRigidbody2D(string, out Rigidbody2D)")]
		public bool GetRigidbody2D(string name, out Rigidbody2D value)
		{
			return TryGetRigidbody2D(name, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody2D型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody2D type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetRigidbody2D(int, out Rigidbody2D)")]
		public bool GetRigidbody2D(int id, out Rigidbody2D value)
		{
			return TryGetRigidbody2D(id, out value);
		}

		#endregion //Rigidbody2D
	}
}