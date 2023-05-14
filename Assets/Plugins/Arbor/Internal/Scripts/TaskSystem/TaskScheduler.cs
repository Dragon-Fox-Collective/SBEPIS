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
	/// タスクを順次実行するスケジューラー
	/// </summary>
#else
	/// <summary>
	/// Scheduler to execute tasks sequentially
	/// </summary>
#endif
	public class TaskScheduler : Task<TaskScheduler>
	{
		private Task _Current;
		private Stack<Task> _Stack = new Stack<Task>();
		private Queue<Task> _Queue = new Queue<Task>();

		private int _CountAll = 0;
		private int _CountCompleted = 0;

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

			_CountAll = 0;
			_CountCompleted = 0;
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
		public override float progress
		{
			get
			{
				switch (taskStatus)
				{
					case TaskStatus.Idle:
						return 0f;
					case TaskStatus.Complete:
						return 1f;
					default:
						{
							if (_CountAll > 0)
							{
								float currentProgress = _Current != null ? _Current.progress : 0f;
								return (_CountCompleted + currentProgress) / _CountAll;
							}
							else
							{
								return 0f;
							}
						}
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// タスクを追加する。
		/// </summary>
		/// <param name="task">追加するタスク</param>
#else
		/// <summary>
		/// Add a task.
		/// </summary>
		/// <param name="task">Tasks to add</param>
#endif
		public void Add(Task task)
		{
			task.RegisterScheduler(this);
			task.Acquire();
			_Queue.Enqueue(task);

			_CountAll++;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// タスクを割り込ませる。
		/// </summary>
		/// <param name="task">割り込むタスク</param>
#else
		/// <summary>
		/// Interrupt the task.
		/// </summary>
		/// <param name="task">Task to interrupt</param>
#endif
		public void Interrupt(Task task)
		{
			task.RegisterScheduler(this);
			task.Acquire();

			if (_Current != null)
			{
				_Current.InterruptPause();

				_Stack.Push(_Current);
			}

			_Current = task;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生開始する
		/// </summary>
#else
		/// <summary>
		/// Start playing
		/// </summary>
#endif
		public void Play()
		{
			Enter();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生停止する。
		/// </summary>
#else
		/// <summary>
		/// Playback stops.
		/// </summary>
#endif
		public void Stop()
		{
			if (_Current != null)
			{
				_Current.Exit();

				_Current.Dispose();

				_Current = null;
			}

			while (_Stack.Count > 0)
			{
				var task = _Stack.Pop();
				task.Dispose();
			}

			while (_Queue.Count > 0)
			{
				var task = _Queue.Dequeue();
				task.Dispose();
			}

			_CountAll = 0;
			_CountCompleted = 0;

			Exit();
		}

		bool NextTask()
		{
			if (_Queue.Count > 0)
			{
				_Current = _Queue.Dequeue();
				_Current.Enter();
				return true;
			}
			return false;
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
			NextTask();
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
			if (_Current == null)
			{
				Complete();
				return;
			}

			_Current.Update();
			if (_Current.taskStatus == TaskStatus.Complete)
			{
				_CountCompleted++;

				_Current.Exit();

				_Current.Dispose();
				_Current = null;

				if (_Stack.Count != 0)
				{
					_Current = _Stack.Pop();

					_Current.InterruptResume();
				}
				else if (!NextTask())
				{
					Complete();
				}
			}
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
			using (Pool.GenericPool<System.Text.StringBuilder>.Get(out var sb))
			{
				sb.Length = 0;

				if (_Current != null)
				{
					sb.Append(_Current.ToString());
				}

				if (_CountAll > 1)
				{
					if (sb.Length != 0)
					{
						sb.Append(" ");
					}
					sb.Append($"({_CountCompleted}/{_CountAll}");
				}

				return sb.ToString();
			}
		}
	}
}