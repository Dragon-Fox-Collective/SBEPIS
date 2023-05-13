//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorRectInt : IParameterAccessor, IParameterAccessor<RectInt>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			RectInt value;
			if (parameter.TryGetRectInt(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(RectInt);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetRectInt((RectInt)value);
		}

		bool IParameterAccessor<RectInt>.TryGetValue(Parameter parameter, out RectInt outValue)
		{
			return parameter.TryGetRectInt(out outValue);
		}

		bool IParameterAccessor<RectInt>.SetValue(Parameter parameter, RectInt value)
		{
			return parameter.SetRectInt(value);
		}
	}
}