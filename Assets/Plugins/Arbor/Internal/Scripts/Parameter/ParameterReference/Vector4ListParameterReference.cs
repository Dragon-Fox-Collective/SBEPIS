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
	/// Vector4Listパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Vector4List parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Vector4List)]
	public sealed class Vector4ListParameterReference : ParameterReference, IValueContainer<IList<Vector4>>
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
		public IList<Vector4> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetVector4List();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetVector4List(value);
				}
			}
		}

		IList<Vector4> IValueGetter<IList<Vector4>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<Vector4>>.SetValue(IList<Vector4> value)
		{
			this.value = value;
		}
	}
}