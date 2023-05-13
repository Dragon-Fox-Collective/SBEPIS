using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor
{
	using Arbor;

	internal static class StateLinkCallback
	{
		private static DelayCallback<StateLink> s_OnTransitionCountChangedDelay = new DelayCallback<StateLink>(
			(stateLink, callback) => stateLink.onTransitionCountChanged += callback,
			(stateLink, callback) => stateLink.onTransitionCountChanged -= callback);

		public static void RegisterTransitionCountCallback(StateLink stateLink, System.Action callback)
		{
			s_OnTransitionCountChangedDelay.Register(stateLink, callback);
		}

		public static void UnregisterTransitionCountCallback(StateLink stateLink, System.Action callback)
		{
			s_OnTransitionCountChangedDelay.Unregister(stateLink, callback);
		}
	}
}