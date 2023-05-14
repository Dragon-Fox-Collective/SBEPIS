//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// RaycastHit型の出力スロット
	/// </summary>
#else
	/// <summary>
	/// RaycastHit type of output slot
	/// </summary>
#endif
	[System.Serializable]
	public sealed class OutputSlotRaycastHit : OutputSlot<RaycastHit>
	{
	}
}