//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorVector2Int : IParameterAccessor, IParameterAccessor<Vector2Int>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			Vector2Int value;
			if (parameter.TryGetVector2Int(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(Vector2Int);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetVector2Int((Vector2Int)value);
		}

		bool IParameterAccessor<Vector2Int>.TryGetValue(Parameter parameter, out Vector2Int outValue)
		{
			return parameter.TryGetVector2Int(out outValue);
		}

		bool IParameterAccessor<Vector2Int>.SetValue(Parameter parameter, Vector2Int value)
		{
			return parameter.SetVector2Int(value);
		}
	}
}