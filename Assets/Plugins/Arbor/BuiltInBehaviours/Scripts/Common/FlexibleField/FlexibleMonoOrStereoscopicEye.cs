//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
	[System.Serializable]
	public class FlexibleMonoOrStereoscopicEye : FlexibleField<Camera.MonoOrStereoscopicEye>
	{
		public FlexibleMonoOrStereoscopicEye()
		{
		}

		public FlexibleMonoOrStereoscopicEye(Camera.MonoOrStereoscopicEye value) : base(value)
		{
		}

		public FlexibleMonoOrStereoscopicEye(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleMonoOrStereoscopicEye(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator Camera.MonoOrStereoscopicEye(FlexibleMonoOrStereoscopicEye flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleMonoOrStereoscopicEye(Camera.MonoOrStereoscopicEye value)
		{
			return new FlexibleMonoOrStereoscopicEye(value);
		}
	}
}