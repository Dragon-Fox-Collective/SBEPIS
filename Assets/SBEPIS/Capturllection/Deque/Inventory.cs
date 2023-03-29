using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class Inventory : MonoBehaviour, IEnumerable<Card>
	{
		public Card cardPrefab;
		[SerializeField]
		private int initialCardCount = 5;

		public UnityEvent<Storable> onLoadIntoDeque = new();

		private List<Card> savedInventory;
		private Storable storable;
		
		private void Start()
		{
			SaveInitialInventory();
		}
		
		private void SaveInitialInventory()
		{
			List<Card> initialInventory = new();
			for (int _ = 0; _ < initialCardCount; _++)
			{
				Card card = Instantiate(cardPrefab);
				initialInventory.Add(card);
			}
			savedInventory = initialInventory;
		}
		
		public void SaveInventoryFromDeque()
		{
			savedInventory = storable.ToList();
			Destroy(storable.gameObject);
		}
		
		public void LoadInventoryIntoDeque(DequeOwner dequeOwner)
		{
			storable = StorableGroupDefinition.GetNewStorable(dequeOwner.Deque.definition);
			storable.Load(savedInventory);
			foreach (Card card in savedInventory)
			{
				print($"Ejecting leftover card {card}");
				card.gameObject.SetActive(true);
				card.transform.SetPositionAndRotation(dequeOwner.Deque.transform.position, dequeOwner.Deque.transform.rotation);
			}
			savedInventory.Clear();
			onLoadIntoDeque.Invoke(storable);
		}
		
		public bool CanFetch(Card card) => storable.CanFetch(card);
		public UniTask<Capturellectable> Fetch(Card card) => storable.Fetch(card);
		public UniTask<(Card, Capturellectainer, Capturellectable)> Store(Capturellectable item) => storable.Store(item);
		public IEnumerable<Texture2D> GetCardTextures(Card card) => storable.GetCardTextures(card);
		public IEnumerator<Card> GetEnumerator() => storable.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
