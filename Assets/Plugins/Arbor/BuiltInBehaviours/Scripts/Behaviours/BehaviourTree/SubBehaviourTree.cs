//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.BehaviourTree;
	using Arbor.Playables;

#if ARBOR_DOC_JA
	/// <summary>
	/// 子階層のBehaviourTreeを再生する
	/// </summary>
	/// <param name="OpenButton" order="-1">子階層のBehaviourTreeを開く</param>
	/// <param name="RestartOnFinish" order="10">終了時に再開するフラグ。</param>
	/// <param name="ExecutionSettings" order="11">実行に関する設定。</param>
#else
	/// <summary>
	/// Play a child hierarchy Behavior Tree
	/// </summary>
	/// <param name="OpenButton" order="-1">Open Behavior Tree of child hierarchy</param>
	/// <param name="RestartOnFinish" order="10">Flag to restart at finish.</param>
	/// <param name="ExecutionSettings" order="11">Settings related to execution.</param>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BehaviourTree/SubBehaviourTree")]
	[BuiltInBehaviour]
	[ExcludeFromPreset]
	public sealed class SubBehaviourTree : StateBehaviour, INodeGraphContainer, ISubGraphBehaviour
	{
		/// <summary>
		/// Behaviour Tree
		/// </summary>
		[SerializeField]
		[HideInInspector]
		private BehaviourTree _BehaviourTree;

#if ARBOR_DOC_JA
		/// <summary>
		/// グラフの引数(パラメータがある場合のみ)<br/>
		/// <list type="bullet">
		/// <item><description>+ボタンから、パラメータを選択して作成。</description></item>
		/// <item><description>パラメータを選択し、-ボタンをクリックで削除。</description></item>
		/// </list>
		/// </summary>
#else
		/// <summary>
		/// Arguments of the graph(Only when there are parameters)<br/>
		/// <list type="bullet">
		/// <item><description>From the + button, select the parameter to create.</description></item>
		/// <item><description>Select the parameter and delete it by clicking the - button.</description></item>
		/// </list>
		/// </summary>
#endif
		[SerializeField]
		private GraphArgumentList _ArgumentList = new GraphArgumentList();

#if ARBOR_DOC_JA
		/// <summary>
		/// 成功時の遷移<br />
		/// 遷移メソッド : OnFinishNodeGraph
		/// </summary>
#else
		/// <summary>
		/// Transition on success<br />
		/// Transition Method : OnFinishNodeGraph
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentOrder(20)]
		private StateLink _SuccessLink = new StateLink();

#if ARBOR_DOC_JA
		/// <summary>
		/// 失敗時の遷移<br />
		/// 遷移メソッド : OnFinishNodeGraph
		/// </summary>
#else
		/// <summary>
		/// Transition on failure<br />
		/// Transition Method : OnFinishNodeGraph
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentOrder(21)]
		private StateLink _FailureLink = new StateLink();

		public BehaviourTree subBT
		{
			get
			{
				return _BehaviourTree;
			}
		}

		protected override void OnCreated()
		{
			BehaviourTree behaviourTree = NodeGraph.Create<BehaviourTree>(gameObject);

			ComponentUtility.RecordObject(behaviourTree, "Add BehaviourTree");
#if !ARBOR_DEBUG
			behaviourTree.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
#endif
			behaviourTree.playOnStart = false;
			behaviourTree.updateSettings.type = UpdateType.Manual;
			behaviourTree.ownerBehaviour = this;
			behaviourTree.enabled = false;

			ComponentUtility.RecordObject(this, "Add BehaviourTree");
			_BehaviourTree = behaviourTree;
		}

		protected override void OnPreDestroy()
		{
			if (_BehaviourTree != null)
			{
				NodeGraph.Destroy(_BehaviourTree);
			}
		}

		void OnEnable()
		{
			if (_BehaviourTree != null && _BehaviourTree.playState != PlayState.Stopping)
			{
				_BehaviourTree.enabled = true;
			}
		}

		void OnDisable()
		{
			if (_BehaviourTree != null && _BehaviourTree.playState != PlayState.Stopping)
			{
				_BehaviourTree.enabled = false;
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_BehaviourTree != null)
			{
				_ArgumentList.UpdateInput(_BehaviourTree, GraphArgumentUpdateTiming.Enter);

				_BehaviourTree.enabled = true;
				_BehaviourTree.Play();
			}
		}

		public override void OnStateUpdate()
		{
			if (_BehaviourTree != null)
			{
				_ArgumentList.UpdateInput(_BehaviourTree, GraphArgumentUpdateTiming.Execute);

				_BehaviourTree.ExecuteUpdate();
			}
		}

		public override void OnStateLateUpdate()
		{
			if (_BehaviourTree != null)
			{
				_ArgumentList.UpdateInput(_BehaviourTree, GraphArgumentUpdateTiming.Execute);

				_BehaviourTree.ExecuteLateUpdate();
			}
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			if (_BehaviourTree != null)
			{
				_BehaviourTree.Stop();
				_BehaviourTree.enabled = false;

				_ArgumentList.UpdateOutput(_BehaviourTree);
			}
		}

		protected override void OnGraphPause()
		{
			if (_BehaviourTree != null)
			{
				_BehaviourTree.Pause();
			}
		}

		protected override void OnGraphResume()
		{
			if (_BehaviourTree != null)
			{
				_BehaviourTree.Resume();
			}
		}

		protected override void OnGraphStop()
		{
			if (_BehaviourTree != null)
			{
				_BehaviourTree.Stop();
				_BehaviourTree.enabled = false;
			}
		}

		int INodeGraphContainer.GetNodeGraphCount()
		{
			return 1;
		}

		T INodeGraphContainer.GetNodeGraph<T>(int index)
		{
			return _BehaviourTree as T;
		}

		void INodeGraphContainer.SetNodeGraph(int index, NodeGraph graph)
		{
			_BehaviourTree = graph as BehaviourTree;
		}

		void INodeGraphContainer.OnFinishNodeGraph(NodeGraph graph, bool success)
		{
			if (_BehaviourTree != null)
			{
				_BehaviourTree.Stop();
				_BehaviourTree.enabled = false;
			}

			if (success)
			{
				Transition(_SuccessLink);
			}
			else
			{
				Transition(_FailureLink);
			}
		}

		bool ISubGraphBehaviour.isExternal
		{
			get
			{
				return false;
			}
		}

		NodeGraph ISubGraphBehaviour.GetSubGraph()
		{
			return _BehaviourTree;
		}
	}
}