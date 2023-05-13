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
		/// Quaternion型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Quaternion type.
		/// </summary>
#endif
		public Quaternion quaternionValue
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetQuaternion();
				}

				return Quaternion.identity;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetQuaternion(value);
				}
			}
		}
	}
}