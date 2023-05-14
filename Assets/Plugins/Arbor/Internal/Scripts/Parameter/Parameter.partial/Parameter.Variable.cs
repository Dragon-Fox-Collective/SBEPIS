//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Variableを格納しているオブジェクト。
		/// </summary>
#else
		/// <summary>
		/// Object that contains Variable.
		/// </summary>
#endif
		public VariableBase variableObject
		{
			get
			{
				if (type != Type.Variable)
				{
					throw new ParameterTypeMismatchException();
				}

				return Internal_GetObject() as VariableBase;
			}
			set
			{
				if (type != Type.Variable)
				{
					throw new ParameterTypeMismatchException();
				}

				Internal_SetObject(value);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variable型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Variable type.
		/// </summary>
#endif
		public object variableValue
		{
			get
			{
				object value;
				if (TryGetVariable(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetVariable(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Variable

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を設定
		/// </summary>
		/// <param name="value">値</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set Variable value
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetVariable(object value)
		{
			if (type == Type.Variable)
			{
				VariableBase variable = Internal_GetObject() as VariableBase;
				if (variable != null)
				{
					System.Type variableType = variable.valueType;
					if (!TypeUtility.IsValueType(variableType) && value == null)
					{
						variable.valueObject = null;
						DoChanged();

						return true;
					}
					else if (value != null && TypeUtility.IsAssignableFrom(variableType, value.GetType()))
					{
						variable.valueObject = value;
						DoChanged();

						return true;
					}
				}
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variable型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		/// <returns>成功した場合はtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Variable type.
		/// </summary>
		/// <param name="value">The value you get.</param>
		/// <returns>Returns true if it succeeds.</returns>
#endif
		public bool TryGetVariable(out object value)
		{
			if (type == Type.Variable)
			{
				VariableBase variable = Internal_GetObject() as VariableBase;
				if (variable != null)
				{
					value = variable.valueObject;
					return true;
				}
			}

			value = null;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を取得
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。取得に失敗した場合に返される。</param>
		/// <returns>Variableの値。取得に失敗した場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get Variable value
		/// </summary>
		/// <param name="defaultValue">Default value. Returned if acquisition failed.</param>
		/// <returns>Value of Variable. If acquisition fails, it returns defaultValue.</returns>
#endif
		public object GetVariable(object defaultValue = null)
		{
			object value;
			if (TryGetVariable(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を設定
		/// </summary>
		/// <typeparam name="TVariable">Variableの値の型</typeparam>
		/// <param name="value">値</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set Variable value
		/// </summary>
		/// <typeparam name="TVariable">Variable value type</typeparam>
		/// <param name="value">Value</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetVariable<TVariable>(TVariable value)
		{
			if (type == Type.Variable)
			{
				VariableBase variable = Internal_GetObject() as VariableBase;
				if (variable != null)
				{
					Variable<TVariable> v = variable as Variable<TVariable>;
					if (v != null)
					{
						v.value = value;
						DoChanged();

						return true;
					}

					if (TypeUtility.IsAssignableFrom(variable.valueType, typeof(TVariable)))
					{
						variable.valueObject = value;
						DoChanged();

						return true;
					}
				}
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variable型の値を取得する。
		/// </summary>
		/// <typeparam name="TVariable">Variableの値の型</typeparam>
		/// <param name="value">取得する値。</param>
		/// <returns>成功した場合はtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Variable type.
		/// </summary>
		/// <typeparam name="TVariable">Variable value type</typeparam>
		/// <param name="value">The value you get.</param>
		/// <returns>Returns true if it succeeds.</returns>
#endif
		public bool TryGetVariable<TVariable>(out TVariable value)
		{
			if (type == Type.Variable)
			{
				VariableBase variable = Internal_GetObject() as VariableBase;
				if (variable != null)
				{
					Variable<TVariable> v = variable as Variable<TVariable>;
					if (v != null)
					{
						value = v.value;
						return true;
					}

					if (TypeUtility.IsAssignableFrom(typeof(TVariable), variable.valueType))
					{
						value = (TVariable)variable.valueObject;
						return true;
					}
				}
			}

			value = default(TVariable);
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を取得
		/// </summary>
		/// <typeparam name="TVariable">Variableの値の型</typeparam>
		/// <param name="defaultValue">デフォルトの値。取得に失敗した場合に返される。</param>
		/// <returns>Variableの値。取得に失敗した場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get Variable value
		/// </summary>
		/// <typeparam name="TVariable">Variable value type</typeparam>
		/// <param name="defaultValue">Default value. Returned if acquisition failed.</param>
		/// <returns>Value of Variable. If acquisition fails, it returns defaultValue.</returns>
#endif
		public TVariable GetVariable<TVariable>(TVariable defaultValue = default(TVariable))
		{
			TVariable value;
			if (TryGetVariable<TVariable>(out value))
			{
				return value;
			}
			return defaultValue;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を取得
		/// </summary>
		/// <typeparam name="TVariable">Variableの値の型</typeparam>
		/// <param name="value">値</param>
		/// <returns>trueなら成功、falseなら失敗</returns>
#else
		/// <summary>
		/// Get Variable value
		/// </summary>
		/// <typeparam name="TVariable">Variable value type</typeparam>
		/// <param name="value">Value</param>
		/// <returns>Succeeds if true, failure if false</returns>
#endif
		[System.Obsolete("use GetVariable<TVariable>(TVariable defaultValue) or TryGetVariable<TVariable>(out TVariable)")]
		public bool GetVariable<TVariable>(ref TVariable value)
		{
			return TryGetVariable<TVariable>(out value);
		}

		#endregion // Variable
	}
}