//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
	[AddComponentMenu("")]
	[HideBehaviour()]
	public abstract class AgentUpdateBase : AgentBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートから抜けるときに停止するかどうか
		/// </summary>
#else
		/// <summary>
		/// Whether to stop when leaving the state.
		/// </summary>
#endif
		[SerializeField]
		protected FlexibleBool _StopOnStateEnd = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// 停止するときに速度をクリアするかどうか
		/// </summary>
#else
		/// <summary>
		/// Whether to clear velocity when stopping
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _ClearVelocityOnStop = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動完了した時のステート遷移<br />
		/// 遷移メソッド : OnStateUpdate
		/// </summary>
#else
		/// <summary>
		/// State transition at the time of movement completion<br />
		/// Transition Method : OnStateUpdate
		/// </summary>
#endif
		[Internal.DocumentOrder(1000)]
		[SerializeField] private StateLink _Done = new StateLink();

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動先が見つからずに移動できなかった場合のステート遷移。<br />
		/// 遷移メソッド : OnStateUpdate
		/// </summary>
#else
		/// <summary>
		/// State transition when the destination cannot be found and cannot be moved.<br/>
		/// Transition method: OnStateUpdate
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentOrder(1001)]
		private StateLink _CantMove = new StateLink();

		#endregion // Serialize fields

		public override void OnStateBegin()
		{
			base.OnStateBegin();

			UpdateAgent();
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			base.OnStateEnd();

			AgentController agentController = cachedAgentController;
			if (_StopOnStateEnd.value && agentController != null)
			{
				agentController.Stop(_ClearVelocityOnStop.value);
			}
		}

		protected abstract bool OnUpdateAgent(AgentController agentController);

		protected virtual void OnDone()
		{
			Transition(_Done);
		}

		protected virtual bool OnCantMove()
		{
			return Transition(_CantMove);
		}

		protected virtual bool IsDone(AgentController agentController)
		{
			return agentController.isDone;
		}

		void UpdateAgent()
		{
			AgentController agentController = cachedAgentController;
			if (agentController != null)
			{
				if (!OnUpdateAgent(agentController))
				{
					if (OnCantMove())
					{
						return;
					}
				}

				if (IsDone(agentController))
				{
					OnDone();
				}
			}
		}

		// Update is called once per frame
		public sealed override void OnStateUpdate()
		{
			UpdateAgent();
		}
	}
}