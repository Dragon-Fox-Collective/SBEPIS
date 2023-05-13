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
	/// Vector2Intパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Vector2Int parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Vector2Int)]
	public sealed class Vector2IntParameterReference : ParameterReference, IValueContainer<Vector2Int>
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
		public Vector2Int value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetVector2Int();
				}

				return Vector2Int.zero;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetVector2Int(value);
				}
			}
		}

		Vector2Int IValueGetter<Vector2Int>.GetValue()
		{
			return value;
		}

		void IValueSetter<Vector2Int>.SetValue(Vector2Int value)
		{
			this.value = value;
		}
	}
}