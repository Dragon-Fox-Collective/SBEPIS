//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.Playables;

#if ARBOR_DOC_JA
	/// <summary>
	/// Stateの挙動を定義するクラス。継承して利用する。
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="AddBehaviourMenu" /></description></item>
	/// <item><description><see cref="HideBehaviour" /></description></item>
	/// <item><description><see cref="BehaviourTitle" /></description></item>
	/// <item><description><see cref="BehaviourHelp" /></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Class that defines the behavior of the State. Inherited and to use.
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="AddBehaviourMenu" /></description></item>
	/// <item><description><see cref="HideBehaviour" /></description></item>
	/// <item><description><see cref="BehaviourTitle" /></description></item>
	/// <item><description><see cref="BehaviourHelp" /></description></item>
	/// </list>
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[Internal.DocumentManual("/manual/scripting/statebehaviour.md")]
	public class StateBehaviour : PlayableBehaviour, IPlayableBehaviourCallbackReceiver
	{
		#region Serialize fields

		[HideInInspector]
		[SerializeField]
		private bool _BehaviourEnabled = true;

		#endregion // Serialize fields

		private List<StateLink> _StateLinkCache = new List<StateLink>();

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートマシンを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets the state machine.
		/// </summary>
#endif
		public ArborFSMInternal stateMachine
		{
			get
			{
				return nodeGraph as ArborFSMInternal;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateを取得。
		/// </summary>
#else
		/// <summary>
		/// Get the State.
		/// </summary>
#endif
		public State state
		{
			get
			{
				return node as State;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateIDを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets the state identifier.
		/// </summary>
#endif
		[System.Obsolete("use nodeID")]
		public int stateID
		{
			get
			{
				return nodeID;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// behaviourEnabledが変更されたときに呼び出される
		/// </summary>
#else
		/// <summary>
		/// Called when behaviorEnabled changes
		/// </summary>
#endif
		public System.Action onBehaviourEnabledChanged;

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourの有効状態を取得/設定。
		/// </summary>
		/// <value>
		///   <c>true</c> 有効; その他、 <c>false</c>.
		/// </value>
#else
		/// <summary>
		/// Gets or sets a value indicating whether [behaviour enabled].
		/// </summary>
		/// <value>
		///   <c>true</c> if [behaviour enabled]; otherwise, <c>false</c>.
		/// </value>
#endif
		public bool behaviourEnabled
		{
			get
			{
				return _BehaviourEnabled;
			}
			set
			{
				if (_BehaviourEnabled != value)
				{
					_BehaviourEnabled = value;

					State state = this.state;
					state.SetDirtyCallbackEntry();
					if (stateMachine.currentState == state)
					{
						if (_BehaviourEnabled)
						{
							this.Activate(true, !this.IsActive());
						}
						else
						{
							this.Activate(false, false);
						}
					}

					onBehaviourEnabledChanged?.Invoke();
				}
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
				ArborFSMInternal fsm = stateMachine;
				return (fsm != null) ? fsm.prevTransitionState : null;
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
				ArborFSMInternal fsm = stateMachine;
				return (fsm != null) ? fsm.nextTransitionState : null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateLinkの数。
		/// </summary>
#else
		/// <summary>
		/// Count of StateLink.
		/// </summary>
#endif
		public int stateLinkCount
		{
			get
			{
				return _StateLinkCache.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// enabledの初期化を行うために呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called to perform enabled initialization.
		/// </summary>
#endif
		protected sealed override void OnInitializeEnabled()
		{
			enabled = false;
			if (state.isActive)
			{
				this.Activate(true, true);
			}
		}

		internal static StateBehaviour CreateStateBehaviour(Node node, System.Type type)
		{
			System.Type classType = typeof(StateBehaviour);
			if (type != classType && !TypeUtility.IsSubclassOf(type, classType))
			{
				throw new System.ArgumentException("The type `" + type.Name + "' must be convertible to `StateBehaviour' in order to use it as parameter `type'", "type");
			}

			return CreateNodeBehaviour(node, type) as StateBehaviour;
		}

		internal static Type CreateStateBehaviour<Type>(Node node) where Type : StateBehaviour
		{
			return CreateNodeBehaviour<Type>(node);
		}

		private StateBehaviourCallbackInfo _CallbackInfo;
		internal StateBehaviourCallbackInfo callbackInfo
		{
			get
			{
				if (_CallbackInfo == null)
				{
					_CallbackInfo = StateBehaviourCallbackInfo.GetCallback(this.GetType());
				}
				return _CallbackInfo;
			}
		}

		internal void CallSendTriggerInternal(string message)
		{
			if (!enabled || !behaviourEnabled)
			{
				return;
			}

			UpdateDataLink(DataLinkUpdateTiming.Execute);

			try
			{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
				using (new ProfilerScope(GetProfilerName("OnStateTrigger()")))
#endif
				{
					OnStateTrigger(message);
				}
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex, this);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateに初めて入った際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when [state enter first].
		/// </summary>
#endif
		public virtual void OnStateAwake()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateに入った際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when [state enter].
		/// </summary>
#endif
		public virtual void OnStateBegin()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateの更新。毎フレーム呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Update of State. It is called every frame.
		/// </summary>
#endif
		public virtual void OnStateUpdate()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// State用のLateUpdate。毎フレーム、全てのUpdate後に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// LateUpdate for State. Every frame, called after all updates.
		/// </summary>
#endif
		public virtual void OnStateLateUpdate()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// State用のFixedUpdate。物理演算のためのフレームレートに依存しないUpdate。
		/// </summary>
#else
		/// <summary>
		/// FixedUpdate for State. Frame-rate-independent Update for physics operations.
		/// </summary>
#endif
		public virtual void OnStateFixedUpdate()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateから出る際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when [state exit].
		/// </summary>
#endif
		public virtual void OnStateEnd()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// SendTriggerから呼び出される。
		/// </summary>
		/// <param name="message">メッセージ</param>
#else
		/// <summary>
		/// Called from SendTrigger.
		/// </summary>
		/// <param name="message">Message</param>
#endif
		public virtual void OnStateTrigger(string message)
		{
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
			if (!enabled)
			{
				return false;
			}

			var stateMachine = this.stateMachine; // improved get property performance.
			if (stateMachine != null)
			{
				return stateMachine.Transition(nextStateID, transitionTiming);
			}

			return false;
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
		[System.Obsolete("use Transition(int nextStateID, TransitionTiming transitionTiming)")]
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
			if (!enabled)
			{
				return false;
			}

			var stateMachine = this.stateMachine; // improved get property performance.
			if (stateMachine != null)
			{
				return stateMachine.Transition(nextStateID);
			}

			return false;
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
			if (!enabled)
			{
				return false;
			}

			var stateMachine = this.stateMachine; // improved get property performance.
			if (stateMachine != null)
			{
				return stateMachine.Transition(nextState, transitionTiming);
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
		[System.Obsolete("use Transition(State nextState, TransitionTiming transitionTiming)")]
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
			if (!enabled)
			{
				return false;
			}

			var stateMachine = this.stateMachine; // improved get property performance.
			if (stateMachine != null)
			{
				return stateMachine.Transition(nextState);
			}

			return false;
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
			if (!enabled)
			{
				return false;
			}

			var stateMachine = this.stateMachine; // improved get property performance.
			if (stateMachine != null)
			{
				return stateMachine.Transition(nextStateLink, transitionTiming);
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
		[System.Obsolete("use Transition(StateLink nextStateLink, TransitionTiming transitionTiming)")]
		public bool Transition(StateLink nextStateLink, bool immediateTransition)
		{
			return Transition(nextStateLink, immediateTransition ? TransitionTiming.Immediate : TransitionTiming.LateUpdateOverwrite);
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
			if (!enabled)
			{
				return false;
			}

			var stateMachine = this.stateMachine; // improved get property performance.
			if (stateMachine != null)
			{
				return stateMachine.Transition(nextStateLink);
			}
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを追加。
		/// </summary>
		/// <param name="type">追加するStateBehaviourの型</param>
		/// <returns>追加したStateBehaviour</returns>
#else
		/// <summary>
		/// Adds the behaviour.
		/// </summary>
		/// <param name="type">Type of add StateBehaviour</param>
		/// <returns>Added StateBehaviour</returns>
#endif
		public StateBehaviour AddBehaviour(System.Type type)
		{
			return state.AddBehaviour(type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを追加。
		/// </summary>
		/// <typeparam name="T">追加するStateBehaviourの型</typeparam>
		/// <returns>追加したStateBehaviour</returns>
#else
		/// <summary>
		/// Adds the behaviour.
		/// </summary>
		/// <typeparam name="T">Type of add StateBehaviour</typeparam>
		/// <returns>Added StateBehaviour</returns>
#endif
		public T AddBehaviour<T>() where T : StateBehaviour
		{
			return state.AddBehaviour<T>();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを取得。
		/// </summary>
		/// <param name="type">取得したいStateBehaviourの型。</param>
		/// <returns>見つかったStateBehaviour。ない場合はnull。</returns>
#else
		/// <summary>
		/// Gets the behaviour.
		/// </summary>
		/// <param name="type">Type of you want to get StateBehaviour.</param>
		/// <returns>Found StateBehaviour. Or null if it is not.</returns>
#endif
		public StateBehaviour GetBehaviour(System.Type type)
		{
			return state.GetBehaviour(type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを取得。
		/// </summary>
		/// <typeparam name="T">取得したいStateBehaviourの型。</typeparam>
		/// <returns>見つかったStateBehaviour。ない場合はnull。</returns>
#else
		/// <summary>
		/// Gets the behaviour.
		/// </summary>
		/// <typeparam name="T">Type of you want to get StateBehaviour.</typeparam>
		/// <returns>Found StateBehaviour. Or null if it is not.</returns>
#endif
		public T GetBehaviour<T>() where T : StateBehaviour
		{
			return state.GetBehaviour<T>();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを取得。
		/// </summary>
		/// <param name="type">取得したいStateBehaviourの型。</param>
		/// <returns>見つかったStateBehaviourの配列。</returns>
#else
		/// <summary>
		/// Gets the behaviours.
		/// </summary>
		/// <param name="type">Type of you want to get StateBehaviour.</param>
		/// <returns>Array of found StateBehaviour.</returns>
#endif
		public StateBehaviour[] GetBehaviours(System.Type type)
		{
			return state.GetBehaviours(type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを取得。
		/// </summary>
		/// <typeparam name="T">取得したいStateBehaviourの型。</typeparam>
		/// <returns>見つかったStateBehaviourの配列。</returns>
#else
		/// <summary>
		/// Gets the behaviours.
		/// </summary>
		/// <typeparam name="T">Type of you want to get StateBehaviour.</typeparam>
		/// <returns>Array of found StateBehaviour.</returns>
#endif
		public T[] GetBehaviours<T>() where T : StateBehaviour
		{
			return state.GetBehaviours<T>();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// インスタンスを削除する。
		/// </summary>
#else
		/// <summary>
		/// Destroys this instance.
		/// </summary>
#endif
		public void Destroy()
		{
			if (state != null)
			{
				state.DestroyBehaviour(this);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// indexのStateLinkを返す。
		/// </summary>
		/// <param name="index">StateLinkのインデックス</param>
		/// <returns>StateLink</returns>
#else
		/// <summary>
		/// Return StateLink of index.
		/// </summary>
		/// <param name="index">Index of StateLink</param>
		/// <returns>StateLink</returns>
#endif
		public StateLink GetStateLink(int index)
		{
			return _StateLinkCache[index];
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateLinkのキャッシュが再構築されたときに呼ばれる
		/// </summary>
#else
		/// <summary>
		/// Called when the StateLink cache is rebuilt
		/// </summary>
#endif
		public event System.Action onStateLinkRebuilt;

#if ARBOR_DOC_JA
		/// <summary>
		/// StateLinkのキャッシュを再構築。
		/// </summary>
#else
		/// <summary>
		/// Rebuild StateLink's cache.
		/// </summary>
#endif
		public void RebuildStateLinkCache()
		{
			_StateLinkCache.Clear();
			EachField<StateLink>.Find(this, this.GetType(), _StateLinkCache);
			onStateLinkRebuilt?.Invoke();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドに関するデータを再構築する際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// It is called when reconstructing data about fields.
		/// </summary>
#endif
		protected override void OnRebuildFields()
		{
			base.OnRebuildFields();

			RebuildStateLinkCache();
		}

		internal void ClearTransitionCount(bool clearHistroy)
		{
			for (int i = 0, count = _StateLinkCache.Count; i < count; ++i)
			{
				StateLink stateLink = _StateLinkCache[i];
				stateLink.transitionCount = 0;
				if (clearHistroy)
				{
					stateLink.histroyIndex = 0;
				}
			}
		}

		internal void DisconnectState(int stateID)
		{
			ComponentUtility.RecordObject(this, "Delete Nodes");

			for (int i = 0, count = _StateLinkCache.Count; i < count; ++i)
			{
				StateLink stateLink = _StateLinkCache[i];
				if (stateLink.stateID == stateID)
				{
					stateLink.stateID = 0;
				}
			}
		}

		void IPlayableBehaviourCallbackReceiver.OnAwake()
		{
			OnStateAwake();
		}

		void IPlayableBehaviourCallbackReceiver.OnStart()
		{
			OnStateBegin();
		}

		void IPlayableBehaviourCallbackReceiver.OnEnd()
		{
			OnStateEnd();
		}

		bool IPlayableBehaviourCallbackReceiver.NeedCallUpdate()
		{
			return callbackInfo.hasUpdate;
		}

		void IPlayableBehaviourCallbackReceiver.OnUpdate()
		{
			OnStateUpdate();
		}

		bool IPlayableBehaviourCallbackReceiver.NeedCallLateUpdate()
		{
			return callbackInfo.hasLateUpdate;
		}

		void IPlayableBehaviourCallbackReceiver.OnLateUpdate()
		{
			OnStateLateUpdate();
		}

		bool IPlayableBehaviourCallbackReceiver.NeedCallFixedUpdate()
		{
			return callbackInfo.hasFixedUpdate;
		}

		void IPlayableBehaviourCallbackReceiver.OnFixedUpdate()
		{
			OnStateFixedUpdate();
		}
	}
}
