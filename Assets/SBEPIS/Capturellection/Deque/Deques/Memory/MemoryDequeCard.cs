using Cysharp.Threading.Tasks;
using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	[RequireComponent(typeof(InventoryStorable), typeof(ProxyCaptureContainer), typeof(FlipTracker))]
	public class MemoryDequeCard : MonoBehaviour
	{
		[SerializeField, Self] private InventoryStorable card;
		public InventoryStorable Card => card;
		[SerializeField, Self] private ProxyCaptureContainer proxy;
		public ProxyCaptureContainer Proxy => proxy;
		[SerializeField, Self] private FlipTracker flipTracker;
		public FlipTracker FlipTracker => flipTracker;
		
		public MemoryDeque Deque { get; set; }
		
		public void Flip()
		{
			card.Interact<MemoryState>(Deque, (state, card) =>
			{
				Deque.Flip(state, card);
				return UniTask.CompletedTask;
			}).Forget();
		}
	}
}
