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
	/// RectIntパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference RectInt parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.RectInt)]
	public sealed class RectIntParameterReference : ParameterReference, IValueContainer<RectInt>
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
		public RectInt value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetRectInt();
				}

				return RectIntExtensions.zero;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetRectInt(value);
				}
			}
		}

		RectInt IValueGetter<RectInt>.GetValue()
		{
			return value;
		}

		void IValueSetter<RectInt>.SetValue(RectInt value)
		{
			this.value = value;
		}
	}
}