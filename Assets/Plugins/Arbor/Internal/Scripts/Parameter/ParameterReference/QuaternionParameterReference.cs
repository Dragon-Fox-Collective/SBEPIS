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
	/// Quaternionパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Quaternion parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Quaternion)]
	public sealed class QuaternionParameterReference : ParameterReference, IValueContainer<Quaternion>
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
		public Quaternion value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetQuaternion();
				}

				return Quaternion.identity;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetQuaternion(value);
				}
			}
		}

		Quaternion IValueGetter<Quaternion>.GetValue()
		{
			return value;
		}

		void IValueSetter<Quaternion>.SetValue(Quaternion value)
		{
			this.value = value;
		}
	}
}
