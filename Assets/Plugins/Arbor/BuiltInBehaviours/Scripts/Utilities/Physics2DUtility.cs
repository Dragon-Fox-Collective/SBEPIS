//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Utilities
{
	public static class Physics2DUtility
	{
		public static void CheckReuseCollision2D(OutputSlotCollision2D slot)
		{
			if (Physics2D.reuseCollisionCallbacks && slot != null && slot.branchCount > 0)
			{
				Debug.LogWarning("Collision2D is set to be reused.\nPlease disable the \"Reuse Collision Callbacks\" of Physics2D Settings.");
			}
		}
	}
}