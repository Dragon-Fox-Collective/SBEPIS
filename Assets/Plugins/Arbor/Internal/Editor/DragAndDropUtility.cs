//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor
{
	internal static class DragAndDropUtility
	{
		private sealed class State
		{
			enum Status
			{
				Ready,
				Begin,
				Drag,
			}
			private const float kDragRange = 6f;

			private Vector2 _BeginMousePosition;
			private Status _Status = Status.Ready;

			public void Begin()
			{
				_BeginMousePosition = Event.current.mousePosition;
				_Status = Status.Begin;
			}

			public void End()
			{
				_Status = Status.Ready;
			}

			public bool CanDrag()
			{
				if (_Status == Status.Begin)
				{
					if (Vector2.Distance(_BeginMousePosition, Event.current.mousePosition) > kDragRange)
					{
						_Status = Status.Drag;
					}
				}

				return _Status == Status.Drag;
			}
		}

		private static Dictionary<int, State> s_States = new Dictionary<int, State>();

		public static void Begin(int controlId)
		{
			State state;
			if (!s_States.TryGetValue(controlId, out state))
			{
				state = new State();
				s_States.Add(controlId, state);
			}

			state.Begin();
		}

		public static bool CanDrag(int controlId)
		{
			State state;
			if (s_States.TryGetValue(controlId, out state))
			{
				return state.CanDrag();
			}

			return false;
		}

		public static void End(int controlId)
		{
			State state;
			if (s_States.TryGetValue(controlId, out state))
			{
				state.End();
			}
		}
	}
}
