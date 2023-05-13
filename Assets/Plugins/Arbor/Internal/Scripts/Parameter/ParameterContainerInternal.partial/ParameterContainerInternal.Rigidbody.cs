//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public partial class ParameterContainerInternal : ParameterContainerBase, ISerializationCallbackReceiver
	{
		#region Rigidbody

		private bool SetRigidbody(Parameter parameter, Rigidbody value)
		{
			return parameter != null && parameter.SetRigidbody(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Rigidbody type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRigidbody(string name, Rigidbody value)
		{
			Parameter parameter = GetParam(name);
			return SetRigidbody(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Rigidbody type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRigidbody(int id, Rigidbody value)
		{
			Parameter parameter = GetParam(id);
			return SetRigidbody(parameter, value);
		}

		private bool TryGetRigidbody(Parameter parameter, out Rigidbody value)
		{
			if (parameter != null)
			{
				return parameter.TryGetRigidbody(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRigidbody(string name, out Rigidbody value)
		{
			Parameter parameter = GetParam(name);
			return TryGetRigidbody(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRigidbody(int id, out Rigidbody value)
		{
			Parameter parameter = GetParam(id);
			return TryGetRigidbody(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Rigidbody GetRigidbody(string name, Rigidbody defaultValue = null)
		{
			Rigidbody value = null;
			if (TryGetRigidbody(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Rigidbody GetRigidbody(int id, Rigidbody defaultValue = null)
		{
			Rigidbody value = null;
			if (TryGetRigidbody(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetRigidbody(string, out Rigidbody)")]
		public bool GetRigidbody(string name, out Rigidbody value)
		{
			return TryGetRigidbody(name, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetRigidbody(int, out Rigidbody)")]
		public bool GetRigidbody(int id, out Rigidbody value)
		{
			return TryGetRigidbody(id, out value);
		}

		#endregion //Rigidbody
	}
}