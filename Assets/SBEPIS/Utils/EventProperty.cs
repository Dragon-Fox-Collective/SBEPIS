using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	[Serializable]
	public class EventProperty<TOwner, TValue, TSetEvent, TUnsetEvent> : EventPropertySlave<TOwner, TValue, TSetEvent, TUnsetEvent> where TValue : UnityEngine.Object where TSetEvent : UnityEvent<TOwner, TValue>, new() where TUnsetEvent : UnityEvent<TOwner, TValue, TValue>, new()
	{
		private TValue value;
		public TValue Get() => value;
		
		public void Set(TOwner owner, TValue newValue, Func<TValue, EventPropertySlave<TOwner, TValue, TSetEvent, TUnsetEvent>> slaveGetter = null)
		{
			if (value == newValue)
				return;
			
			if (value)
			{
				TValue oldVal = value;
				value = null;
				onUnset.Invoke(owner, oldVal, newValue);
				if (oldVal) slaveGetter?.Invoke(oldVal)?.onUnset.Invoke(owner, oldVal, newValue);
			}
			
			if (newValue)
			{
				value = newValue;
				onSet.Invoke(owner, value);
				if (value) slaveGetter?.Invoke(value)?.onSet.Invoke(owner, value);
			}
		}
	}
	
	[Serializable]
	public class EventPropertySlave<TOwner, TValue, TSetEvent, TUnsetEvent> where TValue : UnityEngine.Object where TSetEvent : UnityEvent<TOwner, TValue>, new() where TUnsetEvent : UnityEvent<TOwner, TValue, TValue>, new()
	{
		public TSetEvent onSet = new();
		public TUnsetEvent onUnset = new();
	}
}