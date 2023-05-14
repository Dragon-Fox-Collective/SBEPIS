//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Arbor.Threading.Tasks
{
#if ARBOR_DOC_JA
	/// <summary>
	/// アクションの発行者を定義するインターフェイス。
	/// </summary>
#else
	/// <summary>
	/// An interface that defines a dispatcher for an action.
	/// </summary>
#endif
	public interface IActionDispatcher
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// アクションを登録する。
		/// </summary>
		/// <param name="continuation">呼び出されるアクション</param>
#else
		/// <summary>
		/// Register the action.
		/// </summary>
		/// <param name="continuation">Action to be called</param>
#endif
		void Register(Action continuation);
	}

	/// <summary>
	/// await可能な呼び出し待機用の構造体
	/// </summary>
	public readonly struct YieldAwaitable
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// アクションの発行者
		/// </summary>
#else
		/// <summary>
		/// Action dispatcher
		/// </summary>
#endif
		public readonly IActionDispatcher dispatcher;

#if ARBOR_DOC_JA
		/// <summary>
		/// キャンセルトークン
		/// </summary>
#else
		/// <summary>
		/// Cancellation token
		/// </summary>
#endif
		public readonly CancellationToken cancellationToken;

		internal YieldAwaitable(IActionDispatcher dispatcher, CancellationToken cancellationToken)
		{
			this.dispatcher = dispatcher;
			this.cancellationToken = cancellationToken;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 待機するためのawaiterを取得する。
		/// </summary>
		/// <returns>awaiterのインスタンス</returns>
#else
		/// <summary>
		/// Get awaiter to wait.
		/// </summary>
		/// <returns>Instance of awaiter</returns>
#endif
		public Awaiter GetAwaiter()
		{
			return new Awaiter(this);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 非同期タスクが完了するまで待機するための構造体
		/// </summary>
#else
		/// <summary>
		/// A structure for waiting for an asynchronous task to complete
		/// </summary>
#endif
		public readonly struct Awaiter : ICriticalNotifyCompletion
		{
			readonly YieldAwaitable _Awaitable;

			internal Awaiter(in YieldAwaitable awaitable)
			{
				_Awaitable = awaitable;
			}

#if ARBOR_DOC_JA
			/// <summary>
			/// 非同期タスクが完了したかどうかを取得する。
			/// </summary>
#else
			/// <summary>
			/// Gets whether the asynchronous task has completed.
			/// </summary>
#endif
			public bool IsCompleted
			{
				get
				{
					return false;
				}
			}

#if ARBOR_DOC_JA
			/// <summary>
			/// 非同期タスクの完了の待機を終了する。
			/// </summary>
#else
			/// <summary>
			/// End the wait for the completion of the asynchronous task.
			/// </summary>
#endif
			public void GetResult()
			{
				_Awaitable.cancellationToken.ThrowIfCancellationRequested();
			}

#if ARBOR_DOC_JA
			/// <summary>
			/// 非同期タスクの完了を待機するのをやめたときに実行するアクションを設定する。
			/// </summary>
			/// <param name="continuation">待機操作の完了時に実行するアクション。</param>
#else
			/// <summary>
			/// Set the action to be taken when you stop waiting for the asynchronous task to complete.
			/// </summary>
			/// <param name="continuation">The action to take when the wait operation completes.</param>
#endif
			public void OnCompleted(Action continuation)
			{
				UnsafeOnCompleted(continuation);
			}

#if ARBOR_DOC_JA
			/// <summary>
			/// このawaiterに関連付けられている非同期タスクに継続の操作をスケジュールする。
			/// </summary>
			/// <param name="continuation">待機操作の完了時に呼び出すアクション。</param>
#else
			/// <summary>
			/// Schedule a continuation operation to the asynchronous task associated with this awaiter.
			/// </summary>
			/// <param name="continuation">The action to call when the wait operation is complete.</param>
#endif
			public void UnsafeOnCompleted(Action continuation)
			{
				if (_Awaitable.cancellationToken.IsCancellationRequested)
				{
					continuation();
					return;
				}

				_Awaitable.dispatcher.Register(continuation);
			}
		}
	}
}