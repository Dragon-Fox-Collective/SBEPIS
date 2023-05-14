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
	/// Colorパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Color parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Color)]
	public sealed class ColorParameterReference : ParameterReference, IValueContainer<Color>
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
		public Color value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetColor();
				}

				return Color.white;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetColor(value);
				}
			}
		}

		Color IValueGetter<Color>.GetValue()
		{
			return value;
		}

		void IValueSetter<Color>.SetValue(Color value)
		{
			this.value = value;
		}
	}
}
