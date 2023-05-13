//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	public sealed partial class AnyParameterReference
	{
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
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetVariable();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetVariable(value);
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値を設定
		/// </summary>
		/// <typeparam name="TVariable">Variableの値の型</typeparam>
		/// <param name="value">値</param>
		/// <returns>値の設定に成功した場合にtrueを返す。</returns>
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
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.SetVariable<TVariable>(value);
			}

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
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.GetVariable<TVariable>();
			}

			return defaultValue;
		}
	}
}