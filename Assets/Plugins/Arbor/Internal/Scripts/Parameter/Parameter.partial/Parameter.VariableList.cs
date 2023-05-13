//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListを格納しているオブジェクト。
		/// </summary>
#else
		/// <summary>
		/// Object that contains VariableList.
		/// </summary>
#endif
		public VariableListBase variableListObject
		{
			get
			{
				if (type != Type.VariableList)
				{
					throw new ParameterTypeMismatchException();
				}

				return Internal_GetObject() as VariableListBase;
			}
			set
			{
				if (type != Type.VariableList)
				{
					throw new ParameterTypeMismatchException();
				}

				Internal_SetObject(value);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Variable type.
		/// </summary>
#endif
		public object variableListValue
		{
			get
			{
				object value;
				if (TryGetVariableList(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetVariableList(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region VariableList

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を設定
		/// </summary>
		/// <param name="value">値</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set VariableList value
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetVariableList(object value)
		{
			if (type == Type.VariableList)
			{
				VariableListBase variable = Internal_GetObject() as VariableListBase;
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
		/// VariableList型の値を取得する。
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
		public bool TryGetVariableList(out object value)
		{
			if (type == Type.VariableList)
			{
				VariableListBase variable = Internal_GetObject() as VariableListBase;
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
		/// VariableListの値を取得
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>Variableの値。取得に失敗した場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get Variable value
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>Value of Variable. If acquisition fails, it returns null.</returns>
#endif
		public object GetVariableList(object defaultValue = null)
		{
			object value;
			if (TryGetVariableList(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を設定
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
		public bool SetVariableList<TVariable>(IList<TVariable> value)
		{
			if (type == Type.VariableList)
			{
				VariableListBase variable = Internal_GetObject() as VariableListBase;
				if (variable != null && TypeUtility.IsAssignableFrom(variable.valueType, typeof(IList<TVariable>)))
				{
					variable.valueObject = value;
					DoChanged();

					return true;
				}
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableList型の値を取得する。
		/// </summary>
		/// <typeparam name="TVariable">Variableの値の型</typeparam>
		/// <param name="value">取得する値。</param>
		/// <returns>成功した場合はtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the VariableList type.
		/// </summary>
		/// <typeparam name="TVariable">Variable value type</typeparam>
		/// <param name="value">The value you get.</param>
		/// <returns>Returns true if it succeeds.</returns>
#endif
		public bool TryGetVariableList<TVariable>(out IList<TVariable> value)
		{
			if (type == Type.VariableList)
			{
				VariableListBase variable = Internal_GetObject() as VariableListBase;
				if (variable != null && TypeUtility.IsAssignableFrom(typeof(IList<TVariable>), variable.valueType))
				{
					value = (IList<TVariable>)variable.valueObject;
					return true;
				}
			}

			value = null;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を取得
		/// </summary>
		/// <typeparam name="TVariable">VariableListの値の型</typeparam>
		/// <returns>Variableの値。取得に失敗した場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get Variable value
		/// </summary>
		/// <typeparam name="TVariable">Variable value type</typeparam>
		/// <returns>Value of Variable. If acquisition fails, it returns null.</returns>
#endif
		public IList<TVariable> GetVariableList<TVariable>()
		{
			IList<TVariable> value;
			if (TryGetVariableList<TVariable>(out value))
			{
				return value;
			}
			return null;
		}

		#endregion // VariableList
	}
}