using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	public class Inventory : MonoBehaviour, IEnumerable<DequeStorable>
	{
		[SerializeField]
		private DequeStorable cardPrefab;
		[SerializeField]
		private int initialCardCount = 5;
		
		[Tooltip("Purely organizational for the hierarchy")]
		[SerializeField]
		private Transform cardParent;
		
		public UnityEvent<Storable> onLoadIntoDeque = new();
		public UnityEvent<List<DequeStorable>> onSaveFromDeque = new();
		
		private List<DequeStorable> savedInventory;
		private Storable storable;
		
		private void Start()
		{
			SaveInitialInventory();
		}
		
		private void SaveInitialInventory()
		{
			List<DequeStorable> initialInventory = new();
			for (int _ = 0; _ < initialCardCount; _++)
			{
				DequeStorable card = Instantiate(cardPrefab);
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
			foreach (DequeStorable card in savedInventory)
			{
				print($"Ejecting leftover card {card}");
				card.gameObject.SetActive(true);
				card.transform.SetPositionAndRotation(deque.transform.position, deque.transform.rotation);
			}
			savedInventory.Clear();
			onLoadIntoDeque.Invoke(storable);
		}
		
		public void SetCardParent(DequeStorable card, DequeOwner owner)
		{
			card.transform.SetParent(owner ? cardParent : null);
		}
		
		public bool CanFetch(DequeStorable card) => storable.CanFetch(card);
		public UniTask<Capturellectable> Fetch(DequeStorable card) => storable.Fetch(card);
		public UniTask<(DequeStorable, Capturellectainer, Capturellectable)> Store(Capturellectable item) => storable.Store(item);
		public IEnumerable<Texture2D> GetCardTextures(DequeStorable card) => storable.GetCardTextures(card);
		public IEnumerator<DequeStorable> GetEnumerator() => storable.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
