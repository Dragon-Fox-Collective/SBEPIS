//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Arbor.Playables
{
	using Arbor.TaskSystem;
	using Arbor.Threading.Tasks;

#if ARBOR_DOC_JA
	/// <summary>
	/// プレイ可能な挙動。PlayableGraphから参照する挙動に使用する。
	/// </summary>
#else
	/// <summary>
	/// Playable behavior, used for behavior that is referenced from a PlayableGraph.
	/// </summary>
#endif
	[AddComponentMenu("")]
	public abstract class PlayableBehaviour : NodeBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// スケジューラーの更新タイミング
		/// </summary>
#else
		/// <summary>
		/// Scheduler update timing
		/// </summary>
#endif
		public enum SchedularUpdateTiming
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// 手動で更新する。更新するにはscheduler.Update()を呼び出す必要がある。
			/// </summary>
#else
			/// <summary>
			/// Update manually. You need to call scheduler.Update() to update.
			/// </summary>
#endif
			Manual,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnUpdateで更新する。
			/// </summary>
#else
			/// <summary>
			/// Update with OnUpdate.
			/// </summary>
#endif
			OnUpdate,


#if ARBOR_DOC_JA
			/// <summary>
			/// OnLateUpdateで更新する。
			/// </summary>
#else
			/// <summary>
			/// Update with OnLateUpdate.
			/// </summary>
#endif
			OnLateUpdate,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnFixedUpdateで更新する。
			/// </summary>
#else
			/// <summary>
			/// Update with OnFixedUpdate.
			/// </summary>
#endif
			OnFixedUpdate,
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スケジューラーを破棄するタイミング。
		/// </summary>
#else
		/// <summary>
		/// Timing to destroy the scheduler.
		/// </summary>
#endif
		public enum SchedulerDestroyTiming
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// 破棄しない。
			/// </summary>
#else
			/// <summary>
			/// Don't destroy.
			/// </summary>
#endif
			None,

#if ARBOR_DOC_JA
			/// <summary>
			/// 開始時に破棄する。
			/// </summary>
#else
			/// <summary>
			/// Destroy at start.
			/// </summary>
#endif
			OnStart,

#if ARBOR_DOC_JA
			/// <summary>
			/// 終了時に破棄する。
			/// </summary>
#else
			/// <summary>
			/// Destroy at end.
			/// </summary>
#endif
			OnEnd,
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 呼び出されるまで待機するタイミング
		/// </summary>
#else
		/// <summary>
		/// Timing to wait until called
		/// </summary>
#endif
		public enum YieldTiming
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// OnUpdateが呼び出されるまで待機する。
			/// </summary>
#else
			/// <summary>
			/// Wait for OnUpdate to be called.
			/// </summary>
#endif
			OnUpdate,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnLateUpdateが呼び出されるまで待機する。
			/// </summary>
#else
			/// <summary>
			/// Wait for OnLateUpdate to be called.
			/// </summary>
#endif
			OnLateUpdate,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnFixedUpdateが呼び出されるまで待機する。
			/// </summary>
#else
			/// <summary>
			/// Wait for OnFixedUpdate to be called.
			/// </summary>
#endif
			OnFixedUpdate,
		}

		private bool _IsAwake = false;

		private bool _IsActive;

#if ARBOR_DOC_JA
		/// <summary>
		/// スケジューラーの更新タイミング
		/// </summary>
#else
		/// <summary>
		/// Scheduler update timing
		/// </summary>
#endif
		protected SchedularUpdateTiming schedulerUpdateTiming = SchedularUpdateTiming.OnUpdate;

#if ARBOR_DOC_JA
		/// <summary>
		/// スケジューラーを破棄するタイミング。
		/// </summary>
#else
		/// <summary>
		/// Timing to destroy the scheduler.
		/// </summary>
#endif
		protected SchedulerDestroyTiming schedulerDestroyTiming = SchedulerDestroyTiming.OnEnd;

		private Dictionary<YieldTiming,YieldDispatcher> _YieldDispatchers = null;

		private CancellationTokenSource _CancellationTokenSource;

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードから抜けた時に発生するキャンセルトークン
		/// </summary>
#else
		/// <summary>
		/// Cancel token generated when exiting a node
		/// </summary>
