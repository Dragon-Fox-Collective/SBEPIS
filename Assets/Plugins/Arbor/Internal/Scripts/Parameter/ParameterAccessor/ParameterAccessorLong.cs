//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorLong : IParameterAccessor, IParameterAccessor<long>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			long value;
			if (parameter.TryGetLong(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(long);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetLong((long)value);
		}

		bool IParameterAccessor<long>.TryGetValue(Parameter parameter, out long outValue)
		{
			return parameter.TryGetLong(out outValue);
		}

		bool IParameterAccessor<long>.SetValue(Parameter parameter, long value)
		{
			return parameter.SetLong(value);
		}
	}
}