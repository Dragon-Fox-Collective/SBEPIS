//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Longパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Long parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Long)]
	public sealed class LongParameterReference : ParameterReference, IValueContainer<long>
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
		public long value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetLong();
				}

				return 0L;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetLong(value);
				}
			}
		}

		long IValueGetter<long>.GetValue()
		{
			return value;
		}

		void IValueSetter<long>.SetValue(long value)
		{
			this.value = value;
		}
	}
}