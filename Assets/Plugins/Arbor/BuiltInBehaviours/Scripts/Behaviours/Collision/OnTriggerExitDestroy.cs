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
	/// OnTriggerExitが呼ばれた際に、相手のGameObjectを破棄する。
	/// </summary>
#else
	/// <summary>
	/// When OnTriggerExit is called, it will destroy an opponent GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnTriggerExitDestroy")]
	[BuiltInBehaviour]
	public sealed class OnTriggerExitDestroy : CheckTagBehaviourBase
	{
		void OnTriggerExit(Collider collider)
		{
			if (!enabled)
			{
				return;
			}

			GameObject target = collider.gameObject;

			if (CheckTag(target))
			{
				ObjectPool.Destroy(target);
			}
		}
	}
}
