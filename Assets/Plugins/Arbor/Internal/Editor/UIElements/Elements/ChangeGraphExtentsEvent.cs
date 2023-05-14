using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	public sealed class ChangeGraphExtentsEvent : EventBase<ChangeGraphExtentsEvent>, IChangeGraphViewEvent
	{
		public Rect oldExtents
		{
			get;
			private set;
		}

		public Rect newExtents
		{
			get;
			private set;
		}

		public static ChangeGraphExtentsEvent GetPooled(Rect oldExtents, Rect newExtents)
		{
			var e = GetPooled();
			e.oldExtents = oldExtents;
			e.newExtents = newExtents;
			return e;
		}
	}
}