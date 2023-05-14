//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorRect : IParameterAccessor, IParameterAccessor<Rect>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			Rect value;
			if (parameter.TryGetRect(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(Rect);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetRect((Rect)value);
		}

		bool IParameterAccessor<Rect>.TryGetValue(Parameter parameter, out Rect outValue)
		{
			return parameter.TryGetRect(out outValue);
		}

		bool IParameterAccessor<Rect>.SetValue(Parameter parameter, Rect value)
		{
			return parameter.SetRect(value);
		}
	}
}