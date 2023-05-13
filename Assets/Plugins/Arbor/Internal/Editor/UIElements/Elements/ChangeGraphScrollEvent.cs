using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	public sealed class ChangeGraphScrollEvent : EventBase<ChangeGraphScrollEvent>, IChangeGraphViewEvent
	{
		public Vector3 oldPosition
		{
			get;
			private set;
		}

		public Vector3 newPosition
		{
			get;
			private set;
		}

		public static ChangeGraphScrollEvent GetPooled(Vector3 oldPosition, Vector3 newPosition)
		{
			var e = GetPooled();
			e.oldPosition = oldPosition;
			e.newPosition = newPosition;
			return e;
		}
	}
}