using System;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	[Serializable]
	public class SetDequeEvent : UnityEvent<DequeOwner, Deque> { }
	[Serializable]
	public class UnsetDequeEvent : UnityEvent<DequeOwner, Deque, Deque> { }
	
	[Serializable]
	public class SetDequeBoxEvent : UnityEvent<DequeBoxOwner, DequeBox> { }
	[Serializable]
	public class UnsetDequeBoxEvent : UnityEvent<DequeBoxOwner, DequeBox, DequeBox> { }
	
	[Serializable]
	public class SetCardOwnerEvent : UnityEvent<DequeStorable, DequeOwner> { }
	[Serializable]
	public class UnsetCardOwnerEvent : UnityEvent<DequeStorable, DequeOwner, DequeOwner> { }
}
