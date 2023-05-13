//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Floatパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Float parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Float)]
	public sealed class FloatParameterReference : ParameterReference, IValueContainer<float>
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
		public float value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetFloat();
				}

				return 0f;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetFloat(value);
				}
			}
		}

		float IValueGetter<float>.GetValue()
		{
			return value;
		}

		void IValueSetter<float>.SetValue(float value)
		{
			this.value = value;
		}
	}
}