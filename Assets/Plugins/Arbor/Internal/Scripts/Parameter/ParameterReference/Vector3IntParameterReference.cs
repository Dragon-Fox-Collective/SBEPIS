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
	/// Vector3Intパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Vector3Int parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Vector3Int)]
	public sealed class Vector3IntParameterReference : ParameterReference, IValueContainer<Vector3Int>
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
		public Vector3Int value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetVector3Int();
				}

				return Vector3Int.zero;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetVector3Int(value);
				}
			}
		}

		Vector3Int IValueGetter<Vector3Int>.GetValue()
		{
			return value;
		}

		void IValueSetter<Vector3Int>.SetValue(Vector3Int value)
		{
			this.value = value;
		}
	}
}