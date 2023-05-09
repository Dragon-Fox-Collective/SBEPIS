using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Capturellection.Storage;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class Inventory : MonoBehaviour, IEnumerable<InventoryStorable>
	{
		[SerializeField, Anywhere] public Deque deque;
		[FormerlySerializedAs("cardPrefab")]
		[SerializeField, Anywhere] private InventoryStorable initialCardPrefab;
		[SerializeField] public int initialCardCount = 0;
		
		[Tooltip("Purely organizational for the hierarchy")]
		[SerializeField, Anywhere] private Transform cardParent;
		public Transform CardParent => cardParent;
		
		public UnityEvent<Inventory> onLoadIntoDeque = new();
		public UnityEvent<Inventory, List<InventoryStorable>> onSaveFromDeque = new();
		
		private List<InventoryStorable> savedInventory = new();
		private Storable storable;
		
		private void Awake()
		{
			SaveInitialInventory();
			LoadInventoryIntoDeque(deque.Definition, deque.transform);
		}
		
		private void SaveInitialInventory()
		{
			for (int _ = 0; _ < initialCardCount; _++)
			{
				InventoryStorable card = Instantiate(initialCardPrefab);
				savedInventory.Add(card);
			}
		}
		
		private void LoadInventoryIntoDeque(StorableGroupDefinition definition, Transform ejectTransform)
		{
			storable = StorableGroupDefinition.GetNewStorable(definition);
			Load(savedInventory);
			foreach (InventoryStorable card in savedInventory)
			{
				print($"Ejecting leftover card {card}");
				card.gameObject.SetActive(true);
				card.transform.SetPositionAndRotation(ejectTransform.position, ejectTransform.rotation);
			}
			savedInventory.Clear();
			onLoadIntoDeque.Invoke(this);
		}
		
		private void SaveInventoryFromDeque()
		{
			savedInventory = Save();
			Destroy(storable.gameObject);
			onSaveFromDeque.Invoke(this, savedInventory);
		}

		private void SetupCard(InventoryStorable card)
		{
			card.Inventory = this;
			card.transform.SetParent(cardParent);
		}
		private static void TearDownCard(InventoryStorable card)
		{
			card.Inventory = null;
			card.transform.SetParent(null);
		}
		
		public void SetStorableParent(Transform transform) => storable.transform.SetParent(transform);
		
		public Vector3 Direction
		{
			get => storable.state.Direction;
			set => storable.state.Direction = value;
		}
		public Vector3 Position
		{
			get => storable.Position;
			set => storable.Position = value;
		}
		public Quaternion Rotation
		{
			get => storable.Rotation;
			set => storable.Rotation = value;
		}
		public Vector3 MaxPossibleSize => storable.MaxPossibleSize;
		public void Tick(float deltaTime) => storable.Tick(deltaTime);
		public void LayoutTarget(InventoryStorable card, CardTarget target) => storable.LayoutTarget(card, target);
		public bool CanFetch(InventoryStorable card) => storable.CanFetch(card);
		public async UniTask<StorableStoreResult> StoreItem(Capturellectable item)
		{
			StorableStoreResult res = await storable.StoreItem(item);
			if (res.card) SetupCard(res.card);
			return res;
		}
		public async UniTask<Capturellectable> FetchItem(InventoryStorable card)
		{
			Capturellectable item = await storable.FetchItem(card);
			if (item) TearDownCard(card);
			return item;
		}
		public UniTask FlushCard(InventoryStorable card) => FlushCard(new List<InventoryStorable>{ card });
		public UniTask FlushCard(List<InventoryStorable> cards)
		{
			foreach (InventoryStorable card in cards)
				SetupCard(card);
			return storable.FlushCards(cards);
		}
		public async UniTask<InventoryStorable> FetchCard(InventoryStorable card)
		{
			InventoryStorable fetchedCard = await storable.FetchCard(card);
			if (fetchedCard) TearDownCard(card);
			return fetchedCard;
		}
		private void Load(List<InventoryStorable> cards)
		{
			foreach (InventoryStorable card in cards)
				SetupCard(card);
			storable.Load(cards);
		}
		private List<InventoryStorable> Save()
		{
			List<InventoryStorable> cards = new();
			storable.Save(cards);
			foreach (InventoryStorable card in cards)
				TearDownCard(card);
			return cards;
		}
		public IEnumerable<Texture2D> GetCardTextures(InventoryStorable card) => storable.GetCardTextures(card);
		public IEnumerator<InventoryStorable> GetEnumerator() => storable.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
