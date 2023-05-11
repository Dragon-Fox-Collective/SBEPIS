//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

using Arbor.ObjectPooling;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// OnControllerColliderHitが呼び出された際、相手のGameObjectを破棄する。
	/// </summary>
#else
	/// <summary>
	/// When OnControllerColliderHit is called, it will destroy an opponent GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnControllerColliderHitDestroy")]
	[BuiltInBehaviour]
	public sealed class OnControllerColliderHitDestroy : CheckTagBehaviourBase
	{
		void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (!enabled)
			{
				return;
			}

			GameObject target = hit.gameObject;

			if (CheckTag(target))
			{
				ObjectPool.Destroy(target);
			}
		}
	}
}
