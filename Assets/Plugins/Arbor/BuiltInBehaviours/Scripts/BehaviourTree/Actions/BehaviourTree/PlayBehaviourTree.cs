//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ArborFSMを再生する。
	/// </summary>
#else
	/// <summary>
	/// Play ArborFSM
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BehaviourTree/PlayBehaviourTree")]
	[BuiltInBehaviour]
	public sealed class PlayBehaviourTree : ActionBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 再生開始するBehaviourTree
		/// </summary>
#else
		/// <summary>
		/// Start playback BehaviourTree
		/// </summary>
#endif
		[SlotType(typeof(BehaviourTree))]
		[SerializeField]
		private FlexibleComponent _BehaviourTree = new FlexibleComponent();

		protected override void OnExecute()
		{
			BehaviourTree behaviourTree = _BehaviourTree.value as BehaviourTree;
			if (behaviourTree != null)
			{
				behaviourTree.Play();
				FinishExecute(true);
			}
			else
			{
				FinishExecute(false);
			}
		}
	}
}