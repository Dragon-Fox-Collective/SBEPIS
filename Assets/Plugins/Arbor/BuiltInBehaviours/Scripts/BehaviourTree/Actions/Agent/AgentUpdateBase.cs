//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;


namespace Arbor.BehaviourTree.Actions
{
	[AddComponentMenu("")]
	[HideBehaviour]
	public abstract class AgentUpdateBase : AgentBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 終了フラグ
		/// </summary>
#else
		/// <summary>
		/// Finish flag
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(AgentFinishFlags))]
		private FlexibleAgentFinishFlags _FinishFlags = new FlexibleAgentFinishFlags(AgentFinishFlags.OnDone);

#if ARBOR_DOC_JA
		/// <summary>
		/// アクション終了時に停止するかどうか
		/// </summary>
#else
		/// <summary>
		/// Whether to stop at the end of the action
		/// </summary>
#endif
		[SerializeField] private FlexibleBool _StopOnEnd = new FlexibleBool(true);

#if ARBOR_DOC_JA
		/// <summary>
		/// 停止時に速度をクリアするかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to clear velocity when stopped.
		/// </summary>
#endif
		[SerializeField] private FlexibleBool _ClearVelocityOnStop = new FlexibleBool(false);

		#endregion // Serialize fields

		protected abstract bool OnUpdateAgent(AgentController agentController);

		protected virtual bool IsDone(AgentController agentController)
		{
			return agentController.isDone;
		}

		protected sealed override void OnExecute()
		{
			AgentController agentController = cachedAgentController;
			if (agentController != null)
			{
				var finishFlags = _FinishFlags.value;

				if (!OnUpdateAgent(agentController) && (finishFlags & AgentFinishFlags.OnCantMove) != 0)
				{
					FinishExecute((finishFlags & AgentFinishFlags.ReturnSuccessOnCantMove) != 0);
					return;
				}

				if (IsDone(agentController) && (finishFlags & AgentFinishFlags.OnDone) != 0)
				{
					FinishExecute(true);
				}
			}
			else
			{
				FinishExecute(false);
			}
		}

		protected override void OnEnd()
		{
			base.OnEnd();

			AgentController agentController = cachedAgentController;
			if (_StopOnEnd.value && agentController != null)
			{
				agentController.Stop(_ClearVelocityOnStop.value);
			}
		}
	}
}