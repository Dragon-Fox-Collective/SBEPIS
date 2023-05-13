//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorFloat : IParameterAccessor, IParameterAccessor<float>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			float value;
			if (parameter.TryGetFloat(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(float);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetFloat((float)value);
		}

		bool IParameterAccessor<float>.TryGetValue(Parameter parameter, out float outValue)
		{
			return parameter.TryGetFloat(out outValue);
		}

		bool IParameterAccessor<float>.SetValue(Parameter parameter, float value)
		{
			return parameter.SetFloat(value);
		}
	}
}