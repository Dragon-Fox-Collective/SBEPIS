//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Ray型の入力スロット
	/// </summary>
#else
	/// <summary>
	/// Ray type of input slot
	/// </summary>
#endif
	[System.Serializable]
	public sealed class InputSlotRay : InputSlot<Ray>
	{
	}
}