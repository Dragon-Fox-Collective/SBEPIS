using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class Bottle : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private InventoryStorable card;
		public InventoryStorable Card => card;
		[SerializeField, Anywhere] private Transform root;
		public Transform Root => root;
		
		public MessageInABottleDeque Deque { get; set; }
		public StorableSlot Slot { get; set; }
		public int SlotIndex { get; set; }
		public Storable OriginalStorable { get; set; }
		public DiajectorPage Page { get; set; }
		public DiajectorCaptureLayout Layout => Page.GetComponentInChildren<DiajectorCaptureLayout>();
		
		public void Fetch()
		{
			card.Interact<MessageInABottleState>(Deque, (state, _) => Deque.RemoveBottle(state, this)).Forget();
		}
	}
}