#endif
		public CancellationToken CancellationTokenOnEnd
		{
			get
			{
				if (_CancellationTokenSource == null)
				{
					_CancellationTokenSource = new CancellationTokenSource();
				}

				return _CancellationTokenSource.Token;
			}
		}

		YieldDispatcher GetOrCreateYieldDispatcher(YieldTiming timing)
		{
			if (_YieldDispatchers == null)
			{
				_YieldDispatchers = new Dictionary<YieldTiming, YieldDispatcher>();
			}

			if (!_YieldDispatchers.TryGetValue(timing, out var callback))
			{
				callback = new YieldDispatcher();
				_YieldDispatchers.Add(timing, callback);
			}

			return callback;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 次のOnUpdate呼び出しまで待機するawait可能な非同期タスクを取得する。
		/// </summary>
		/// <returns>await可能な非同期タスクを返す</returns>
#else
		/// <summary>
		/// Gets an awaitable asynchronous task that waits until the next OnUpdate call.
		/// </summary>
		/// <returns>Returns awaitable async task</returns>
#endif
		protected YieldAwaitable Yield()
		{
			var dispatcher = GetOrCreateYieldDispatcher(YieldTiming.OnUpdate);

			return new YieldAwaitable(dispatcher, CancellationTokenOnEnd);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 次のメソッド呼び出しまで待機するawait可能な非同期タスクを取得する。
		/// </summary>
		/// <param name="timing">呼び出されるまで待機するタイミング</param>
		/// <returns>await可能な非同期タスクを返す</returns>
#else
		/// <summary>
		/// Gets an awaitable asynchronous task that waits until the next method call.
		/// </summary>
		/// <param name="timing">Timing to wait until called</param>
		/// <returns>Returns awaitable async task</returns>
#endif
		protected YieldAwaitable Yield(YieldTiming timing)
		{
			var dispatcher = GetOrCreateYieldDispatcher(timing);

			return new YieldAwaitable(dispatcher, CancellationTokenOnEnd);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スケジューラーを取得する。
		/// </summary>
#else
		/// <summary>
		/// Get scheduler.
		/// </summary>
#endif
		protected TaskScheduler scheduler
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スケジューラーの進捗を取得する
		/// </summary>
#else
		/// <summary>
		/// Get the progress of the scheduler
		/// </summary>
#endif
		public IProgress schedulerProgress
		{
			get
			{
				return scheduler;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スケジューラーを取得する。スケジューラーがない場合は作成する。
		/// </summary>
		/// <returns>スケジューラーを返す。</returns>
#else
		/// <summary>
		/// Get the scheduler. If there is no scheduler, create one.
		/// </summary>
		/// <returns>Returns the scheduler.</returns>
#endif
		protected TaskScheduler GetOrCreateScheduler()
		{
			if (scheduler == null)
			{
				scheduler = TaskScheduler.GetPooled();
			}
			return scheduler;
		}

		internal bool isActive
		{
			get
			{
				return _IsActive;
			}
		}

		void CallActiveEvent()
		{
			if (_IsActive)
			{
				return;
			}
			_IsActive = true;

			IPlayableBehaviourCallbackReceiver receiver = this as IPlayableBehaviourCallbackReceiver;

			if (receiver != null)
			{
				UpdateDataLink(DataLinkUpdateTiming.Enter);
			}

			if (!_IsAwake)
			{
				_IsAwake = true;

				if (receiver != null)
				{
					try
					{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
						using (new ProfilerScope(GetProfilerName("OnAwake()")))
#endif
						{
							receiver.OnAwake();
						}
					}
					catch (System.Exception ex)
					{
						Debug.LogException(ex, this);
					}
				}
			}

			if (receiver != null)
			{
				try
				{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
					using (new ProfilerScope(GetProfilerName("OnStart()")))
#endif
					{
						receiver.OnStart();
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogException(ex, this);
				}
			}

			if (schedulerDestroyTiming == SchedulerDestroyTiming.OnStart)
			{
				DestroyScheduler();
			}
		}

		protected private virtual void OnCancelTokenSource()
		{
		}

		void CancelTokenSource()
		{
			if (_CancellationTokenSource != null)
			{
				_CancellationTokenSource.Cancel();

				if (_YieldDispatchers != null)
				{
					// call MoveNext
					foreach (var pair in _YieldDispatchers)
					{
						pair.Value.Invoke();
					}
				}

				OnCancelTokenSource();

				_CancellationTokenSource.Dispose();
				_CancellationTokenSource = null;
			}
		}

		void CallInactiveEvent()
		{
			if (!_IsActive)
			{
				return;
			}
			_IsActive = false;

			IPlayableBehaviourCallbackReceiver receiver = this as IPlayableBehaviourCallbackReceiver;

			if (_CancellationTokenSource != null || receiver != null)
			{
				UpdateDataLink(DataLinkUpdateTiming.Execute);
			}

			CancelTokenSource();

			if (receiver != null)
			{
				try
				{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
					using (new ProfilerScope(GetProfilerName("OnEnd()")))
#endif
					{
						receiver.OnEnd();
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogException(ex, this);
				}
			}

			if (schedulerDestroyTiming == SchedulerDestroyTiming.OnEnd)
			{
				DestroyScheduler();
			}
		}

		void DestroyScheduler()
		{
			if (scheduler != null)
			{
				scheduler.Stop();
				scheduler.Dispose();
				scheduler = null;
			}
		}

		void CallUpdateInternal()
		{
			if (!_IsActive)
			{
				return;
			}

			YieldDispatcher callback = null;
			bool updateYield = _YieldDispatchers != null && _YieldDispatchers.TryGetValue(YieldTiming.OnUpdate, out callback) && callback != null && callback.hasActions;

			var scheduler = this.scheduler;
			bool updateScheduler = scheduler != null && schedulerUpdateTiming == SchedularUpdateTiming.OnUpdate;

			IPlayableBehaviourCallbackReceiver receiver = this as IPlayableBehaviourCallbackReceiver;
			bool updateReceiver = receiver != null && receiver.NeedCallUpdate();

			if (updateYield || updateScheduler || updateReceiver)
			{
				UpdateDataLink(DataLinkUpdateTiming.Execute);
			}

			if (updateYield)
			{
				callback.Invoke();
			}

			if (updateScheduler)
			{
				scheduler.Update();
			}

			if (updateReceiver)
			{
				try
				{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
					using (new ProfilerScope(GetProfilerName("OnStateUpdate()")))
#endif
					{
						receiver.OnUpdate();
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogException(ex, this);
				}
			}
		}

		void CallLateUpdateInternal()
		{
			if (!_IsActive)
			{
				return;
			}

			YieldDispatcher callback = null;
			bool updateYield = _YieldDispatchers != null && _YieldDispatchers.TryGetValue(YieldTiming.OnLateUpdate, out callback) && callback != null && callback.hasActions;

			var scheduler = this.scheduler;
			bool updateScheduler = scheduler != null && schedulerUpdateTiming == SchedularUpdateTiming.OnLateUpdate;

			IPlayableBehaviourCallbackReceiver receiver = this as IPlayableBehaviourCallbackReceiver;
			bool updateReceiver = receiver != null && receiver.NeedCallLateUpdate();

			if (updateYield || updateScheduler || updateReceiver)
			{
				UpdateDataLink(DataLinkUpdateTiming.Execute);
			}

			if (updateYield)
			{
				callback.Invoke();
			}

			if (updateScheduler)
			{
				scheduler.Update();
			}

			if (updateReceiver)
			{
				try
				{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
					using (new ProfilerScope(GetProfilerName("OnStateUpdate()")))
#endif
					{
						receiver.OnLateUpdate();
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogException(ex, this);
				}
			}
		}

		void CallFixedUpdateInternal()
		{
			if (!_IsActive)
			{
				return;
			}

			YieldDispatcher callback = null;
			bool updateYield = _YieldDispatchers != null && _YieldDispatchers.TryGetValue(YieldTiming.OnFixedUpdate, out callback) && callback != null && callback.hasActions;

			var scheduler = this.scheduler;
			bool updateScheduler = scheduler != null && schedulerUpdateTiming == SchedularUpdateTiming.OnFixedUpdate;

			IPlayableBehaviourCallbackReceiver receiver = this as IPlayableBehaviourCallbackReceiver;
			bool updateReceiver = receiver != null && receiver.NeedCallFixedUpdate();

			if (updateYield || updateScheduler || updateReceiver)
			{
				UpdateDataLink(DataLinkUpdateTiming.Execute);
			}

			if (updateYield)
			{
				callback.Invoke();
			}

			if (updateScheduler)
			{
				scheduler.Update();
			}

			if (updateReceiver)
			{
				try
				{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
					using (new ProfilerScope(GetProfilerName("OnStateUpdate()")))
#endif
					{
						receiver.OnFixedUpdate();
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogException(ex, this);
				}
			}
		}

		internal void ActivateInternal(bool active, bool changeState)
		{
			if (active)
			{
				if (!enabled)
				{
					enabled = true;
					if (changeState)
					{
						CallActiveEvent();
					}
				}
			}
			else
			{
				if (enabled)
				{
					if (changeState)
					{
						CallInactiveEvent();
					}

					enabled = false;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 破棄前に呼ばれるメソッド。
		/// </summary>
#else
		/// <summary>
		/// Raises the pre destroy event.
		/// </summary>
#endif
		protected override void OnPreDestroy()
		{
			base.OnPreDestroy();

			CancelTokenSource();

			DestroyScheduler();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はグラフが一時停止したときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the graph is paused.
		/// </summary>
#endif
		protected override void OnGraphPause()
		{
			base.OnGraphPause();

			scheduler?.Pause();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はグラフが再開したときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the graph resumes.
		/// </summary>
#endif
		protected override void OnGraphResume()
		{
			base.OnGraphResume();

			scheduler?.Resume();
		}

		internal void PauseInternal()
		{
			if (_IsActive)
			{
				CallPauseEvent();
			}
			enabled = false;
		}

		internal void ResumeInternal()
		{
			if (_IsActive)
			{
				enabled = true;
				CallResumeEvent();
			}
			else
			{
				ActivateInternal(true, true);
			}
		}

		internal void StopInternal()
		{
			if (_IsActive)
			{
				CallStopEvent();
				CallInactiveEvent();
			}

			enabled = false;
		}

		internal void UpdateInternal()
		{
			CallUpdateInternal();
		}

		internal void LateUpdateInternal()
		{
			CallLateUpdateInternal();
		}

		internal void FixedUpdateInternal()
		{
			CallFixedUpdateInternal();
		}
	}
}