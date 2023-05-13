//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.Extensions;
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference BoundsInt parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.BoundsInt)]
	public sealed class BoundsIntParameterReference : ParameterReference, IValueContainer<BoundsInt>
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
		public BoundsInt value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetBoundsInt();
				}

				return BoundsIntExtensions.zero;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetBoundsInt(value);
				}
			}
		}

		BoundsInt IValueGetter<BoundsInt>.GetValue()
		{
			return value;
		}

		void IValueSetter<BoundsInt>.SetValue(BoundsInt value)
		{
			this.value = value;
		}
	}
}