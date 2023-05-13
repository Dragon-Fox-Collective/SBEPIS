//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorBool : IParameterAccessor, IParameterAccessor<bool>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			bool value;
			if (parameter.TryGetBool(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(bool);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetBool((bool)value);
		}

		bool IParameterAccessor<bool>.TryGetValue(Parameter parameter, out bool outValue)
		{
			return parameter.TryGetBool(out outValue);
		}

		bool IParameterAccessor<bool>.SetValue(Parameter parameter, bool value)
		{
			return parameter.SetBool(value);
		}
	}
}