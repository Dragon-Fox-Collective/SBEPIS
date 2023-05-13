//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Rectパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Rect parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Rect)]
	public sealed class RectParameterReference : ParameterReference, IValueContainer<Rect>
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
		public Rect value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetRect();
				}

				return new Rect();
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetRect(value);
				}
			}
		}

		Rect IValueGetter<Rect>.GetValue()
		{
			return value;
		}

		void IValueSetter<Rect>.SetValue(Rect value)
		{
			this.value = value;
		}
	}
}
