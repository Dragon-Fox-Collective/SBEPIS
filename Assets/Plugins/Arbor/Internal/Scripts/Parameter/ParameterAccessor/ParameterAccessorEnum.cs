//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
	internal sealed class ParameterAccessorEnum : IParameterAccessor, IParameterAccessor<int>, IParameterAccessor<System.Enum>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			System.Enum value;
			if (parameter.TryGetEnum(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(System.Enum);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetEnumInt((int)value);
		}

		bool IParameterAccessor<int>.TryGetValue(Parameter parameter, out int outValue)
		{
			return parameter.TryGetEnumInt(out outValue);
		}

		bool IParameterAccessor<int>.SetValue(Parameter parameter, int value)
		{
			return parameter.SetEnumInt(value);
		}

		bool IParameterAccessor<System.Enum>.TryGetValue(Parameter parameter, out System.Enum outValue)
		{
			return parameter.TryGetEnum(out outValue);
		}

		bool IParameterAccessor<System.Enum>.SetValue(Parameter parameter, System.Enum value)
		{
			return parameter.SetEnum(value);
		}
	}
}