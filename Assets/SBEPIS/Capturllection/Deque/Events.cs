using System;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	[Serializable]
	public class SetDequeEvent : UnityEvent<DequeOwner, Deque> { }
	[Serializable]
	public class UnsetDequeEvent : UnityEvent<DequeOwner, Deque, Deque> { }
	
	[Serializable]
	public class SetCardOwnerEvent : UnityEvent<Card, DequeOwner> { }
	[Serializable]
	public class UnsetCardOwnerEvent : UnityEvent<Card, DequeOwner, DequeOwner> { }
}
