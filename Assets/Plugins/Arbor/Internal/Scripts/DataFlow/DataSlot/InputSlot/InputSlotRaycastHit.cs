﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// RaycastHit型の入力スロット
	/// </summary>
#else
	/// <summary>
	/// RaycastHit type of input slot
	/// </summary>
#endif
	[System.Serializable]
	public sealed class InputSlotRaycastHit : InputSlot<RaycastHit>
	{
	}
}