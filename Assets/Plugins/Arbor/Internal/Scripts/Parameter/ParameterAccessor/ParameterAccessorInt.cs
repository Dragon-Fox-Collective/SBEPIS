//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorInt : IParameterAccessor, IParameterAccessor<int>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			int value;
			if (parameter.TryGetInt(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(int);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetInt((int)value);
		}

		bool IParameterAccessor<int>.TryGetValue(Parameter parameter, out int outValue)
		{
			return parameter.TryGetInt(out outValue);
		}

		bool IParameterAccessor<int>.SetValue(Parameter parameter, int value)
		{
			return parameter.SetInt(value);
		}
	}
}