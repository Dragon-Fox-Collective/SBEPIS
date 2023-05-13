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
		/// Vector2Int型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Vector2Int type.
		/// </summary>
#endif
		public Vector2Int vector2IntValue
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
	}
}