//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference BoundsIntList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.BoundsIntList)]
	public sealed class BoundsIntListParameterReference : ParameterReference, IValueContainer<IList<BoundsInt>>
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
		public IList<BoundsInt> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetBoundsIntList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetBoundsIntList(value);
				}
			}
		}

		IList<BoundsInt> IValueGetter<IList<BoundsInt>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<BoundsInt>>.SetValue(IList<BoundsInt> value)
		{
			this.value = value;
		}
	}
}