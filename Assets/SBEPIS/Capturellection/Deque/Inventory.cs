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
	public class Inventory : ValidatedMonoBehaviour, IEnumerable<InventoryStorable>
	{
		[SerializeField, Anywhere] public Deque deque;
		[FormerlySerializedAs("cardPrefab")]
		[SerializeField, Anywhere] private GameObject initialCardPrefab;
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
		
		private void Update()
		{
			Tick(Time.deltaTime);
		}
		
		private void SaveInitialInventory()
		{
			for (int _ = 0; _ < initialCardCount; _++)
			{
				InventoryStorable card = Instantiate(initialCardPrefab).GetComponentInChildren<InventoryStorable>();
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
			onSaveFromDeque.Invoke(this, savedInventory);
		}
		
		private void SetupCard(InventoryStorable card)
		{
			card.Inventory = this;
			card.DequeElement.SetParent(cardParent);
		}
		private static void TearDownCard(InventoryStorable card)
		{
			card.Inventory = null;
			card.DequeElement.SetParent(null);
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
		public void SetStorableParent(Transform transform) => storable.Parent = transform;
		public Vector3 MaxPossibleSize => storable.MaxPossibleSize;
		public void SetupPage(DiajectorPage page) => storable.SetupPage(page);
		public void Tick(float deltaTime) => storable.Tick(deltaTime);
		public void Layout(Vector3 direction) => storable.Layout(direction);
		public void LayoutTarget(InventoryStorable card, CardTarget target) => storable.LayoutTarget(card, target);
		public bool CanFetch(InventoryStorable card) => storable.CanFetch(card);
		public async UniTask<StorableStoreResult> StoreItem(Capturellectable item)
		{
			item.IsBeingCaptured = true;
			StorableStoreResult result = await storable.StoreItem(item);
			item.IsBeingCaptured = false;
			return result;
		}
		public async UniTask<Capturellectable> FetchItem(InventoryStorable card)
		{
			card.IsBeingFetched = true;
			Capturellectable item = await storable.FetchItem(card);
			card.IsBeingFetched = false;
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
		public UniTask Interact<TState>(InventoryStorable card, DequeRuleset targetRuleset, DequeInteraction<TState> action) => storable.Interact(card, targetRuleset, action);
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
