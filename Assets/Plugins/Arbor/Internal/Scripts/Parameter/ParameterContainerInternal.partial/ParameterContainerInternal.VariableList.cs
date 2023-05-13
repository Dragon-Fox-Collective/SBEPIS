//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	public partial class ParameterContainerInternal : ParameterContainerBase, ISerializationCallbackReceiver
	{
		#region VariableList

		private bool SetVariableList(Parameter parameter, object value)
		{
			return parameter != null && parameter.SetVariableList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of VariableList.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVariableList(string name, object value)
		{
			Parameter parameter = GetParam(name);
			return SetVariableList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of VariableList.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVariableList(int id, object value)
		{
			Parameter parameter = GetParam(id);
			return SetVariableList(parameter, value);
		}

		private bool TryGetVariableList(Parameter parameter, out object value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVariableList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of VariableList.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVariableList(string name, out object value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVariableList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of VariableList.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVariableList(int id, out object value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVariableList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of VariableList.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public object GetVariableList(string name, object defaultValue = null)
		{
			object value;
			if (TryGetVariableList(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of VariableList.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public object GetVariableList(int id, object defaultValue = null)
		{
			object value;
			if (TryGetVariableList(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

		private bool SetVariableList<TVariable>(Parameter parameter, IList<TVariable> value)
		{
			return parameter != null && parameter.SetVariableList<TVariable>(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を設定する。
		/// </summary>
		/// <typeparam name="TVariable">設定するVariableの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of Variable.
		/// </summary>
		/// <typeparam name="TVariable">Type of Variable to set</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVariableList<TVariable>(string name, IList<TVariable> value)
		{
			Parameter parameter = GetParam(name);
			return SetVariableList<TVariable>(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を設定する。
		/// </summary>
		/// <typeparam name="TVariable">設定するVariableの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of VariableList.
		/// </summary>
		/// <typeparam name="TVariable">Type of Variable to set</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVariableList<TVariable>(int id, IList<TVariable> value)
		{
			Parameter parameter = GetParam(id);
			return SetVariableList<TVariable>(parameter, value);
		}

		private bool TryGetVariableList<TVariable>(Parameter parameter, out IList<TVariable> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVariableList<TVariable>(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を取得する。
		/// </summary>
		/// <typeparam name="TVariable">取得するVariableの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of VariableList.
		/// </summary>
		/// <typeparam name="TVariable">Type of Variable to get</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVariableList<TVariable>(string name, out IList<TVariable> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVariableList<TVariable>(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を取得する。
		/// </summary>
		/// <typeparam name="TVariable">取得するVariableの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of VariableList.
		/// </summary>
		/// <typeparam name="TVariable">Type of Variable to get</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVariableList<TVariable>(int id, out IList<TVariable> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVariableList<TVariable>(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を取得する。
		/// </summary>
		/// <typeparam name="TVariable">取得するVariableの型</typeparam>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of VariableList.
		/// </summary>
		/// <typeparam name="TVariable">Type of Variable to get</typeparam>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<TVariable> GetVariableList<TVariable>(string name)
		{
			IList<TVariable> value;
			if (TryGetVariableList<TVariable>(name, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を取得する。
		/// </summary>
		/// <typeparam name="TVariable">取得するVariableの型</typeparam>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of VariableList.
		/// </summary>
		/// <typeparam name="TVariable">Type of Variable to get</typeparam>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<TVariable> GetVariableList<TVariable>(int id)
		{
			IList<TVariable> value;
			if (TryGetVariableList<TVariable>(id, out value))
			{
				return value;
			}
			return null;
		}

		#endregion //VariableList
	}
}