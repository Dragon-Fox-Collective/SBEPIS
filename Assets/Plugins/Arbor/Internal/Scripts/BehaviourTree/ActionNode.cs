//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// アクションを実行するノード
	/// </summary>
#else
	/// <summary>
	/// The node that executes the action
	/// </summary>
#endif
	[System.Serializable]
	public sealed class ActionNode : TreeBehaviourNode
	{
		internal ActionNode(NodeGraph nodeGraph, int nodeID, System.Type classType)
			: base(nodeGraph, nodeID)
		{
			name = "New Action";
			CreateActionBehaviour(classType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ActionBehaviourを作成する。エディタで使用する。
		/// </summary>
		/// <param name="classType">ActionBehaviourの型</param>
		/// <returns>作成したActionBehaviourを返す。</returns>
#else
		/// <summary>
		/// Create a ActionBehaviour. Use it in the editor.
		/// </summary>
		/// <param name="classType">ActionBehaviour type</param>
		/// <returns>Returns the created ActionBehaviour.</returns>
#endif
		public ActionBehaviour CreateActionBehaviour(System.Type classType)
		{
			ActionBehaviour behaviour = ActionBehaviour.Create(this, classType);
			SetBehaviour(behaviour);
			return behaviour;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行する際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when executing.
		/// </summary>
#endif
		protected override void OnExecute()
		{
			ActionBehaviour actionBehaviour = behaviour as ActionBehaviour;
			if (actionBehaviour == null)
			{
				FinishExecute(false);
				return;
			}

			actionBehaviour.CallExecuteInternal();
		}
	}
}