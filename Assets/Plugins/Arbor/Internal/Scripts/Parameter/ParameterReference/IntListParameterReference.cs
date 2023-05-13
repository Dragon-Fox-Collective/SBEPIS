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
	/// IntListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference IntList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.IntList)]
	public sealed class IntListParameterReference : ParameterReference, IValueContainer<IList<int>>
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
		public IList<int> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetIntList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetIntList(value);
				}
			}
		}

		IList<int> IValueGetter<IList<int>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<int>>.SetValue(IList<int> value)
		{
			this.value = value;
		}
	}
}