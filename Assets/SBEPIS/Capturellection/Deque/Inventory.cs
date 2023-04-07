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
		private int initialCardCount = 0;
		
		[Tooltip("Purely organizational for the hierarchy")]
		[SerializeField]
		private Transform cardParent;
		
		public UnityEvent<Storable> onLoadIntoDeque = new();
		public UnityEvent<List<DequeStorable>> onSaveFromDeque = new();
		
		private List<DequeStorable> savedInventory;
		private Storable storable;
		
		private void Awake()
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
		
		private void LoadInventoryIntoDeque(Deque deque)
		{
			storable = StorableGroupDefinition.GetNewStorable(deque.definition);
			foreach (DequeStorable card in savedInventory)
				card.Deque = deque;
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
		
		private void SaveInventoryFromDeque()
		{
			savedInventory = storable.ToList();
			Destroy(storable.gameObject);
			foreach (DequeStorable card in savedInventory)
				card.Deque = null;
			onSaveFromDeque.Invoke(savedInventory);
		}
		
		private void SetCardParent(DequeStorable card) => card.transform.SetParent(cardParent);
		private static void UnsetCardParent(DequeStorable card) => card.transform.SetParent(null);
		
		public bool CanFetch(DequeStorable card) => storable.CanFetch(card);
		public UniTask<Capturellectable> Fetch(DequeStorable card) => storable.Fetch(card);
		public UniTask<(DequeStorable, Capturellectainer, Capturellectable)> Store(Capturellectable item) => storable.Store(item);
		public IEnumerable<Texture2D> GetCardTextures(DequeStorable card) => storable.GetCardTextures(card);
		public IEnumerator<DequeStorable> GetEnumerator() => storable.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
