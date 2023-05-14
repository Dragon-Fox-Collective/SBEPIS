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
	/// Vector2パラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Vector2 parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Vector2)]
	public sealed class Vector2ParameterReference : ParameterReference, IValueContainer<Vector2>
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
		public Vector2 value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetVector2();
				}

				return Vector2.zero;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetVector2(value);
				}
			}
		}

		Vector2 IValueGetter<Vector2>.GetValue()
		{
			return value;
		}

		void IValueSetter<Vector2>.SetValue(Vector2 value)
		{
			this.value = value;
		}
	}
}
