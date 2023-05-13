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
	/// RectListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference RectList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.RectList)]
	public sealed class RectListParameterReference : ParameterReference, IValueContainer<IList<Rect>>
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
		public IList<Rect> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetRectList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetRectList(value);
				}
			}
		}

		IList<Rect> IValueGetter<IList<Rect>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<Rect>>.SetValue(IList<Rect> value)
		{
			this.value = value;
		}
	}
}