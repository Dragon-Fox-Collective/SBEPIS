//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;

namespace Arbor.TaskSystem
{
#if ARBOR_DOC_JA
	/// <summary>
	/// タスクの状態
	/// </summary>
#else
	/// <summary>
	/// Task status
	/// </summary>
#endif
	public enum TaskStatus
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// アイドル
		/// </summary>
#else
		/// <summary>
		/// Idle
		/// </summary>
#endif
		Idle,

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行中
		/// </summary>
#else
		/// <summary>
		/// Running
		/// </summary>
#endif
		Running,

#if ARBOR_DOC_JA
		/// <summary>
		/// ポーズ中
		/// </summary>
#else
		/// <summary>
		/// Pausing
		/// </summary>
#endif
		Pausing,

#if ARBOR_DOC_JA
		/// <summary>
		/// 割り込みによってポーズ中
		/// </summary>
#else
		/// <summary>
		/// Pausing by interrupt
		/// </summary>
#endif
		InterruptPausing,

#if ARBOR_DOC_JA
		/// <summary>
		/// 完了
		/// </summary>
#else
		/// <summary>
		/// Completion
		/// </summary>
#endif
		Complete,
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// 処理を行うクラス。行う処理は継承して実装する。<br/><see cref="TaskScheduler"/>に登録して順次実行する。
	/// </summary>
	/// <remarks>
	/// 非同期タスクとは異なりキューに積まれたタスクを順次実行するシンプルなシステム。
	/// </remarks>
#else
	/// <summary>
	/// The class to process. The processing to be performed is inherited and implemented. <br/> Register in <see cref="TaskScheduler"/> and execute sequentially.
	/// </summary>
	/// <remarks>
	/// Unlike asynchronous tasks, it is a simple system that sequentially executes queued tasks.
	/// </remarks>
#endif
	public abstract class Task : System.IDisposable, IProgress
	{
		private TaskStatus _TaskStatus = TaskStatus.Idle;
		private TaskScheduler _Scheduler;

#if ARBOR_DOC_JA
		/// <summary>
		/// 完了したときに呼ばれる
		/// </summary>
#else
		/// <summary>
		/// Called when completed
		/// </summary>
#endif
		public event System.Action onComplete;

#if ARBOR_DOC_JA
		/// <summary>
		/// タスクの状態
		/// </summary>
#else
		/// <summary>
		/// Task status
		/// </summary>
#endif
		public TaskStatus taskStatus
		{
			get
			{
				return _TaskStatus;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 登録しているTaskSchedulerを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns the registered Task Scheduler.
		/// </summary>
#endif
		public TaskScheduler scheduler
		{
			get
			{
				return _Scheduler;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 0fから1fまでの進捗度
		/// </summary>
#else
		/// <summary>
		/// Progress from 0f to 1f
		/// </summary>
#endif
		public abstract float progress
		{
			get;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プールされているかどうか
		/// </summary>
#else
		/// <summary>
		/// Whether it is pooled
		/// </summary>
#endif
		protected bool pooled = false;

#if ARBOR_DOC_JA
		/// <summary>
		/// 初期化するときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when initializing
		/// </summary>
#endif
		protected virtual void Init()
		{
			_TaskStatus = TaskStatus.Idle;
			onComplete = null;
		}

		internal abstract void Acquire();

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行開始されたときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when execution starts
		/// </summary>
#endif
		protected virtual void OnEnter()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 更新されるときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when updated
		/// </summary>
#endif
		protected abstract void OnUpdate();

#if ARBOR_DOC_JA
		/// <summary>
		/// 割り込みによってポーズされたときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when paused by interrupt
		/// </summary>
#endif
		protected virtual void OnInterruptPause()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 割り込みによるポーズから再開されたときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when resuming from pause due to interrupt
		/// </summary>
#endif
		protected virtual void OnInterruptResume()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// タスクから抜けるときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when exiting a task
		/// </summary>
#endif
		protected virtual void OnExit()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ポーズされたときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when paused
		/// </summary>
#endif
		protected virtual void OnPause()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ポーズから再開されたときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when resuming from pause
		/// </summary>
#endif
		protected virtual void OnResume()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// タスクが完了したときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when the task is completed
		/// </summary>
#endif
		protected virtual void OnComplete()
		{
			onComplete?.Invoke();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// タスクを完了する。
		/// </summary>
#else
		/// <summary>
		/// Complete the task.
		/// </summary>
#endif
		protected void Complete()
		{
			if (_TaskStatus == TaskStatus.Running)
			{
				_TaskStatus = TaskStatus.Complete;

				OnComplete();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 廃棄処理を行う。
		/// </summary>
#else
		/// <summary>
		/// Dispose of it.
		/// </summary>
#endif
		public abstract void Dispose();

		internal void RegisterScheduler(TaskScheduler scheduler)
		{
			_Scheduler = scheduler;
		}

		internal void Enter()
		{
			if (_TaskStatus == TaskStatus.Idle)
			{
				_TaskStatus = TaskStatus.Running;
				OnEnter();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// タスクを更新する。
		/// </summary>
#else
		/// <summary>
		/// Update the task.
		/// </summary>
#endif
		public void Update()
		{
			if (_TaskStatus == TaskStatus.Running)
			{
				OnUpdate();
			}
		}

		internal void InterruptPause()
		{
			if (_TaskStatus == TaskStatus.Running)
			{
				_TaskStatus = TaskStatus.InterruptPausing;

				OnInterruptPause();
			}
		}

		internal void InterruptResume()
		{
			if (_TaskStatus == TaskStatus.InterruptPausing)
			{
				OnInterruptResume();

				_TaskStatus = TaskStatus.Running;
			}
		}

		internal void Exit()
		{
			if (_TaskStatus != TaskStatus.Idle)
			{
				_TaskStatus = TaskStatus.Idle;

				OnExit();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// タスクをポーズする。
		/// </summary>
#else
		/// <summary>
		/// Pause the task.
		/// </summary>
#endif
		public void Pause()
		{
			if (_TaskStatus == TaskStatus.Running)
			{
				_TaskStatus = TaskStatus.Pausing;

				OnPause();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// タスクをポーズから再開する。
		/// </summary>
#else
		/// <summary>
		/// Resume the task from pause.
		/// </summary>
#endif
		public void Resume()
		{
			if (_TaskStatus == TaskStatus.Pausing)
			{
				_TaskStatus = TaskStatus.Running;

				OnResume();
			}
		}
	}
}