//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public sealed partial class AnyParameterReference
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Vector3Int type.
		/// </summary>
#endif
		public Vector3Int vector3IntValue
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
	}
}