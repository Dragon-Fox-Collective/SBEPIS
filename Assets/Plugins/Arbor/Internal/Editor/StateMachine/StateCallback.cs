using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	internal static class StateCallback
	{
		private static DelayCallback<State> s_OnChangedTransitionCountDelay = new DelayCallback<State>(
			(state,callback)=> state.onChangedTransitionCount += callback, 
			(state, callback) => state.onChangedTransitionCount -= callback);

		public static void RegisterTransitionCountCallback(State state, System.Action callback)
		{
			s_OnChangedTransitionCountDelay.Register(state, callback);
		}

		public static void UnregisterTranstiionCountCallback(State state, System.Action callback)
		{
			s_OnChangedTransitionCountDelay.Unregister(state, callback);
		}
	}
}