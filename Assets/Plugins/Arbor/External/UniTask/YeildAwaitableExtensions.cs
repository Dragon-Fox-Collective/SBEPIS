//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UNITASK
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Arbor.Threading.Tasks
{
	public static class YieldAwaitableExtensions
	{
		public static UniTask ToUniTask(this YieldAwaitable awaitable)
		{
			return new UniTask(YieldPromise.Create(awaitable.dispatcher, awaitable.cancellationToken, out var token), token);
		}

		internal sealed class YieldPromise : IUniTaskSource, ITaskPoolNode<YieldPromise>
		{
			static TaskPool<YieldPromise> pool;
			YieldPromise nextNode;
			public ref YieldPromise NextNode => ref nextNode;

			static YieldPromise()
			{
				TaskPool.RegisterSizeGetter(typeof(YieldPromise), () => pool.Size);
			}

			readonly Action action;
			CancellationToken cancellationToken;
			UniTaskCompletionSourceCore<object> core;

			YieldPromise()
			{
				action = Invoke;
			}

			public static IUniTaskSource Create(IActionDispatcher dispatcher, CancellationToken cancellationToken, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}

				if (!pool.TryPop(out var result))
				{
					result = new YieldPromise();
				}

				result.cancellationToken = cancellationToken;

				dispatcher.Register(result.action);

				TaskTracker.TrackActiveTask(result, 3);

				token = result.core.Version;
				return result;
			}

			void Invoke()
			{
				if (cancellationToken.IsCancellationRequested)
				{
					core.TrySetCanceled(cancellationToken);
					return;
				}

				core.TrySetResult(null);
			}

			public void GetResult(short token)
			{
				try
				{
					core.GetResult(token);
				}
				finally
				{
					TryReturn();
				}
			}

			bool TryReturn()
			{
				TaskTracker.RemoveTracking(this);
				core.Reset();
				cancellationToken = default;
				return pool.TryPush(this);
			}

			public UniTaskStatus GetStatus(short token)
			{
				return core.GetStatus(token);
			}

			public UniTaskStatus UnsafeGetStatus()
			{
				return core.UnsafeGetStatus();
			}

			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				core.OnCompleted(continuation, state, token);
			}
		}
	}
}
#endif