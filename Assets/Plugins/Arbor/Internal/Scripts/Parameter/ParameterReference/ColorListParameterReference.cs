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
	/// ColorListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference ColorList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.ColorList)]
	public sealed class ColorListParameterReference : ParameterReference, IValueContainer<IList<Color>>
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
		public IList<Color> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetColorList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetColorList(value);
				}
			}
		}

		IList<Color> IValueGetter<IList<Color>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<Color>>.SetValue(IList<Color> value)
		{
			this.value = value;
		}
	}
}