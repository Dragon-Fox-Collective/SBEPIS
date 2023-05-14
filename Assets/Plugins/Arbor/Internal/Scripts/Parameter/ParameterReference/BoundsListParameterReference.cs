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
	/// BoundsListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference BoundsList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.BoundsList)]
	public sealed class BoundsListParameterReference : ParameterReference, IValueContainer<IList<Bounds>>
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
		public IList<Bounds> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetBoundsList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetBoundsList(value);
				}
			}
		}

		IList<Bounds> IValueGetter<IList<Bounds>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<Bounds>>.SetValue(IList<Bounds> value)
		{
			this.value = value;
		}
	}
}