//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.TaskSystem
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 時間による処理を行うタスクの基本クラス。
	/// </summary>
	/// <typeparam name="T">実装する型</typeparam>
#else
	/// <summary>
	/// The base class of a task that processes by time.
	/// </summary>
	/// <typeparam name="T">Type to implement</typeparam>
#endif
	public abstract class TimerTaskBase<T> : Task<T> where T : TimerTaskBase<T>, new()
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// プールからタスクインスタンスを取得する
		/// </summary>
		/// <param name="timeType">時間タイプ</param>
		/// <param name="duration">処理を行う時間</param>
		/// <returns>タスクインスタンス</returns>
#else
		/// <summary>
		/// Get a task instance from the pool
		/// </summary>
		/// <param name="timeType">Time type</param>
		/// <param name="duration">Time to process</param>
		/// <returns>Task instance</returns>
#endif
		public static T GetPooled(TimeType timeType, float duration)
		{
			var timerTask = GetPooled();
			timerTask.timeType = timeType;
			timerTask.duration = duration;

			return timerTask;
		}

		private Timer _Timer = new Timer();

#if ARBOR_DOC_JA
		/// <summary>
		/// 進捗が変更されたときに呼ばれる
		/// </summary>
#else
		/// <summary>
		/// Called when progress changes
		/// </summary>
#endif
		public event System.Action<float> onProgress;

#if ARBOR_DOC_JA
		/// <summary>
		/// 0fから1fまでの進捗度
		/// </summary>
#else
		/// <summary>
		/// Progress from 0f to 1f
		/// </summary>
#endif
		public override float progress
		{
			get
			{
				if (duration > 0f)
				{
					return Mathf.Clamp01(_Timer.elapsedTime / duration);
				}
				return 1f;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 時間タイプ
		/// </summary>
#else
		/// <summary>
		/// Time type
		/// </summary>
#endif
		public TimeType timeType
		{
			get
			{
				return _Timer.timeType;
			}
			set
			{
				_Timer.timeType = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 処理を行う時間
		/// </summary>
#else
		/// <summary>
		/// Time to process
		/// </summary>
#endif
		public float duration
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 割り込みによってタイマーをポーズするかどうか
		/// </summary>
#else
		/// <summary>
		/// Whether to pause the timer by interrupt
		/// </summary>
#endif
		public bool interuptPause = true;

#if ARBOR_DOC_JA
		/// <summary>
		/// 初期化するときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when initializing
		/// </summary>
#endif
		protected override void Init()
		{
			base.Init();

			onProgress = null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行開始されたときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when execution starts
		/// </summary>
#endif
		protected override void OnEnter()
		{
			_Timer.Start();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 進捗度による処理を行う
		/// </summary>
		/// <param name="progress">0fから1fまでの進捗度</param>
#else
		/// <summary>
		/// Perform processing according to progress
		/// </summary>
		/// <param name="progress">Progress from 0f to 1f</param>
#endif
		protected virtual void OnProgress(float progress)
		{
			onProgress?.Invoke(progress);
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
		protected override void OnUpdate()
		{
			float elapsedTime = _Timer.elapsedTime;
			OnProgress(progress);

			if (_Timer.elapsedTime >= duration)
			{
				Complete();
			}
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
		protected override void OnExit()
		{
			_Timer.Stop();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 割り込みによってポーズされたときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when paused by interrupt
		/// </summary>
#endif
		protected override void OnInterruptPause()
		{
			if (interuptPause)
			{
				_Timer.Pause();
			}
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
		protected override void OnInterruptResume()
		{
			if (_Timer.playState == Timer.PlayState.Pausing)
			{
				_Timer.Resume();
			}
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
		protected override void OnPause()
		{
			_Timer.Pause();
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
		protected override void OnResume()
		{
			_Timer.Resume();
		}

		#if ARBOR_DOC_JA
		/// <summary>
		/// タスクの実行状況を表す文字列に変換する。
		/// </summary>
		/// <returns>タスクの実行状況を表す文字列を返す。</returns>
#else
		/// <summary>
		/// Convert to a character string that represents the execution status of the task.
		/// </summary>
		/// <returns>Returns a string that represents the execution status of the task.</returns>
#endif
		public override string ToString()
		{
			return _Timer.elapsedTime.ToString("0.00");
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// 時間経過を待つタスク
	/// </summary>
#else
	/// <summary>
	/// Tasks that wait for the passage of time
	/// </summary>
#endif
	public sealed class TimerTask : TimerTaskBase<TimerTask>
	{
	}
}