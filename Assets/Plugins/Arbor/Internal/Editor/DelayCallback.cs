using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	internal class DelayCallback<T>
	{
		private Dictionary<T, Callback> _Callbacks;

		private System.Action<T, System.Action> _Register;
		private System.Action<T, System.Action> _Unregister;

		public DelayCallback(System.Action<T, System.Action> register, System.Action<T, System.Action> unregister, IEqualityComparer<T> comparer = null)
		{
			_Register = register;
			_Unregister = unregister;

			_Callbacks = new Dictionary<T, Callback>(comparer);
		}

		public void Register(T target, System.Action callback)
		{
			if (!_Callbacks.TryGetValue(target, out var delayCallback))
			{
				delayCallback = new Callback();
				_Register?.Invoke(target, delayCallback.OnCallback);
				_Callbacks.Add(target, delayCallback);
			}

			delayCallback.callbacks.Add(callback);
		}

		public void Unregister(T target, System.Action callback)
		{
			if (_Callbacks.TryGetValue(target, out var delayCallback))
			{
				delayCallback.callbacks.Remove(callback);

				if (delayCallback.callbacks.Count == 0)
				{
					_Unregister?.Invoke(target, delayCallback.OnCallback);
					delayCallback.Dispose();
					_Callbacks.Remove(target);
				}
			}
		}

		class Callback : System.IDisposable
		{
			public List<System.Action> callbacks = new List<System.Action>();

			private bool _Callbacked;

			public Callback()
			{
				EditorApplication.update += OnUpdate;
			}

			public void Dispose()
			{
				EditorApplication.update -= OnUpdate;
			}

			public void OnCallback()
			{
				_Callbacked = true;
			}

			void OnUpdate()
			{
				if (!_Callbacked)
				{
					return;
				}

				foreach (var callback in callbacks)
				{
					callback?.Invoke();
				}
				_Callbacked = false;
			}
		}
	}

	internal class DelayCallback<T, TArg>
	{
		private Dictionary<T, Callback> _Callbacks;

		private System.Action<T, System.Action<TArg>> _Register;
		private System.Action<T, System.Action<TArg>> _Unregister;

		public DelayCallback(System.Action<T, System.Action<TArg>> register, System.Action<T, System.Action<TArg>> unregister, IEqualityComparer<T> comparer = null)
		{
			_Register = register;
			_Unregister = unregister;

			_Callbacks = new Dictionary<T, Callback>(comparer);
		}

		public void Register(T target, System.Action<TArg> callback)
		{
			if (!_Callbacks.TryGetValue(target, out var delayCallback))
			{
				delayCallback = new Callback();
				_Register?.Invoke(target, delayCallback.OnCallback);
				_Callbacks.Add(target, delayCallback);
			}

			delayCallback.callbacks.Add(callback);
		}

		public void Unregister(T target, System.Action<TArg> callback)
		{
			if (_Callbacks.TryGetValue(target, out var delayCallback))
			{
				delayCallback.callbacks.Remove(callback);

				if (delayCallback.callbacks.Count == 0)
				{
					_Unregister?.Invoke(target, delayCallback.OnCallback);
					delayCallback.Dispose();
					_Callbacks.Remove(target);
				}
			}
		}

		class Callback : System.IDisposable
		{
			public List<System.Action<TArg>> callbacks = new List<System.Action<TArg>>();

			private bool _Callbacked;
			private TArg _Arg;

			public Callback()
			{
				EditorApplication.update += OnUpdate;
			}

			public void Dispose()
			{
				EditorApplication.update -= OnUpdate;
			}

			public void OnCallback(TArg arg)
			{
				_Callbacked = true;
				_Arg = arg;
			}

			void OnUpdate()
			{
				if (!_Callbacked)
				{
					return;
				}

				foreach (var callback in callbacks)
				{
					callback?.Invoke(_Arg);
				}
				_Callbacked = false;
			}
		}
	}
}