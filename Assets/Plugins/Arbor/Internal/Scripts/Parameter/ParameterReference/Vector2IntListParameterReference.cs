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
	/// Vector2IntListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Vector2IntList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Vector2IntList)]
	public sealed class Vector2IntListParameterReference : ParameterReference, IValueContainer<IList<Vector2Int>>
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
		public IList<Vector2Int> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetVector2IntList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetVector2IntList(value);
				}
			}
		}

		IList<Vector2Int> IValueGetter<IList<Vector2Int>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<Vector2Int>>.SetValue(IList<Vector2Int> value)
		{
			this.value = value;
		}
	}
}