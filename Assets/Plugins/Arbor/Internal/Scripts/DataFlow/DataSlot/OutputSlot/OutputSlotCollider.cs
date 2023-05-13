//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Collider型の出力スロット
	/// </summary>
#else
	/// <summary>
	/// Collider type of output slot
	/// </summary>
#endif
	[System.Serializable]
	public sealed class OutputSlotCollider : OutputSlotComponent<Collider>
	{
	}
}