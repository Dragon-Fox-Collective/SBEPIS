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
		[EulerAngles]
		internal List<Quaternion> _QuaternionParameters = new List<Quaternion>();

		#region Quaternion

		private bool SetQuaternion(Parameter parameter, Quaternion value)
		{
			return parameter != null && parameter.SetQuaternion(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Quaternion type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetQuaternion(string name, Quaternion value)
		{
			Parameter parameter = GetParam(name);
			return SetQuaternion(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Quaternion type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetQuaternion(int id, Quaternion value)
		{
			Parameter parameter = GetParam(id);
			return SetQuaternion(parameter, value);
		}

		private bool TryGetQuaternion(Parameter parameter, out Quaternion value)
		{
			if (parameter != null)
			{
				return parameter.TryGetQuaternion(out value);
			}

			value = Quaternion.identity;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Quaternion type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetQuaternion(string name, out Quaternion value)
		{
			Parameter parameter = GetParam(name);
			return TryGetQuaternion(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Quaternion type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetQuaternion(int id, out Quaternion value)
		{
			Parameter parameter = GetParam(id);
			return TryGetQuaternion(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Quaternion type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Quaternion GetQuaternion(string name, Quaternion defaultValue)
		{
			Quaternion value = Quaternion.identity;
			if (TryGetQuaternion(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はQuaternion.identityを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Quaternion type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Quaternion.identity.</returns>
#endif
		public Quaternion GetQuaternion(string name)
		{
			return GetQuaternion(name, Quaternion.identity);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Quaternion type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Quaternion GetQuaternion(int id, Quaternion defaultValue)
		{
			Quaternion value = Quaternion.identity;
			if (TryGetQuaternion(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はQuaternion.identityを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Quaternion type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Quaternion.identity.</returns>
#endif
		public Quaternion GetQuaternion(int id)
		{
			return GetQuaternion(id, Quaternion.identity);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Quaternion type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetQuaternion(string, out Quaternion)")]
		public bool GetQuaternion(string name, out Quaternion value)
		{
			return TryGetQuaternion(name, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Quaternion type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetQuaternion(int, out Quaternion)")]
		public bool GetQuaternion(int id, out Quaternion value)
		{
			return TryGetQuaternion(id, out value);
		}

		#endregion //Quaternion
	}
}