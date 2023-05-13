//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.ValueFlow
{
	internal class EnumMediator<T> : ValueMediator<T> where T : struct, System.Enum
	{
		public override IValueContainer CreateContainer()
		{
			return new EnumContainer<T>();
		}
	}
}