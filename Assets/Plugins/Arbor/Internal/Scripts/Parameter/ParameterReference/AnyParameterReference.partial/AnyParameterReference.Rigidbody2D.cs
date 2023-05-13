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
		/// Rigidbody2D型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Rigidbody2D type.
		/// </summary>
#endif
		public Rigidbody2D rigidbody2DValue
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetRigidbody2D();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetRigidbody2D(value);
				}
			}
		}
	}
}