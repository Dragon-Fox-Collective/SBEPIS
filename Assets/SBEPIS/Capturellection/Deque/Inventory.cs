using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(DequeOwner))]
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

		private DequeOwner dequeOwner;
		
		private List<DequeStorable> savedInventory;
		private Storable storable;
		
		private void Awake()
		{
			dequeOwner = GetComponent<DequeOwner>();
			
			dequeOwner.dequeEvents.onSet.AddListener(LoadInventoryIntoDeque);
			dequeOwner.dequeEvents.onUnset.AddListener(SaveInventoryFromDeque);
			dequeOwner.cardOwnerSlaveEvents.onSet.AddListener(SetCardParent);
			dequeOwner.cardOwnerSlaveEvents.onUnset.AddListener(UnsetCardParent);
			
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
		
		private void LoadInventoryIntoDeque(DequeOwner dequeOwner, Deque deque)
		{
			storable = StorableGroupDefinition.GetNewStorable(deque.definition);
			foreach (DequeStorable card in savedInventory)
				card.DequeOwner = dequeOwner;
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
		
		private void SaveInventoryFromDeque(DequeOwner dequeOwner, Deque oldDeque, Deque newDeque)
		{
			savedInventory = storable.ToList();
			Destroy(storable.gameObject);
			foreach (DequeStorable card in savedInventory)
				card.DequeOwner = null;
			onSaveFromDeque.Invoke(savedInventory);
		}
		
		private void SetCardParent(DequeStorable card, DequeOwner owner) => card.transform.SetParent(cardParent);
		private static void UnsetCardParent(DequeStorable card, DequeOwner oldOwner, DequeOwner newOwner) => card.transform.SetParent(null);
		
		public bool CanFetch(DequeStorable card) => storable.CanFetch(card);
		public UniTask<Capturellectable> Fetch(DequeStorable card) => storable.Fetch(card);
		public UniTask<(DequeStorable, Capturellectainer, Capturellectable)> Store(Capturellectable item) => storable.Store(item);
		public IEnumerable<Texture2D> GetCardTextures(DequeStorable card) => storable.GetCardTextures(card);
		public IEnumerator<DequeStorable> GetEnumerator() => storable.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
