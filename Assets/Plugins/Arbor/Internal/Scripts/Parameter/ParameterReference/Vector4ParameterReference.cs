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
	/// Vector4パラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Vector4 parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Vector4)]
	public sealed class Vector4ParameterReference : ParameterReference, IValueContainer<Vector4>
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
		public Vector4 value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetVector4();
				}

				return Vector2.zero;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetVector4(value);
				}
			}
		}

		Vector4 IValueGetter<Vector4>.GetValue()
		{
			return value;
		}

		void IValueSetter<Vector4>.SetValue(Vector4 value)
		{
			this.value = value;
		}
	}
}
