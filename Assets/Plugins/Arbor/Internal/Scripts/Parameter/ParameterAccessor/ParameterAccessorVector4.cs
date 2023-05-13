//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorVector4 : IParameterAccessor, IParameterAccessor<Vector4>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			Vector4 value;
			if (parameter.TryGetVector4(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(Vector4);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetVector4((Vector4)value);
		}

		bool IParameterAccessor<Vector4>.TryGetValue(Parameter parameter, out Vector4 outValue)
		{
			return parameter.TryGetVector4(out outValue);
		}

		bool IParameterAccessor<Vector4>.SetValue(Parameter parameter, Vector4 value)
		{
			return parameter.SetVector4(value);
		}
	}
}