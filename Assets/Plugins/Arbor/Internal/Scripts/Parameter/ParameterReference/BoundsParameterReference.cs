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
	/// Boundsパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Bounds parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Bounds)]
	public sealed class BoundsParameterReference : ParameterReference, IValueContainer<Bounds>
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
		public Bounds value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetBounds();
				}

				return new Bounds();
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetBounds(value);
				}
			}
		}

		Bounds IValueGetter<Bounds>.GetValue()
		{
			return value;
		}

		void IValueSetter<Bounds>.SetValue(Bounds value)
		{
			this.value = value;
		}
	}
}
