using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor
{
	using Arbor;

	internal static class NodeGraphCallback
	{
		private static DelayCallback<NodeGraph, NodeGraph> s_StateChangedCallback = new DelayCallback<NodeGraph, NodeGraph>(
			(target, callback) => target.stateChangedCallback += callback,
			(target, callback) => target.stateChangedCallback -= callback,
			new ReferenceComparer<NodeGraph>());

		private static DelayCallback<NodeGraph> s_ChangedGraphTreeCallback = new DelayCallback<NodeGraph>(
			(target, callback) => target.onChangedGraphTree += callback,
			(target, callback) => target.onChangedGraphTree -= callback,
			new ReferenceComparer<NodeGraph>());

		class ReferenceComparer<T> : IEqualityComparer<T>
		{
			public bool Equals(T x, T y)
			{
				return ReferenceEquals(x, y);
			}

			public int GetHashCode(T obj)
			{
				return obj.GetHashCode();
			}
		}

		public static void RegisterStateChangedCallback(NodeGraph nodeGraph, System.Action<NodeGraph> callback)
		{
			s_StateChangedCallback.Register(nodeGraph, callback);
		}

		public static void UnregisterStateChangedCallback(NodeGraph nodeGraph, System.Action<NodeGraph> callback)
		{
			s_StateChangedCallback.Unregister(nodeGraph, callback);
		}

		public static void RegisterChangedGraphTreeCallback(NodeGraph nodeGraph, System.Action callback)
		{
			s_ChangedGraphTreeCallback.Register(nodeGraph, callback);
		}

		public static void UnregisterChangedGraphTreeCallback(NodeGraph nodeGraph, System.Action callback)
		{
			s_ChangedGraphTreeCallback.Unregister(nodeGraph, callback);
		}
	}
}