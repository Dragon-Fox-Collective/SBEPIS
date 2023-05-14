//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorBounds : IParameterAccessor, IParameterAccessor<Bounds>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			Bounds value;
			if (parameter.TryGetBounds(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(Bounds);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetBounds((Bounds)value);
		}

		bool IParameterAccessor<Bounds>.TryGetValue(Parameter parameter, out Bounds outValue)
		{
			return parameter.TryGetBounds(out outValue);
		}

		bool IParameterAccessor<Bounds>.SetValue(Parameter parameter, Bounds value)
		{
			return parameter.SetBounds(value);
		}
	}
}