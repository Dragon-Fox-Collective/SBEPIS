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
	/// OnTriggerEnterが呼び出された際、相手のGameObjectを破棄する。
	/// </summary>
#else
	/// <summary>
	/// When OnTriggerEnter is called, it will destroy an opponent GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnTriggerEnterDestroy")]
	[BuiltInBehaviour]
	public sealed class OnTriggerEnterDestroy : CheckTagBehaviourBase
	{
		void OnTriggerEnter(Collider collider)
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
