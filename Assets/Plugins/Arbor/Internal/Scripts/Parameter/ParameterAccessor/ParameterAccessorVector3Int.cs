//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorVector3Int : IParameterAccessor, IParameterAccessor<Vector3Int>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			Vector3Int value;
			if (parameter.TryGetVector3Int(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(Vector3Int);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetVector3Int((Vector3Int)value);
		}

		bool IParameterAccessor<Vector3Int>.TryGetValue(Parameter parameter, out Vector3Int outValue)
		{
			return parameter.TryGetVector3Int(out outValue);
		}

		bool IParameterAccessor<Vector3Int>.SetValue(Parameter parameter, Vector3Int value)
		{
			return parameter.SetVector3Int(value);
		}
	}
}