using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Items
{
	public class ItemSensor : MonoBehaviour
	{
		public UnityEvent<Item> onItemChanged = new();
		
		private Item item;
		public Item Item
		{
			get => item;
			set
			{
				if (item == value)
					return;
				
				item = value;
				onItemChanged.Invoke(item);
			}
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if (Item != null) return;
			if (!other.attachedRigidbody) return;
			if (!other.attachedRigidbody.TryGetComponent(out Item collisionItem)) return;
			
			Item = collisionItem;
		}
		
		private void OnTriggerExit(Collider other)
		{
			if (Item == null) return;
			if (!other.attachedRigidbody) return;
			if (!other.attachedRigidbody.TryGetComponent(out Item collisionItem)) return;
			
			if (Item == collisionItem)
				Item = null;
		}
	}
}