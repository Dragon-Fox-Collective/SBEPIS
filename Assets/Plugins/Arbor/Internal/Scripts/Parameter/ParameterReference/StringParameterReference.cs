//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Stringパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference String parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.String)]
	public sealed class StringParameterReference : ParameterReference, IValueContainer<string>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値。
		/// </summary>
#else
		/// <summary>
		/// Value of the parameter
		/// </summary>
#endif
		public string value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetString();
				}

				return "";
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetString(value);
				}
			}
		}

		string IValueGetter<string>.GetValue()
		{
			return value;
		}

		void IValueSetter<string>.SetValue(string value)
		{
			this.value = value;
		}
	}
}
