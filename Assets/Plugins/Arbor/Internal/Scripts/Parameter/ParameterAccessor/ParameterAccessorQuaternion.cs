//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal sealed class ParameterAccessorQuaternion : IParameterAccessor, IParameterAccessor<Quaternion>
	{
		bool IParameterAccessor.TryGetValue(Parameter parameter, out object outValue)
		{
			Quaternion value;
			if (parameter.TryGetQuaternion(out value))
			{
				outValue = value;
				return true;
			}

			outValue = default(Quaternion);
			return false;
		}

		bool IParameterAccessor.SetValue(Parameter parameter, object value)
		{
			return parameter.SetQuaternion((Quaternion)value);
		}

		bool IParameterAccessor<Quaternion>.TryGetValue(Parameter parameter, out Quaternion outValue)
		{
			return parameter.TryGetQuaternion(out outValue);
		}

		bool IParameterAccessor<Quaternion>.SetValue(Parameter parameter, Quaternion value)
		{
			return parameter.SetQuaternion(value);
		}
	}
}