//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Collider2D型の入力スロット
	/// </summary>
#else
	/// <summary>
	/// Collider2D type of input slot
	/// </summary>
#endif
	[System.Serializable]
	public sealed class InputSlotCollider2D : InputSlotComponent<Collider2D>
	{
	}
}