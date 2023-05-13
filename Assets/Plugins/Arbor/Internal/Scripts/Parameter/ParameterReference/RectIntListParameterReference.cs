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
	/// RectIntListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference RectIntList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.RectIntList)]
	public sealed class RectIntListParameterReference : ParameterReference, IValueContainer<IList<RectInt>>
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
		public IList<RectInt> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetRectIntList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetRectIntList(value);
				}
			}
		}

		IList<RectInt> IValueGetter<IList<RectInt>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<RectInt>>.SetValue(IList<RectInt> value)
		{
			this.value = value;
		}
	}
}