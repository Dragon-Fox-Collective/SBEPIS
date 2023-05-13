//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// QuaternionListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference QuaternionList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.QuaternionList)]
	public sealed class QuaternionListParameterReference : ParameterReference, IValueContainer<IList<Quaternion>>
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
		public IList<Quaternion> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetQuaternionList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetQuaternionList(value);
				}
			}
		}

		IList<Quaternion> IValueGetter<IList<Quaternion>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<Quaternion>>.SetValue(IList<Quaternion> value)
		{
			this.value = value;
		}
	}
}