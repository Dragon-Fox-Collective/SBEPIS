//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
	using Arbor.Playables;

#if ARBOR_DOC_JA
	/// <summary>
	/// 子階層のArborFSMを再生する。
	/// </summary>
	/// <param name="OpenButton" order="-1">子階層のArborFSMを開く</param>
#else
	/// <summary>
	/// Play a child hierarchy ArborFSM.
	/// </summary>
	/// <param name="OpenButton" order="-1">Open Arbor FSM of child hierarchy</param>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("StateMachine/SubStateMachine")]
	[BuiltInBehaviour]
	[ExcludeFromPreset]
	public sealed class SubStateMachine : ActionBehaviour, INodeGraphContainer, ISubGraphBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private ArborFSM _StateMachine;

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

		private bool _IsFinished = false;
		private bool _Success = false;

		public ArborFSM subFSM
		{
			get
			{
				return _StateMachine;
			}
		}

		protected override void OnCreated()
		{
			ArborFSM stateMachine = NodeGraph.Create<ArborFSM>(gameObject);

			ComponentUtility.RecordObject(stateMachine, "Add ArborFSM");
#if !ARBOR_DEBUG
			stateMachine.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
#endif
			stateMachine.playOnStart = false;
			stateMachine.updateSettings.type = UpdateType.Manual;
			stateMachine.ownerBehaviour = this;
			stateMachine.enabled = false;

			ComponentUtility.RecordObject(this, "Add ArborFSM");
			_StateMachine = stateMachine;
		}

		protected override void OnPreDestroy()
		{
			if (_StateMachine != null)
			{
				NodeGraph.Destroy(_StateMachine);
			}
		}

		void OnEnable()
		{
			if (_StateMachine != null && _StateMachine.playState != PlayState.Stopping)
			{
				_StateMachine.enabled = true;
			}
		}

		void OnDisable()
		{
			if (_StateMachine != null && _StateMachine.playState != PlayState.Stopping)
			{
				_StateMachine.enabled = false;
			}
		}

		protected override void OnStart()
		{
			_IsFinished = false;
			if (_StateMachine != null)
			{
				_ArgumentList.UpdateInput(_StateMachine, GraphArgumentUpdateTiming.Enter);

				_StateMachine.enabled = true;
				_StateMachine.Play();
			}
		}

		protected override void OnExecute()
		{
			if (_StateMachine != null)
			{
				_ArgumentList.UpdateInput(_StateMachine, GraphArgumentUpdateTiming.Execute);

				_StateMachine.ExecuteUpdate(true);

				if (_IsFinished)
				{
					_StateMachine.Stop();
					_StateMachine.enabled = false;

					FinishExecute(_Success);
				}
			}
		}

		protected override void OnEnd()
		{
			if (_StateMachine != null)
			{
				_StateMachine.Stop();
				_StateMachine.enabled = false;

				_ArgumentList.UpdateOutput(_StateMachine);
			}
		}

		protected override void OnGraphPause()
		{
			if (_StateMachine != null)
			{
				_StateMachine.Pause();
			}
		}

		protected override void OnGraphResume()
		{
			if (_StateMachine != null)
			{
				_StateMachine.Resume();
			}
		}

		protected override void OnGraphStop()
		{
			if (_StateMachine != null)
			{
				_StateMachine.Stop();
				_StateMachine.enabled = false;
			}
		}

		int INodeGraphContainer.GetNodeGraphCount()
		{
			return 1;
		}

		T INodeGraphContainer.GetNodeGraph<T>(int index)
		{
			return _StateMachine as T;
		}

		void INodeGraphContainer.SetNodeGraph(int index, NodeGraph graph)
		{
			_StateMachine = graph as ArborFSM;
		}

		void INodeGraphContainer.OnFinishNodeGraph(NodeGraph graph, bool success)
		{
			_IsFinished = true;
			_Success = success;
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
			return _StateMachine;
		}
	}
}