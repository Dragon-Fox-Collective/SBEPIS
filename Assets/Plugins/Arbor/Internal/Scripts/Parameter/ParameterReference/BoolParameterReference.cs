//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Boolパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Bool parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Bool)]
	public sealed class BoolParameterReference : ParameterReference, IValueContainer<bool>
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
		public bool value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetBool(false);
				}

				return false;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetBool(value);
				}
			}
		}

		bool IValueGetter<bool>.GetValue()
		{
			return value;
		}

		void IValueSetter<bool>.SetValue(bool value)
		{
			this.value = value;
		}
	}
}
