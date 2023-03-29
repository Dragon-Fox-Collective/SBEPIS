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
		[SerializeField]
		private Card cardPrefab;
		[SerializeField]
		private int initialCardCount = 5;
		
		[Tooltip("Purely organizational for the hierarchy")]
		[SerializeField]
		private Transform cardParent;
		
		public UnityEvent<Storable> onLoadIntoDeque = new();
		public UnityEvent<List<Card>> onSaveFromDeque = new();
		
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
			onSaveFromDeque.Invoke(savedInventory);
		}
		
		public void LoadInventoryIntoDeque(DequeOwner dequeOwner, Deque deque)
		{
			storable = StorableGroupDefinition.GetNewStorable(deque.definition);
			storable.Load(savedInventory);
			foreach (Card card in savedInventory)
			{
				print($"Ejecting leftover card {card}");
				card.gameObject.SetActive(true);
				card.transform.SetPositionAndRotation(deque.transform.position, deque.transform.rotation);
			}
			savedInventory.Clear();
			onLoadIntoDeque.Invoke(storable);
		}
		
		public void SetCardParent(Card card, DequeOwner owner)
		{
			card.transform.SetParent(owner ? cardParent : null);
		}
		
		public bool CanFetch(Card card) => storable.CanFetch(card);
		public UniTask<Capturellectable> Fetch(Card card) => storable.Fetch(card);
		public UniTask<(Card, Capturellectainer, Capturellectable)> Store(Capturellectable item) => storable.Store(item);
		public IEnumerable<Texture2D> GetCardTextures(Card card) => storable.GetCardTextures(card);
		public IEnumerator<Card> GetEnumerator() => storable.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
