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
	/// Vector3IntListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Vector3IntList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Vector3IntList)]
	public sealed class Vector3IntListParameterReference : ParameterReference, IValueContainer<IList<Vector3Int>>
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
		public IList<Vector3Int> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetVector3IntList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetVector3IntList(value);
				}
			}
		}

		IList<Vector3Int> IValueGetter<IList<Vector3Int>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<Vector3Int>>.SetValue(IList<Vector3Int> value)
		{
			this.value = value;
		}
	}
}