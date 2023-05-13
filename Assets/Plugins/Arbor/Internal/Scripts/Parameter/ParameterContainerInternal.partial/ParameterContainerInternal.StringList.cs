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
		internal List<StringListParameter> _StringListParameters = new List<StringListParameter>();

		#region StringList

		private bool SetStringList(Parameter parameter, IList<string> value)
		{
			return parameter != null && parameter.SetStringList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StringList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the StringList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetStringList(string name, IList<string> value)
		{
			Parameter parameter = GetParam(name);
			return SetStringList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StringList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the StringList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetStringList(int id, IList<string> value)
		{
			Parameter parameter = GetParam(id);
			return SetStringList(parameter, value);
		}

		private bool TryGetStringList(Parameter parameter, out IList<string> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetStringList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StringList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the StringList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetStringList(string name, out IList<string> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetStringList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StringList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the StringList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetStringList(int id, out IList<string> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetStringList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// StringList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the StringList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<string> GetStringList(int id)
		{
			IList<string> value = null;
			if (TryGetStringList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StringList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the StringList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<string> GetStringList(string name)
		{
			IList<string> value = null;
			if (TryGetStringList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // StringList
	}
}