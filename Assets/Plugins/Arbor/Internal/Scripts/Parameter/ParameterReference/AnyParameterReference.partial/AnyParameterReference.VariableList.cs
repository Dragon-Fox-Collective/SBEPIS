//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor
{
	public sealed partial class AnyParameterReference
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// VariableList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of VariableList type.
		/// </summary>
#endif
		public object variableListValue
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetVariableList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetVariableList(value);
				}
			}
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
		/// Set VariableList value
		/// </summary>
		/// <typeparam name="TVariable">Variable value type</typeparam>
		/// <param name="value">Value</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetVariableList<TVariable>(IList<TVariable> value)
		{
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.SetVariableList<TVariable>(value);
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableListの値を取得
		/// </summary>
		/// <typeparam name="TVariable">Variableの値の型</typeparam>
		/// <returns>Variableの値。取得に失敗した場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get VariableList value
		/// </summary>
		/// <typeparam name="TVariable">Variable value type</typeparam>
		/// <returns>Value of Variable. If acquisition fails, it returns null.</returns>
#endif
		public IList<TVariable> GetVariableList<TVariable>()
		{
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.GetVariableList<TVariable>();
			}

			return null;
		}
	}
}