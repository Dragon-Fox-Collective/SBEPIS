//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Utilities
{
	public static class PhysicsUtility
	{
		public static void CheckReuseCollision(OutputSlotCollision slot)
		{
			if (Physics.reuseCollisionCallbacks && slot!=null && slot.branchCount > 0)
			{
				Debug.LogWarning("Collision is set to be reused.\nPlease disable the \"Reuse Collision Callbacks\" of Physics Settings.");
			}
		}
	}
}