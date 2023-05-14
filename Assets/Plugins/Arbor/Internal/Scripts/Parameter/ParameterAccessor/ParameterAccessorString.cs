//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorString : IParameterAccessor, IParameterAccessor<string>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			string value;
			if (parameter.TryGetString(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(string);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetString((string)value);
		}

		bool IParameterAccessor<string>.TryGetValue(Parameter parameter, out string outValue)
		{
			return parameter.TryGetString(out outValue);
		}

		bool IParameterAccessor<string>.SetValue(Parameter parameter, string value)
		{
			return parameter.SetString(value);
		}
	}
}