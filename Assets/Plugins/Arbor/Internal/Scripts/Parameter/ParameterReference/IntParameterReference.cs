//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Intパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Int parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Int)]
	public sealed class IntParameterReference : ParameterReference, IValueContainer<int>
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
		public int value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetInt();
				}

				return 0;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetInt(value);
				}
			}
		}

		int IValueGetter<int>.GetValue()
		{
			return value;
		}

		void IValueSetter<int>.SetValue(int value)
		{
			this.value = value;
		}
	}
}