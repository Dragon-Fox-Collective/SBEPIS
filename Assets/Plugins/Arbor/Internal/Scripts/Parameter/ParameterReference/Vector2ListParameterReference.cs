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
	/// Vector2Listパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Vector2List parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Vector2List)]
	public sealed class Vector2ListParameterReference : ParameterReference, IValueContainer<IList<Vector2>>
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
		public IList<Vector2> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetVector2List();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetVector2List(value);
				}
			}
		}

		IList<Vector2> IValueGetter<IList<Vector2>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<Vector2>>.SetValue(IList<Vector2> value)
		{
			this.value = value;
		}
	}
}