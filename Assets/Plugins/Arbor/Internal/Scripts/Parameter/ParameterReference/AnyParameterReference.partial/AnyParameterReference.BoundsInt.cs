//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.Extensions;

	public sealed partial class AnyParameterReference
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsInt型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of BoundsInt type.
		/// </summary>
#endif
		public BoundsInt boundsIntValue
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetBoundsInt();
				}

				return BoundsIntExtensions.zero;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetBoundsInt(value);
				}
			}
		}
	}
}