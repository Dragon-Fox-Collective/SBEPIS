//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.BehaviourTree;

#if ARBOR_DOC_JA
	/// <summary>
	/// BehaviourTreeを停止する
	/// </summary>
#else
	/// <summary>
	/// Stop BehaviourTree
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BehaviourTree/StopBehaviourTree")]
	[BuiltInBehaviour]
	public sealed class StopBehaviourTree : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 再生停止するBehaviourTree
		/// </summary>
#else
		/// <summary>
		/// BehaviourTree to stop playback
		/// </summary>
#endif
		[SlotType(typeof(BehaviourTree))]
		[SerializeField]
		private FlexibleComponent _BehaviourTree = new FlexibleComponent();

		// Use this for enter state
		public override void OnStateBegin()
		{
			BehaviourTree behaviourTree = _BehaviourTree.value as BehaviourTree;
			if (behaviourTree != null)
			{
				behaviourTree.Stop();
			}
		}
	}
}