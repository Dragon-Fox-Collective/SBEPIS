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
	/// OnCollisionExitが呼び出された際、相手のGameObjectを破棄する。
	/// </summary>
#else
	/// <summary>
	/// When OnCollisionExit is called, it will destroy an opponent GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnCollisionExitDestroy")]
	[BuiltInBehaviour]
	public sealed class OnCollisionExitDestroy : CheckTagBehaviourBase
	{
		void OnCollisionExit(Collision collision)
		{
			if (!enabled)
			{
				return;
			}

			GameObject target = collision.gameObject;

			if (CheckTag(target))
			{
				ObjectPool.Destroy(target);
			}
		}
	}
}
