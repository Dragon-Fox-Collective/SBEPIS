//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.Playables;

#pragma warning disable 1574
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="Arbor.ArborFSM" />の内部クラス。
	/// 実際にGameObjectにアタッチするには<see cref="Arbor.ArborFSM" />を使用する。
	/// </summary>
#else
	/// <summary>
	/// Internal class of <see cref="Arbor.ArborFSM" />.
	/// To actually attach to GameObject is to use the <see cref = "Arbor.ArborFSM" />.
	/// </summary>
#endif
#pragma warning restore 1574
	[AddComponentMenu("")]
	public abstract class ArborFSMInternal : NodeGraph
	{
		#region Serialize fields

		[SerializeField]
		[HideInInspector]
		private int _StartStateID;

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private List<State> _States = new List<State>();

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private StateLinkRerouteNodeList _StateLinkRerouteNodes = new StateLinkRerouteNodeList();

		#endregion // Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// SendTriggerFlagsの全てが有効なフラグ
		/// </summary>
#else
		/// <summary>
		/// All flags in SendTriggerFlags are valid
		/// </summary>
#endif
		public const SendTriggerFlags allSendTrigger = (SendTriggerFlags)(-1);

		sealed class Trigger
		{
			public string message;
			public SendTriggerFlags flags = allSendTrigger;
		}

		[System.NonSerialized]
		private State _CurrentState = null;

		[System.NonSerialized]
		private State _ReservedState = null;

		[System.NonSerialized]
		private StateLink _ReservedStateLink = null;

		[System.NonSerialized]
		private State _PrevTransitionState = null;

		[System.NonSerialized]
		private State _NextTransitionState = null;

		[System.NonSerialized]
		private TransitionTiming _ReservedStateTransitionTiming;

		private long _StateLinkHistoryCount = 0L;

		private bool _InStateEvent = false;
		private bool _ChangingState = false;
		private bool _IsBreaked = false;
		private List<Trigger> _Triggers = new List<Trigger>();
		private Queue<List<Trigger>> _TriggersBuffers = new Queue<List<Trigger>>(new[] {
			new List<Trigger>(),
			new List<Trigger>(),
			new List<Trigger>(),
			new List<Trigger>(),
			new List<Trigger>()}
		);

		private bool _IsInfiniteLoopWarning = false;
		private int _TransitionCounter = 0;

		[System.NonSerialized]
		private List<State> _ResidentStates = new List<State>();

		[System.NonSerialized]
		private Dictionary<int, State> _DicStates = new Dictionary<int, State>();

#if ARBOR_DOC_JA
		/// <summary>
		/// FSMの名前。<br/>
		/// 一つのGameObjectに複数のFSMがある場合の識別や検索に使用する。
		/// </summary>
#else
		/// <summary>
		/// The FSM name.<br/>
		/// It is used for identification and retrieval when there is more than one FSM in one GameObject.
		/// </summary>
#endif
		[System.Obsolete("use graphName")]
		public string fsmName
		{
			get
			{
				return graphName;
			}
			set
			{
				graphName = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始ステートのIDを取得する。
		/// </summary>
		/// <value>
		/// 開始ステートID。
		/// </value>
#else
		/// <summary>
		/// Gets the start state identifier.
		/// </summary>
		/// <value>
		/// The start state identifier.
		/// </value>
#endif
		public int startStateID
		{
			get
			{
				return _StartStateID;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在の<see cref="Arbor.State" />を取得する。
		/// </summary>
		/// <value>
		/// 現在の<see cref="Arbor.State" />。
		/// </value>
#else
		/// <summary>
		/// Gets <see cref="Arbor.State" /> of the current.
		/// </summary>
		/// <value>
		/// <see cref="Arbor.State" /> of the current.
		/// </value>
#endif
		public State currentState
		{
			get
			{
				return _CurrentState;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 前のステート
		/// </summary>
#else
		/// <summary>
		/// Prev state
		/// </summary>
#endif
		public State prevTransitionState
		{
			get
			{
				return _PrevTransitionState;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 次のステート
		/// </summary>
#else
		/// <summary>
		/// Next state
		/// </summary>
#endif
		public State nextTransitionState
		{
			get
			{
				return _NextTransitionState;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移予約された<see cref="Arbor.State" />を取得する。
		/// </summary>
		/// <value>
		/// 遷移予約された<see cref="Arbor.State" />。
		/// </value>
#else
		/// <summary>
		/// Gets the transition reserved <see cref="Arbor.State" />.
		/// </summary>
		/// <value>
		/// Transition reserved <see cref="Arbor.State" />.
		/// </value>
#endif
		[System.Obsolete("Use reservedState.")]
		public State nextState
		{
			get
			{
				return reservedState;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移予約された<see cref="Arbor.State" />を取得する。
		/// </summary>
		/// <value>
		/// 遷移予約された<see cref="Arbor.State" />。
		/// </value>
#else
		/// <summary>
		/// Gets the transition reserved <see cref="Arbor.State" />.
		/// </summary>
		/// <value>
		/// Transition reserved <see cref="Arbor.State" />.
		/// </value>
#endif
		public State reservedState
		{
			get
			{
				return _ReservedState;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移予約された<see cref="Arbor.StateLink" />を取得する。
		/// </summary>
		/// <value>
		/// 遷移予約された<see cref="Arbor.StateLink" />。
		/// </value>
#else
		/// <summary>
		/// Gets the transition reserved <see cref="Arbor.StateLink" />.
		/// </summary>
		/// <value>
		/// Transition reserved <see cref="Arbor.StateLink" />.
		/// </value>
#endif
		public StateLink reservedStateLink
		{
			get
			{
				return _ReservedStateLink;
			}
		}

		internal bool nextUpdateTransition
		{
			get
			{
				return _ReservedState != null && 
					(_ReservedStateTransitionTiming == TransitionTiming.Immediate ||
					_ReservedStateTransitionTiming == TransitionTiming.NextUpdateDontOverwrite ||
					_ReservedStateTransitionTiming == TransitionTiming.NextUpdateOverwrite);
			}
		}

		internal bool nextLateUpdateTransition
		{
			get
			{
				return _ReservedState != null &&
					(_ReservedStateTransitionTiming == TransitionTiming.Immediate ||
					_ReservedStateTransitionTiming == TransitionTiming.LateUpdateDontOverwrite ||
					_ReservedStateTransitionTiming == TransitionTiming.LateUpdateOverwrite);
			}
		}

		internal bool nextImmediateTransition
		{
			get
			{
				return _ReservedState != null && _ReservedStateTransitionTiming == TransitionTiming.Immediate;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateの数を取得。
		/// </summary>
#else
		/// <summary>
		///  Get a count of State.
		/// </summary>
#endif
		public int stateCount
		{
			get
			{
				return _States.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateLinkRerouteNodeリスト
		/// </summary>
#else
		/// <summary>
		/// StateLinkRerouteNode List
		/// </summary>
#endif
		public StateLinkRerouteNodeList stateLinkRerouteNodes
		{
			get
			{
				return _StateLinkRerouteNodes;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Startメソッドでプレイ開始した際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when playing is started with the Start method.
		/// </summary>
#endif
		protected sealed override void OnPlayOnStart()
		{
			if (_StartingTriggers.Count > 0)
			{
				if (_StartingTriggerFrameCount == Time.frameCount)
				{
					for (int i = 0; i < _StartingTriggers.Count; i++)
					{
						var trigger = _StartingTriggers[i];
						SendTrigger(trigger.message, trigger.flags);
					}
				}
				_StartingTriggers.Clear();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はスクリプトのインスタンスがロードされたときに呼び出される。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the script instance is being loaded.
		/// </summary>
#endif
		protected virtual void Awake()
		{
			for (int i = 0, count = _States.Count; i < count; i++)
			{
				State state = _States[i];
				state.Awake();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プレイ開始した際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when you start playing.
		/// </summary>
#endif
		protected sealed override void OnPlay()
		{
			for (int i = 0, count = _ResidentStates.Count; i < count; i++)
			{
				State state = _ResidentStates[i];
				state.Activate(true);
			}

			BeginTransitionLoop();

			if (!NextState())
			{
				State nextState = GetStateFromID(_StartStateID);
				ChangeState(nextState);
			}

			EndTransitionLoop();
		}

		internal bool _EnterStoppingPlayState
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プレイ停止した際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when play is stopped.
		/// </summary>
#endif
		protected sealed override void OnStop()
		{
			_ReservedState = null;
			_InStateEvent = true;

			for (int i = 0, count = _ResidentStates.Count; i < count; i++)
			{
				State state = _ResidentStates[i];
				state.Stop();
			}

			if (!nextImmediateTransition)
			{
				// don't immediate transition in resident state.
				if (_CurrentState != null)
				{
					_CurrentState.Stop();
				}
			}

			// immediate transition in resident state or currnet state.
			try
			{
				_EnterStoppingPlayState = true;
				NextImmediateState();
			}
			finally
			{
				_EnterStoppingPlayState = false;
			}

			for (int i = 0, count = _ResidentStates.Count; i < count; i++)
			{
				State state = _ResidentStates[i];
				state.Activate(false);
			}

			if (_CurrentState != null)
			{
				_CurrentState.Activate(false);
			}

			_InStateEvent = false;

			ClearTransitionCount(true);
			ClearLinkHistory();

			_CurrentState = null;
			_NextTransitionState = null;
			_ReservedState = null;
			_ReservedStateLink = null;
			_PrevTransitionState = null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ポーズした際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when you pause.
		/// </summary>
#endif
		protected sealed override void OnPause()
		{
			if (_CurrentState != null)
			{
				_CurrentState.Pause();
			}

			for (int i = 0, count = _ResidentStates.Count; i < count; i++)
			{
				State state = _ResidentStates[i];
				state.Pause();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再開した際に呼ばれる
		/// </summary>
#else
		/// <summary>
		/// Called when resuming
		/// </summary>
#endif
		protected sealed override void OnResume()
		{
			if (_CurrentState != null)
			{
				_CurrentState.Resume();
			}

			for (int i = 0, count = _ResidentStates.Count; i < count; i++)
			{
				State state = _ResidentStates[i];
				state.Resume();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// OnEnableメソッドで再開した際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when resumed with the OnEnable method.
		/// </summary>
#endif
		protected sealed override void OnResumeOnEnable()
		{
			BeginTransitionLoop();

			if (!NextState())
			{
				if (_CurrentState == null)
				{
					State nextState = GetStateFromID(_StartStateID);
					ChangeState(nextState);
				}
			}

			EndTransitionLoop();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 更新する際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when updating.
		/// </summary>
#endif
		protected sealed override void OnUpdate()
		{
			using (CalculateScope.OpenScope())
			{
				if (nextUpdateTransition)
				{
					BeginTransitionLoop();

					NextState();

					EndTransitionLoop();
				}

				_DidImmediateTransition = false;

				_InStateEvent = true;

				for (int i = 0, count = _ResidentStates.Count; i < count; i++)
				{
					State state = _ResidentStates[i];
					state.UpdateBehaviours();
				}

				if (_CurrentState != null)
				{
					_CurrentState.UpdateBehaviours();
				}

				_InStateEvent = false;

				if (NextImmediateState())
				{
					_DidImmediateTransition = true;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LateUpdateの際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when LateUpdate.
		/// </summary>
#endif
		protected sealed override void OnLateUpdate()
		{
			using (CalculateScope.OpenScope())
			{
				if (_DidImmediateTransition)
				{
					_DidImmediateTransition = false;
					return;
				}

				_InStateEvent = true;

				for (int i = 0, count = _ResidentStates.Count; i < count; i++)
				{
					State state = _ResidentStates[i];
					state.LateUpdateBehaviours();
				}

				if (_CurrentState != null)
				{
					_CurrentState.LateUpdateBehaviours();
				}

				_InStateEvent = false;

				if (nextLateUpdateTransition)
				{
					BeginTransitionLoop();

					NextState();

					EndTransitionLoop();
				}
			}

			_IsBreaked = false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FixedUpdateの際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when FixedUpdate.
		/// </summary>
#endif
		protected sealed override void OnFixedUpdate()
		{
			using (CalculateScope.OpenScope())
			{
				_InStateEvent = true;

				for (int i = 0, count = _ResidentStates.Count; i < count; i++)
				{
					State state = _ResidentStates[i];
					state.FixedUpdateBehaviours();
				}

				if (_CurrentState != null)
				{
					_CurrentState.FixedUpdateBehaviours();
				}

				_InStateEvent = false;

				NextImmediateState();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したStateLinkによって遷移したヒストリーでのインデックスを取得。
		/// </summary>
		/// <param name="stateLink">取得するStateLink</param>
		/// <returns>ヒストリーのインデックス。-1だと対象外。値が大きいほど古い遷移を指す。</returns>
#else
		/// <summary>
		/// Retrieve the index in the history that transited by the specified StateLink.
		/// </summary>
		/// <param name="stateLink">StateLink to acquire</param>
		/// <returns>Index of history. -1 is not eligible. Larger values indicate older transitions.</returns>
#endif
		public int IndexOfStateLinkHistory(StateLink stateLink)
		{
			if (stateLink.histroyIndex == 0)
			{
				return -1;
			}

			long index = _StateLinkHistoryCount - stateLink.histroyIndex;
			if (index > 5L)
			{
				index = -1L;
			}
			return (int)index;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateをインデックスから取得
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>State</returns>
#else
		/// <summary>
		/// Get State from index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>State</returns>
#endif
		public State GetStateFromIndex(int index)
		{
			return _States[index];
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateのインデックスを取得
		/// </summary>
		/// <param name="state">State</param>
		/// <returns>インデックス。ない場合は-1を返す。</returns>
#else
		/// <summary>
		/// Get State index.
		/// </summary>
		/// <param name="state">State</param>
		/// <returns>Index. If not, it returns -1.</returns>
#endif
		public int GetStateIndex(State state)
		{
			return _States.IndexOf(state);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 全ての<see cref="Arbor.State" />を取得する。
		/// </summary>
		/// <value>
		/// <see cref="Arbor.State" />の配列。
		/// </value>
#else
		/// <summary>
		/// Gets all of <see cref="Arbor.State" />.
		/// </summary>
		/// <value>
		/// Array of <see cref="Arbor.State" />.
		/// </value>
#endif
		[System.Obsolete("use stateCount and GetStateFromIndex()")]
		public State[] states
		{
			get
			{
				return _States.ToArray();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートIDを指定して<see cref="Arbor.State" />を取得する。
		/// </summary>
		/// <param name="stateID">ステートID</param>
		/// <returns>見つかった<see cref="Arbor.State" />。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets <see cref="Arbor.State" /> from the state identifier.
		/// </summary>
		/// <param name="stateID">The state identifier.</param>
		/// <returns>Found <see cref = "Arbor.State" />. Returns null if not found.</returns>
#endif
		public State GetStateFromID(int stateID)
		{
			State result = null;
			if (_DicStates.TryGetValue(stateID, out result))
			{
				return result;
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateLinkを指定して<see cref="Arbor.State" />を取得する。
		/// </summary>
		/// <param name="stateLink">StateLink</param>
		/// <returns>見つかった<see cref="Arbor.State" />。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets <see cref="Arbor.State" /> from the StateLink.
		/// </summary>
		/// <param name="stateLink">StateLink</param>
		/// <returns>Found <see cref = "Arbor.State" />. Returns null if not found.</returns>
#endif
		public State GetState(StateLink stateLink)
		{
			if (stateLink == null)
			{
				return null;
			}

			int targetID = stateLink.stateID;
			while (targetID != 0)
			{
				Node targetNode = GetNodeFromID(targetID);

				State state = targetNode as State;
				if (state != null)
				{
					return state;
				}

				StateLinkRerouteNode stateLinkNode = targetNode as StateLinkRerouteNode;
				if (stateLinkNode == null)
				{
					return null;
				}

				targetID = stateLinkNode.link.stateID;
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートを生成。
		/// </summary>
		/// <param name="nodeID">ノードID</param>
		/// <param name="resident">常駐するかどうかのフラグ。</param>
		/// <param name="types">追加する挙動の型リスト</param>
		/// <returns>生成したステート。ノードIDが重複している場合は生成せずにnullを返す。</returns>
#else
		/// <summary>
		/// Create state.
		/// </summary>
		/// <param name="nodeID">Node ID</param>
		/// <param name="resident">Resident whether flags.</param>
		/// <param name="types">Type list of behaviors to add</param>
		/// <returns>The created state. If the node ID is not unique, return null without creating it.</returns>
#endif
		public State CreateState(int nodeID, bool resident, IList<System.Type> types)
		{
			if (!IsUniqueNodeID(nodeID))
			{
				Debug.LogWarning("CreateState id(" + nodeID + ") is not unique.");
				return null;
			}

			State state = new State(this, nodeID, resident, types);

			ComponentUtility.RecordObject(this, "Created State");

			_States.Add(state);
			RegisterState(state);

			if (!resident && _StartStateID == 0)
			{
				_StartStateID = state.nodeID;
			}

			ComponentUtility.SetDirty(this);

			return state;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートを生成。
		/// </summary>
		/// <param name="nodeID">ノードID</param>
		/// <param name="resident">常駐するかどうかのフラグ。</param>
		/// <returns>生成したステート。ノードIDが重複している場合は生成せずにnullを返す。</returns>
#else
		/// <summary>
		/// Create state.
		/// </summary>
		/// <param name="nodeID">Node ID</param>
		/// <param name="resident">Resident whether flags.</param>
		/// <returns>The created state. If the node ID is not unique, return null without creating it.</returns>
#endif
		public State CreateState(int nodeID, bool resident)
		{
			return CreateState(nodeID, resident, null);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートを生成。
		/// </summary>
		/// <param name="resident">常駐するかどうかのフラグ。</param>
		/// <returns>生成したステート。</returns>
#else
		/// <summary>
		/// Create state.
		/// </summary>
		/// <param name="resident">Resident whether flags.</param>
		/// <returns>The created state.</returns>
#endif
		public State CreateState(bool resident)
		{
			return CreateState(GetUniqueNodeID(), resident);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートを生成。
		/// </summary>
		/// <param name="resident">常駐するかどうかのフラグ。</param>
		/// <param name="types">追加する挙動の型リスト</param>
		/// <returns>生成したステート。</returns>
#else
		/// <summary>
		/// Create state.
		/// </summary>
		/// <param name="resident">Resident whether flags.</param>
		/// <param name="types">Type list of behaviors to add</param>
		/// <returns>The created state.</returns>
#endif
		public State CreateState(bool resident, IList<System.Type> types)
		{
			return CreateState(GetUniqueNodeID(), resident, types);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートを生成。
		/// </summary>
		/// <returns>生成したステート。</returns>
#else
		/// <summary>
		/// Create state.
		/// </summary>
		/// <returns>The created state.</returns>
#endif
		public State CreateState()
		{
			return CreateState(false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートを名前で検索。
		/// </summary>
		/// <param name="stateName">検索するステートの名前。</param>
		/// <returns>見つかったステート。ない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Search state by name.
		/// </summary>
		/// <param name="stateName">The name of the search state.</param>
		/// <returns>Found state. Return null if not.</returns>
#endif
		public State FindState(string stateName)
		{
			for (int i = 0; i < _States.Count; i++)
			{
				var state = _States[i];
				if (state.name == stateName)
				{
					return state;
				}
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートを名前で検索。
		/// </summary>
		/// <param name="stateName">検索するステートの名前。</param>
		/// <returns>見つかったステートの配列。</returns>
#else
		/// <summary>
		/// Search state by name.
		/// </summary>
		/// <param name="stateName">The name of the search state.</param>
		/// <returns>Array of found state.</returns>
#endif
		public State[] FindStates(string stateName)
		{
			List<State> states = new List<State>();
			for (int i = 0; i < _States.Count; i++)
			{
				var state = _States[i];
				if (state.name == stateName)
				{
					states.Add(state);
				}
			}
			return states.ToArray();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourが属しているステートの取得。
		/// </summary>
		/// <param name="behaviour">StateBehaviour</param>
		/// <returns>StateBehaviourが属しているステート。ない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Acquisition of states StateBehaviour belongs.
		/// </summary>
		/// <param name="behaviour">StateBehaviour</param>
		/// <returns>States StateBehaviour belongs. Return null if not.</returns>
#endif
		public State FindStateContainsBehaviour(StateBehaviour behaviour)
		{
			for (int i = 0; i < _States.Count; i++)
			{
				var state = _States[i];
				if (state.Contains(behaviour))
				{
					return state;
				}
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートの削除。
		/// </summary>
		/// <param name="state">削除するステート。</param>
		/// <returns>削除した場合にtrue</returns>
#else
		/// <summary>
		/// Delete state.
		/// </summary>
		/// <param name="state">State that you want to delete.</param>
		/// <returns>true if deleted</returns>
#endif
		public bool DeleteState(State state)
		{
			int stateID = state.nodeID;

			ComponentUtility.RegisterCompleteObjectUndo(this, "Delete Nodes");

			ComponentUtility.RecordObject(this, "Delete Nodes");
			_States.Remove(state);
			UnregisterState(state);

			if (_StartStateID == stateID)
			{
				ComponentUtility.RecordObject(this, "Delete Nodes");
				_StartStateID = 0;
				ComponentUtility.SetDirty(this);
			}

			int stateCount = _States.Count;
			for (int stateIndex = 0; stateIndex < stateCount; stateIndex++)
			{
				State otherState = _States[stateIndex];
				if (otherState != state)
				{
					otherState.DisconnectState(stateID);
				}
			}

			state.DestroyBehaviours();

			ComponentUtility.RecordObject(this, "Delete Nodes");

			int stateLinkCount = _StateLinkRerouteNodes.count;
			for (int stateLinkIndex = 0; stateLinkIndex < stateLinkCount; stateLinkIndex++)
			{
				StateLinkRerouteNode otherStateLink = _StateLinkRerouteNodes[stateLinkIndex];
				if (otherStateLink.link.stateID == stateID)
				{
					otherStateLink.link.stateID = 0;
				}
			}

			ComponentUtility.SetDirty(this);

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateLinkRerouteNodeを作成する。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="nodeID">ノードID</param>
		/// <param name="lineColor">ライン色</param>
		/// <param name="direction">向き</param>
		/// <param name="targetID">接続先ID</param>
		/// <returns>作成したStateLinkRerouteNode</returns>
#else
		/// <summary>
		/// Create StateLinkRerouteNode.
		/// </summary>
		/// <param name="position">Node position</param>
		/// <param name="nodeID">Node ID</param>
		/// <param name="lineColor">Line Color</param>
		/// <param name="direction">Direction</param>
		/// <param name="targetID">Connection destination ID</param>
		/// <returns>The created StateLinkRerouteNode</returns>
#endif
		public StateLinkRerouteNode CreateStateLinkRerouteNode(Vector2 position, int nodeID, Color lineColor, Vector2 direction, int targetID)
		{
			if (!IsUniqueNodeID(nodeID))
			{
				Debug.LogWarning("CreateStateLinkRerouteNode id(" + nodeID + ") is not unique.");
				return null;
			}

			StateLinkRerouteNode stateLinkRerouteNode = new StateLinkRerouteNode(this, nodeID);
			stateLinkRerouteNode.position = new Rect(position.x, position.y, Node.defaultWidth, 0);
			stateLinkRerouteNode.direction = direction;
			stateLinkRerouteNode.link.lineColor = lineColor;
			stateLinkRerouteNode.link.stateID = targetID;

			ComponentUtility.RecordObject(this, "Created StateLinkRerouteNode");

			_StateLinkRerouteNodes.Add(stateLinkRerouteNode);
			RegisterNode(stateLinkRerouteNode);

			ComponentUtility.SetDirty(this);

			return stateLinkRerouteNode;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateLinkRerouteNodeを作成する。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="nodeID">ノードID</param>
		/// <param name="lineColor">ライン色</param>
		/// <returns>作成したStateLinkRerouteNode</returns>
#else
		/// <summary>
		/// Create StateLinkRerouteNode.
		/// </summary>
		/// <param name="position">Node position</param>
		/// <param name="nodeID">Node ID</param>
		/// <param name="lineColor">Line Color</param>
		/// <returns>The created StateLinkRerouteNode</returns>
#endif
		public StateLinkRerouteNode CreateStateLinkRerouteNode(Vector2 position, int nodeID, Color lineColor)
		{
			return CreateStateLinkRerouteNode(position, nodeID, lineColor, StateLinkRerouteNode.kDefaultDirection, 0);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateLinkRerouteNodeを作成する。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="nodeID">ノードID</param>
		/// <returns>作成したStateLinkRerouteNode</returns>
#else
		/// <summary>
		/// Create StateLinkRerouteNode.
		/// </summary>
		/// <param name="position">Node position</param>
		/// <param name="nodeID">Node ID</param>
		/// <returns>The created StateLinkRerouteNode</returns>
#endif
		public StateLinkRerouteNode CreateStateLinkRerouteNode(Vector2 position, int nodeID)
		{
			return CreateStateLinkRerouteNode(position, nodeID, Color.white, StateLinkRerouteNode.kDefaultDirection, 0);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateLinkRerouteNodeを作成する。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="lineColor">ライン色</param>
		/// <param name="direction">向き</param>
		/// <param name="targetID">接続先ID</param>
		/// <returns>作成したStateLinkRerouteNode</returns>
#else
		/// <summary>
		/// Create StateLinkRerouteNode.
		/// </summary>
		/// <param name="position">Node position</param>
		/// <param name="lineColor">Line Color</param>
		/// <param name="direction">Direction</param>
		/// <param name="targetID">Connection destination ID</param>
		/// <returns>The created StateLinkRerouteNode</returns>
#endif
		public StateLinkRerouteNode CreateStateLinkRerouteNode(Vector2 position, Color lineColor, Vector2 direction, int targetID)
		{
			return CreateStateLinkRerouteNode(position, GetUniqueNodeID(), lineColor, direction, targetID);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateLinkRerouteNodeを作成する。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="lineColor">ライン色</param>
		/// <returns>作成したStateLinkRerouteNode</returns>
#else
		/// <summary>
		/// Create StateLinkRerouteNode.
		/// </summary>
		/// <param name="position">Node position</param>
		/// <param name="lineColor">Line Color</param>
		/// <returns>The created StateLinkRerouteNode</returns>
#endif
		public StateLinkRerouteNode CreateStateLinkRerouteNode(Vector2 position, Color lineColor)
		{
			return CreateStateLinkRerouteNode(position, GetUniqueNodeID(), lineColor, StateLinkRerouteNode.kDefaultDirection, 0);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateLinkRerouteNodeを作成する。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <returns>作成したStateLinkRerouteNode</returns>
#else
		/// <summary>
		/// Create StateLinkRerouteNode.
		/// </summary>
		/// <param name="position">Node position</param>
		/// <returns>The created StateLinkRerouteNode</returns>
#endif
		public StateLinkRerouteNode CreateStateLinkRerouteNode(Vector2 position)
		{
			return CreateStateLinkRerouteNode(position, GetUniqueNodeID(), Color.white, StateLinkRerouteNode.kDefaultDirection, 0);
		}

		bool DeleteStateLinkRerouteNode(StateLinkRerouteNode stateLinkRerouteNode)
		{
			int nodeID = stateLinkRerouteNode.nodeID;

			ComponentUtility.RegisterCompleteObjectUndo(this, "Delete Nodes");

			ComponentUtility.RecordObject(this, "Delete Nodes");
			_StateLinkRerouteNodes.Remove(stateLinkRerouteNode);
			RemoveNode(stateLinkRerouteNode);

			int stateCount = _States.Count;
			for (int stateIndex = 0; stateIndex < stateCount; stateIndex++)
			{
				State otherState = _States[stateIndex];
				otherState.DisconnectState(nodeID);
			}

			ComponentUtility.RecordObject(this, "Delete Nodes");

			int stateLinkCount = _StateLinkRerouteNodes.count;
			for (int stateLinkIndex = 0; stateLinkIndex < stateLinkCount; stateLinkIndex++)
			{
				StateLinkRerouteNode otherStateLink = _StateLinkRerouteNodes[stateLinkIndex];
				if (stateLinkRerouteNode != otherStateLink && otherStateLink.link.stateID == nodeID)
				{
					otherStateLink.link.stateID = 0;
				}
			}

			ComponentUtility.SetDirty(this);

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードの削除。
		/// </summary>
		/// <param name="node">削除するノード</param>
		/// <returns>削除した場合はtrue、していなければfalseを返す。</returns>
#else
		/// <summary>
		/// Delete node.
		/// </summary>
		/// <param name="node">The node to delete</param>
		/// <returns>Returns true if deleted, false otherwise.</returns>
#endif
		protected sealed override bool OnDeleteNode(Node node)
		{
			State state = node as State;
			if (state != null)
			{
				return DeleteState(state);
			}

			StateLinkRerouteNode stateLinkNode = node as StateLinkRerouteNode;
			if (stateLinkNode != null)
			{
				return DeleteStateLinkRerouteNode(stateLinkNode);
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Resetもしくは生成時のコールバック。
		/// </summary>
#else
		/// <summary>
		/// Reset or create callback.
		/// </summary>
#endif
		protected sealed override void OnReset()
		{
			graphName = "New StateMachine";
		}

		private int _StartingTriggerFrameCount = 0;
		private List<Trigger> _StartingTriggers = new List<Trigger>();

#if ARBOR_DOC_JA
		/// <summary>
		/// 各ステートとStateLinkの遷移回数を0にクリアする。
		/// </summary>
#else
		/// <summary>
		/// Clear the number of transitions between each state and StateLink to 0.
		/// </summary>
#endif
		public void ClearTransitionCount()
		{
			ClearTransitionCount(false);
		}

		void ClearTransitionCount(bool clearHistory)
		{
			int stateCount = _States.Count;
			for (int stateIndex = 0; stateIndex < stateCount; stateIndex++)
			{
				State state = _States[stateIndex];
				state.transitionCount = 0;

				int behaviourCount = state.behaviourCount;
				for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
				{
					StateBehaviour behaviour = state.GetBehaviourFromIndex(behaviourIndex);
					if (behaviour != null)
					{
						behaviour.ClearTransitionCount(clearHistory);
					}
				}
			}

			int rerouteCount = _StateLinkRerouteNodes.count;
			for (int rerouteIndex = 0; rerouteIndex < rerouteCount; rerouteIndex++)
			{
				var rerouteNode = _StateLinkRerouteNodes[rerouteIndex];
				rerouteNode.link.transitionCount = 0;
				if (clearHistory)
				{
					rerouteNode.link.histroyIndex = 0;
				}
			}
		}

		void ClearLinkHistory()
		{
			_StateLinkHistoryCount = 0;
		}

		bool NextState()
		{
			if (_ReservedState != null)
			{
				if (!_IsBreaked)
				{
					State nextState = _ReservedState;
					_ReservedState = null;

					ChangeState(nextState);
				}

				return true;
			}

			return false;
		}

		private bool _DidImmediateTransition = false;

		bool NextImmediateState()
		{
			if (!nextImmediateTransition)
			{
				return false;
			}

			BeginTransitionLoop();

			NextState();
			
			EndTransitionLoop();

			return true;
		}

		void BeginTransitionLoop()
		{
			_IsInfiniteLoopWarning = false;
			_TransitionCounter = 0;
		}

		bool UpdateTransitionLoop()
		{
			if (_IsInfiniteLoopWarning)
			{
				return false;
			}

			if (_TransitionCounter >= currentDebugInfiniteLoopSettings.maxLoopCount)
			{
				_IsInfiniteLoopWarning = true;
				return false;
			}
			else
			{
				_TransitionCounter++;
			}

			return true;
		}

		void EndTransitionLoop()
		{
			if (_IsInfiniteLoopWarning)
			{
				_IsInfiniteLoopWarning = false;
				if (currentDebugInfiniteLoopSettings.enableLogging)
				{
					Debug.LogWarning("Over " + currentDebugInfiniteLoopSettings.maxLoopCount + " transitions per frame. Please check the infinite loop of " + ToString(), this);
				}

				if (currentDebugInfiniteLoopSettings.enableBreak)
				{
					Debug.Break();
				}
			}
		}

		void UpdateLinkHistory(StateLink link)
		{
			// improved get property performance.
			uint transitionCount = link.transitionCount;
			if (transitionCount < uint.MaxValue)
			{
				transitionCount++;
			}
			link.transitionCount = transitionCount;

			long historyIndex = ++_StateLinkHistoryCount;

			while (link != null)
			{
				link.histroyIndex = historyIndex;

				StateLinkRerouteNode rerouteNode = GetNodeFromID(link.stateID) as StateLinkRerouteNode;
				if (rerouteNode != null)
				{
					link = rerouteNode.link;
					uint rerouteTransitionCount = link.transitionCount;
					if (rerouteTransitionCount < uint.MaxValue)
					{
						rerouteTransitionCount++;
					}
					link.transitionCount = rerouteTransitionCount;
				}
				else
				{
					link = null;
				}
			}
		}

		internal bool IsLeavedState(int nodeID)
		{
			if (nextImmediateTransition)
			{
				return true;
			}

			if (_CurrentState != null && _CurrentState.nodeID != nodeID)
			{
				return true;
			}

			return false;
		}

		void ChangeState(State nextState)
		{
			while (nextState != null)
			{
				_ChangingState = true;
				_InStateEvent = true;

				_NextTransitionState = nextState;

				if (_CurrentState != null)
				{
					_CurrentState.Activate(false);
					_PrevTransitionState = _CurrentState;
				}

				_NextTransitionState = null;

				if (!isActiveAndEnabled)
				{
					_CurrentState = null;
					_ReservedState = nextState;

					StateChanged();

					_ChangingState = false;
					_InStateEvent = false;

					return;
				}

				if (_ReservedStateLink != null)
				{
					UpdateLinkHistory(_ReservedStateLink);

					_ReservedStateLink = null;
				}
				else
				{
					ClearLinkHistory();
				}

				_CurrentState = nextState;

				bool isBreaked = false;

				if (_CurrentState != null)
				{
					State currentState = _CurrentState;
					currentState.Activate(true);
					isBreaked = currentState.breakPoint;
				}

				StateChanged();

				_ChangingState = false;
				_InStateEvent = false;

				if (!_EnterStoppingPlayState && playState != PlayState.Playing)
				{
					_Triggers.Clear();
					break;
				}

				int triggerCount = _Triggers.Count;
				if (triggerCount > 0)
				{
					List<Trigger> triggers = _Triggers;
					_Triggers = (_TriggersBuffers.Count > 0) ? _TriggersBuffers.Dequeue() : null;
					if (_Triggers == null)
					{
						_Triggers = new List<Trigger>();
					}
					else
					{
						_Triggers.Clear();
					}

					for (int i = 0; i < triggerCount; ++i)
					{
						Trigger trigger = triggers[i];
						SendTrigger(trigger.message, trigger.flags);
					}
					triggers.Clear();
					_TriggersBuffers.Enqueue(triggers);
				}

				if (isBreaked)
				{
					_IsBreaked = isBreaked;
				}

				if (!isBreaked && nextImmediateTransition)
				{
					if (UpdateTransitionLoop())
					{
						nextState = _ReservedState;
						_ReservedState = null;
					}
					else
					{
						break;
					}
				}
				else
				{
					break;
				}
			}
		}

		private bool IsValidTransition(State nextState, TransitionTiming transitionTiming)
		{
			if (nextState == null || nextState.stateMachine != this || nextState.resident)
			{
				return false;
			}

			switch (transitionTiming)
			{
				case TransitionTiming.Immediate:
					return true;
				case TransitionTiming.LateUpdateDontOverwrite:
					return _ReservedState == null;
				case TransitionTiming.LateUpdateOverwrite:
					return true;
				case TransitionTiming.NextUpdateOverwrite:
					return true;
				case TransitionTiming.NextUpdateDontOverwrite:
					return _ReservedState == null;
			}

			return false;
		}

		private void TransitionInternal(State nextState, TransitionTiming transitionTiming)
		{
			switch (transitionTiming)
			{
				case TransitionTiming.Immediate:
					if (!_InStateEvent && isActiveAndEnabled)
					{
						if (_ReservedState != null)
						{
							_ReservedState = null;
						}

						bool hasGroup = CalculateScope.hasScope;
						if (!hasGroup)
						{
							using (CalculateScope.OpenScope())
							{
								ChangeState(nextState);
							}
						}
						else
						{
							ChangeState(nextState);
						}
					}
					else
					{
						_ReservedStateTransitionTiming = transitionTiming;
						_ReservedState = nextState;
					}
					break;
				case TransitionTiming.LateUpdateDontOverwrite:
					if (_ReservedState == null)
					{
						_ReservedStateTransitionTiming = transitionTiming;
						_ReservedState = nextState;
					}
					break;
				case TransitionTiming.LateUpdateOverwrite:
					_ReservedStateTransitionTiming = transitionTiming;
					_ReservedState = nextState;
					break;
				case TransitionTiming.NextUpdateDontOverwrite:
					if (_ReservedState == null)
					{
						_ReservedStateTransitionTiming = transitionTiming;
						_ReservedState = nextState;
					}
					break;
				case TransitionTiming.NextUpdateOverwrite:
					_ReservedStateTransitionTiming = transitionTiming;
					_ReservedState = nextState;
					break;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移
		/// </summary>
		/// <param name="nextState">遷移先のステート。</param>
		/// <param name="transitionTiming">遷移するタイミング。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition
		/// </summary>
		/// <param name="nextState">Destination state.</param>
		/// <param name="transitionTiming">Transition timing.</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(State nextState, TransitionTiming transitionTiming)
		{
			_ReservedStateLink = null;

			if (IsValidTransition(nextState, transitionTiming))
			{
				TransitionInternal(nextState, transitionTiming);
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移
		/// </summary>
		/// <param name="nextState">遷移先のステート。</param>
		/// <param name="immediateTransition">すぐに遷移するかどうか。falseの場合は現在フレームの最後(LateUpdate時)に遷移する。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition
		/// </summary>
		/// <param name="nextState">Destination state.</param>
		/// <param name="immediateTransition">Whether or not to transition immediately. If false I will transition to the end of the current frame (when LateUpdate).</param>
		/// <returns>Whether or not the transition</returns>
#endif
		[System.Obsolete("use Transition(State nextState,TransitionTiming transitionTiming).")]
		public bool Transition(State nextState, bool immediateTransition)
		{
			return Transition(nextState, immediateTransition ? TransitionTiming.Immediate : TransitionTiming.LateUpdateOverwrite);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移する。実際に遷移するタイミングは現在フレームの最後(LateUpdate時)。
		/// </summary>
		/// <param name="nextState">遷移先のステート。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition. Timing to actually transition current frame last (when LateUpdate).
		/// </summary>
		/// <param name="nextState">Destination state.</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(State nextState)
		{
			return Transition(nextState, TransitionTiming.LateUpdateOverwrite);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移
		/// </summary>
		/// <param name="nextStateID">遷移先のステートID。</param>
		/// <param name="transitionTiming">遷移するタイミング。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition
		/// </summary>
		/// <param name="nextStateID">State ID for the transition destination.</param>
		/// <param name="transitionTiming">Transition timing.</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(int nextStateID, TransitionTiming transitionTiming)
		{
			State nextState = GetStateFromID(nextStateID);
			return Transition(nextState, transitionTiming);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移
		/// </summary>
		/// <param name="nextStateID">遷移先のステートID。</param>
		/// <param name="immediateTransition">すぐに遷移するかどうか。falseの場合は現在フレームの最後(LateUpdate時)に遷移する。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition
		/// </summary>
		/// <param name="nextStateID">State ID for the transition destination.</param>
		/// <param name="immediateTransition">Whether or not to transition immediately. If false I will transition to the end of the current frame (when LateUpdate).</param>
		/// <returns>Whether or not the transition</returns>
#endif
		[System.Obsolete("use Transition(int nextStateID,TransitionTiming transitionTiming).")]
		public bool Transition(int nextStateID, bool immediateTransition)
		{
			return Transition(nextStateID, immediateTransition ? TransitionTiming.Immediate : TransitionTiming.LateUpdateOverwrite);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移する。実際に遷移するタイミングは現在フレームの最後(LateUpdate時)。
		/// </summary>
		/// <param name="nextStateID">遷移先のステートID。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition. Timing to actually transition current frame last (when LateUpdate).
		/// </summary>
		/// <param name="nextStateID">State ID for the transition destination.</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(int nextStateID)
		{
			return Transition(nextStateID, TransitionTiming.LateUpdateOverwrite);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移
		/// </summary>
		/// <param name="nextStateLink">遷移の接続先。</param>
		/// <param name="transitionTiming">遷移するタイミング。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition
		/// </summary>
		/// <param name="nextStateLink">The destination of transition.</param>
		/// <param name="transitionTiming">Transition timing.</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(StateLink nextStateLink, TransitionTiming transitionTiming)
		{
			if (nextStateLink.stateID != 0)
			{
				State nextState = GetState(nextStateLink);

				if (IsValidTransition(nextState, transitionTiming))
				{
					_ReservedStateLink = nextStateLink;

					TransitionInternal(nextState, transitionTiming);

					return true;
				}
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移
		/// </summary>
		/// <param name="nextStateLink">遷移の接続先。</param>
		/// <param name="immediateTransition">すぐに遷移するかどうか。falseの場合は現在フレームの最後(LateUpdate時)に遷移する。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition
		/// </summary>
		/// <param name="nextStateLink">The destination of transition.</param>
		/// <param name="immediateTransition">Whether or not to transition immediately. If false I will transition to the end of the current frame (when LateUpdate).</param>
		/// <returns>Whether or not the transition</returns>
#endif
		[System.Obsolete("use Transition(StateLink nextStateLink,TransitionTiming transitionTiming).")]
		public bool Transition(StateLink nextStateLink, bool immediateTransition)
		{
			return Transition(nextState, immediateTransition ? TransitionTiming.Immediate : TransitionTiming.LateUpdateOverwrite);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移する。実際に遷移するタイミングは現在フレームの最後(LateUpdate時)。
		/// </summary>
		/// <param name="nextStateLink">遷移の接続先。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition. Timing to actually transition current frame last (when LateUpdate).
		/// </summary>
		/// <param name="nextStateLink">The destination of transition.</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(StateLink nextStateLink)
		{
			return Transition(nextStateLink, nextStateLink.transitionTiming);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// トリガーの送信
		/// </summary>
		/// <param name="message">送信するメッセージ</param>
		/// <param name="flags">送信するトリガーフラグ</param>
#else
		/// <summary>
		/// Sending of trigger
		/// </summary>
		/// <param name="message">Message to be sent</param>
		/// <param name="flags">Trigger flag to send</param>
#endif
		public void SendTrigger(string message, SendTriggerFlags flags)
		{
			using (CalculateScope.OpenScope())
			{
				if (!(isActiveAndEnabled && playState == PlayState.Playing))
				{
					if (isActiveAndEnabled && playOnStart && !isStarted && playState != PlayState.Playing)
					{
						int frameCount = Time.frameCount;

						if (_StartingTriggerFrameCount != frameCount)
						{
							_StartingTriggers.Clear();
							_StartingTriggerFrameCount = frameCount;
						}

						_StartingTriggers.Add(new Trigger() { message = message, flags = flags });
					}
					return;
				}

				if (_ChangingState)
				{
					Trigger trigger = new Trigger()
					{
						message = message,
						flags = flags,
					};
					_Triggers.Add(trigger);
					return;
				}

				bool inStateEvent = _InStateEvent;
				_InStateEvent = true;

				if ((flags & SendTriggerFlags.ResidentStates) != 0)
				{
					for (int i = 0, count = _ResidentStates.Count; i < count; i++)
					{
						State state = _ResidentStates[i];
						state.SendTrigger(message);
					}
				}

				if ((flags & SendTriggerFlags.CurrentState) != 0)
				{
					if (_CurrentState != null)
					{
						_CurrentState.SendTrigger(message);
					}
				}

				_InStateEvent = inStateEvent;

				BeginTransitionLoop();

				if (!inStateEvent && nextImmediateTransition)
				{
					NextState();
				}

				EndTransitionLoop();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// トリガーの送信
		/// </summary>
		/// <param name="message">送信するメッセージ</param>
#else
		/// <summary>
		/// Sending of trigger
		/// </summary>
		/// <param name="message">Message to be sent</param>
#endif
		public void SendTrigger(string message)
		{
			// For calls from UnityEvent
			SendTrigger(message, allSendTrigger);
		}

		void RegisterState(State state)
		{
			_DicStates.Add(state.nodeID, state);
			if (state.resident)
			{
				_ResidentStates.Add(state);
			}
			RegisterNode(state);
		}

		void UnregisterState(State state)
		{
			_DicStates.Remove(state.nodeID);
			if (state.resident)
			{
				_ResidentStates.Remove(state);
			}
			RemoveNode(state);
		}

		/// <summary>
		/// Register nodes
		/// </summary>
		protected sealed override void OnRegisterNodes()
		{
			_ResidentStates.Clear();
			_DicStates.Clear();

			for (int i = 0; i < _States.Count; i++)
			{
				State state = _States[i];
				RegisterState(state);
			}

			for (int i = 0; i < _StateLinkRerouteNodes.count; i++)
			{
				RegisterNode(_StateLinkRerouteNodes[i]);
			}
		}
	}
}
