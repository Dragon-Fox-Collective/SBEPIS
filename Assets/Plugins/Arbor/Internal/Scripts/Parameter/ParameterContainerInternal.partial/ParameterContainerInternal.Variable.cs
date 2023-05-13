//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public partial class ParameterContainerInternal : ParameterContainerBase, ISerializationCallbackReceiver
	{
		#region Variable

		private bool SetVariable(Parameter parameter, object value)
		{
			return parameter != null && parameter.SetVariable(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of Variable.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVariable(string name, object value)
		{
			Parameter parameter = GetParam(name);
			return SetVariable(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of Variable.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVariable(int id, object value)
		{
			Parameter parameter = GetParam(id);
			return SetVariable(parameter, value);
		}

		private bool TryGetVariable(Parameter parameter, out object value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVariable(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of Variable.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVariable(string name, out object value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVariable(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of Variable.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVariable(int id, out object value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVariable(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of Variable.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public object GetVariable(string name, object defaultValue = null)
		{
			object value;
			if (TryGetVariable(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of Variable.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public object GetVariable(int id, object defaultValue = null)
		{
			object value;
			if (TryGetVariable(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

		private bool SetVariable<TVariable>(Parameter parameter, TVariable value)
		{
			return parameter != null && parameter.SetVariable<TVariable>(value);
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
		public bool SetVariable<TVariable>(string name, TVariable value)
		{
			Parameter parameter = GetParam(name);
			return SetVariable<TVariable>(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を設定する。
		/// </summary>
		/// <typeparam name="TVariable">設定するVariableの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of Variable.
		/// </summary>
		/// <typeparam name="TVariable">Type of Variable to set</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetVariable<TVariable>(int id, TVariable value)
		{
			Parameter parameter = GetParam(id);
			return SetVariable<TVariable>(parameter, value);
		}

		private bool TryGetVariable<TVariable>(Parameter parameter, out TVariable value)
		{
			if (parameter != null)
			{
				return parameter.TryGetVariable<TVariable>(out value);
			}

			value = default(TVariable);
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を取得する。
		/// </summary>
		/// <typeparam name="TVariable">取得するVariableの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of Variable.
		/// </summary>
		/// <typeparam name="TVariable">Type of Variable to get</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVariable<TVariable>(string name, out TVariable value)
		{
			Parameter parameter = GetParam(name);
			return TryGetVariable<TVariable>(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を取得する。
		/// </summary>
		/// <typeparam name="TVariable">取得するVariableの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of Variable.
		/// </summary>
		/// <typeparam name="TVariable">Type of Variable to get</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetVariable<TVariable>(int id, out TVariable value)
		{
			Parameter parameter = GetParam(id);
			return TryGetVariable<TVariable>(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を取得する。
		/// </summary>
		/// <typeparam name="TVariable">取得するVariableの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of Variable.
		/// </summary>
		/// <typeparam name="TVariable">Type of Variable to get</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public TVariable GetVariable<TVariable>(string name, TVariable defaultValue = default(TVariable))
		{
			TVariable value;
			if (TryGetVariable<TVariable>(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を取得する。
		/// </summary>
		/// <typeparam name="TVariable">取得するVariableの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of Variable.
		/// </summary>
		/// <typeparam name="TVariable">Type of Variable to get</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public TVariable GetVariable<TVariable>(int id, TVariable defaultValue = default(TVariable))
		{
			TVariable value;
			if (TryGetVariable<TVariable>(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

		#endregion //Variable
	}
}