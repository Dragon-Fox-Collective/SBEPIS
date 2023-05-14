//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Collision2D型の出力スロット
	/// </summary>
#else
	/// <summary>
	/// Collision2D type of output slot
	/// </summary>
#endif
	[System.Serializable]
	public sealed class OutputSlotCollision2D : OutputSlot<Collision2D>
	{
	}
}