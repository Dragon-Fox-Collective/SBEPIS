//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorVector2 : IParameterAccessor, IParameterAccessor<Vector2>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			Vector2 value;
			if (parameter.TryGetVector2(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(Vector2);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetVector2((Vector2)value);
		}

		bool IParameterAccessor<Vector2>.TryGetValue(Parameter parameter, out Vector2 outValue)
		{
			return parameter.TryGetVector2(out outValue);
		}

		bool IParameterAccessor<Vector2>.SetValue(Parameter parameter, Vector2 value)
		{
			return parameter.SetVector2(value);
		}
	}
}