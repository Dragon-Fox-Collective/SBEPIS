//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// FloatListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference FloatList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.FloatList)]
	public sealed class FloatListParameterReference : ParameterReference, IValueContainer<IList<float>>
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
		public IList<float> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetFloatList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetFloatList(value);
				}
			}
		}

		IList<float> IValueGetter<IList<float>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<float>>.SetValue(IList<float> value)
		{
			this.value = value;
		}
	}
}