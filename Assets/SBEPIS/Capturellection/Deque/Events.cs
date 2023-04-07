using System;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	[Serializable]
	public class SetDequeBoxEvent : UnityEvent<DequeBoxOwner, DequeBox> { }
	[Serializable]
	public class UnsetDequeBoxEvent : UnityEvent<DequeBoxOwner, DequeBox, DequeBox> { }
	
	[Serializable]
	public class SetCardDequeEvent : UnityEvent<DequeStorable, Deque> { }
	[Serializable]
	public class UnsetCardDequeEvent : UnityEvent<DequeStorable, Deque, Deque> { }
}
