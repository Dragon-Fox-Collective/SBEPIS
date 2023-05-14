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
	/// Vector3Listパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Vector3List parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Vector3List)]
	public sealed class Vector3ListParameterReference : ParameterReference, IValueContainer<IList<Vector3>>
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
		public IList<Vector3> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetVector3List();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetVector3List(value);
				}
			}
		}

		IList<Vector3> IValueGetter<IList<Vector3>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<Vector3>>.SetValue(IList<Vector3> value)
		{
			this.value = value;
		}
	}
}