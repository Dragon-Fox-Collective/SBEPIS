//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3パラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Vector3 parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Vector3)]
	public sealed class Vector3ParameterReference : ParameterReference, IValueContainer<Vector3>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値。
		/// </summary>
#else
		/// <summary>
		/// Value of the parameter
		/// </summary>
#endif
		public Vector3 value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetVector3();
				}

				return Vector2.zero;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetVector3(value);
				}
			}
		}

		Vector3 IValueGetter<Vector3>.GetValue()
		{
			return value;
		}

		void IValueSetter<Vector3>.SetValue(Vector3 value)
		{
			this.value = value;
		}
	}
}
