//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorVector3 : IParameterAccessor, IParameterAccessor<Vector3>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			Vector3 value;
			if (parameter.TryGetVector3(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(Vector3);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetVector3((Vector3)value);
		}

		bool IParameterAccessor<Vector3>.TryGetValue(Parameter parameter, out Vector3 outValue)
		{
			return parameter.TryGetVector3(out outValue);
		}

		bool IParameterAccessor<Vector3>.SetValue(Parameter parameter, Vector3 value)
		{
			return parameter.SetVector3(value);
		}
	}
}