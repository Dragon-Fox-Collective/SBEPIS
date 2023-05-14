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
	/// BoolListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference BoolList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.BoolList)]
	public sealed class BoolListParameterReference : ParameterReference, IValueContainer<IList<bool>>
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
		public IList<bool> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetBoolList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetBoolList(value);
				}
			}
		}

		IList<bool> IValueGetter<IList<bool>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<bool>>.SetValue(IList<bool> value)
		{
			this.value = value;
		}
	}
}