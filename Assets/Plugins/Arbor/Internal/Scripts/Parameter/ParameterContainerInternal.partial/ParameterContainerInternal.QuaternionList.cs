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
		internal List<QuaternionListParameter> _QuaternionListParameters = new List<QuaternionListParameter>();

		#region QuaternionList

		private bool SetQuaternionList(Parameter parameter, IList<Quaternion> value)
		{
			return parameter != null && parameter.SetQuaternionList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// QuaternionList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the QuaternionList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetQuaternionList(string name, IList<Quaternion> value)
		{
			Parameter parameter = GetParam(name);
			return SetQuaternionList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// QuaternionList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the QuaternionList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetQuaternionList(int id, IList<Quaternion> value)
		{
			Parameter parameter = GetParam(id);
			return SetQuaternionList(parameter, value);
		}

		private bool TryGetQuaternionList(Parameter parameter, out IList<Quaternion> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetQuaternionList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// QuaternionList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the QuaternionList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetQuaternionList(string name, out IList<Quaternion> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetQuaternionList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// QuaternionList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the QuaternionList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetQuaternionList(int id, out IList<Quaternion> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetQuaternionList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// QuaternionList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the QuaternionList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Quaternion> GetQuaternionList(int id)
		{
			IList<Quaternion> value = null;
			if (TryGetQuaternionList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// QuaternionList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the QuaternionList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Quaternion> GetQuaternionList(string name)
		{
			IList<Quaternion> value = null;
			if (TryGetQuaternionList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // QuaternionList
	}
}