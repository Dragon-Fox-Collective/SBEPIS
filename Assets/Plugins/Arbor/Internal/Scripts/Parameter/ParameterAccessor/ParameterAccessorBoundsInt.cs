//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorBoundsInt : IParameterAccessor, IParameterAccessor<BoundsInt>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			BoundsInt value;
			if (parameter.TryGetBoundsInt(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(BoundsInt);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetBoundsInt((BoundsInt)value);
		}

		bool IParameterAccessor<BoundsInt>.TryGetValue(Parameter parameter, out BoundsInt outValue)
		{
			return parameter.TryGetBoundsInt(out outValue);
		}

		bool IParameterAccessor<BoundsInt>.SetValue(Parameter parameter, BoundsInt value)
		{
			return parameter.SetBoundsInt(value);
		}
	}
}