//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
	internal sealed class StateBehaviourCallbackInfo
	{
		private static readonly System.Type[] s_NoneParameterTypes = new System.Type[0];
		private static Dictionary<System.Type, StateBehaviourCallbackInfo> s_Callbacks = new Dictionary<System.Type, StateBehaviourCallbackInfo>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void InitializeOnLoad()
		{
			s_Callbacks.Clear();

			var types = TypeUtility.GetRuntimeTypes();
			for (int i = 0, count = types.Count; i < count; i++)
			{
				var type = types[i];
				if (!TypeUtility.IsSubclassOf(type, typeof(StateBehaviour)))
				{
					continue;
				}

				GetCallback(type);
			}
		}

		internal static StateBehaviourCallbackInfo GetCallback(System.Type type)
		{
			StateBehaviourCallbackInfo callback = null;
			if (!s_Callbacks.TryGetValue(type, out callback))
			{
				callback = new StateBehaviourCallbackInfo(type);
				s_Callbacks.Add(type, callback);
			}
			return callback;
		}

		[System.Flags]
		public enum CallbackFlags
		{
			Update = 1 << 0,
			LateUpdate = 1 << 2,
			FixedUpdate = 1 << 3,
		}

		public readonly CallbackFlags flags;

		public bool hasUpdate
		{
			get
			{
				return (flags & CallbackFlags.Update) == CallbackFlags.Update;
			}
		}

		public bool hasLateUpdate
		{
			get
			{
				return (flags & CallbackFlags.LateUpdate) == CallbackFlags.LateUpdate;
			}
		}

		public bool hasFixedUpdate
		{
			get
			{
				return (flags & CallbackFlags.FixedUpdate) == CallbackFlags.FixedUpdate;
			}
		}

		static bool HasCallback(System.Type type, string methodName)
		{
			var method = MemberCache.GetMethodInfo(type, methodName, s_NoneParameterTypes);
			return (method != null && method.DeclaringType != typeof(StateBehaviour));
		}

		private StateBehaviourCallbackInfo(System.Type type)
		{
			if (HasCallback(type, "OnStateUpdate"))
			{
				flags |= CallbackFlags.Update;
			}
			else
			{
				flags &= ~CallbackFlags.Update;
			}

			if (HasCallback(type, "OnStateLateUpdate"))
			{
				flags |= CallbackFlags.LateUpdate;
			}
			else
			{
				flags &= ~CallbackFlags.LateUpdate;
			}

			if (HasCallback(type, "OnStateFixedUpdate"))
			{
				flags |= CallbackFlags.FixedUpdate;
			}
			else
			{
				flags &= ~CallbackFlags.FixedUpdate;
			}
		}
	}
}