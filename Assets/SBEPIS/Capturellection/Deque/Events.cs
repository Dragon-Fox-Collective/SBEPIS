using System;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	[Serializable]
	public class SetDequeBoxEvent : UnityEvent<DequeBoxOwner, DequeBox> { }
	[Serializable]
	public class UnsetDequeBoxEvent : UnityEvent<DequeBoxOwner, DequeBox, DequeBox> { }
	
	[Serializable]
	public class SetCardDequeEvent : UnityEvent<DequeElement, Deque> { }
	[Serializable]
	public class UnsetCardDequeEvent : UnityEvent<DequeElement, Deque, Deque> { }
	
	[Serializable]
	public class SetCardInventoryEvent : UnityEvent<InventoryStorable, Inventory> { }
	[Serializable]
	public class UnsetCardInventoryEvent : UnityEvent<InventoryStorable, Inventory, Inventory> { }
}
