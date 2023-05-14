//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorColor : IParameterAccessor, IParameterAccessor<Color>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			Color value;
			if (parameter.TryGetColor(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(Color);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetColor((Color)value);
		}

		bool IParameterAccessor<Color>.TryGetValue(Parameter parameter, out Color outValue)
		{
			return parameter.TryGetColor(out outValue);
		}

		bool IParameterAccessor<Color>.SetValue(Parameter parameter, Color value)
		{
			return parameter.SetColor(value);
		}
	}
}