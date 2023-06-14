using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Capturellection.Storage;
using SBEPIS.Utils.Linq;
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
		[SerializeField] private int initialCardCount = 0;
		public int InitialCardCount
		{
			get => initialCardCount;
			set => initialCardCount = value;
		}
		
		[Tooltip("Purely organizational for the hierarchy")]
		[SerializeField, Anywhere] private Transform cardParent;
		
		[SerializeField, Anywhere(Flag.Optional)] private DiajectorCaptureLayout layout;
		
		public UnityEvent<Inventory> onLoadIntoDeque = new();
		public UnityEvent<Inventory, List<InventoryStorable>> onSaveFromDeque = new();
		
		private List<InventoryStorable> savedInventory = new();
		private Storable storable;
		
		private void Start()
		{
			SaveInitialInventory();
			LoadInventoryIntoDeque(deque.Definition, deque.transform);
		}
		
		private void Update()
		{
			Tick(Time.deltaTime);
		}
		
		private void SaveInitialInventory() => savedInventory.AddRange(initialCardCount, () => Instantiate(initialCardPrefab).GetComponentInChildren<InventoryStorable>());
		
		private void LoadInventoryIntoDeque(StorableGroupDefinition definition, Transform ejectTransform)
		{
			if (storable != null) throw new InvalidOperationException($"Inventory {this} is already loaded");
			
			storable = StorableGroupDefinition.GetNewStorable(definition);
			if (layout) storable.Parent = layout.transform;
			savedInventory.ForEach(SetupCard);
			storable.LoadInit(savedInventory);
			savedInventory.ForEach(TearDownCard);
			
			foreach (InventoryStorable card in savedInventory)
			{
				Debug.Log($"Ejecting leftover card {card}", this);
				card.gameObject.SetActive(true);
				card.transform.SetPositionAndRotation(ejectTransform.position, ejectTransform.rotation);
			}
			
			savedInventory.Clear();
			
			if (layout) storable.ForEach(layout.SyncAddNewCard);
			
			onLoadIntoDeque.Invoke(this);
		}
		private void UseExistingStorable(Storable existingStorable)
		{
			storable = existingStorable;
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
		public Vector3 MaxPossibleSize => storable.MaxPossibleSize;
		public void InitPage(DiajectorPage page) => storable.InitPage(page);
		public void Tick(float deltaTime) => storable.Tick(deltaTime);
		public void Layout(Vector3 direction) => storable.Layout(direction);
		public void LayoutTarget(InventoryStorable card, CardTarget target) => storable.LayoutTarget(card, target);
		public bool CanFetch(InventoryStorable card) => storable.CanFetch(card);
		public async UniTask<StoreResult> StoreItem(Capturellectable item)
		{
			item.IsBeingCaptured = true;
			StoreResult result = await storable.StoreItem(item);
			item.IsBeingCaptured = false;
			return result;
		}
		public async UniTask<FetchResult> FetchItem(InventoryStorable card)
		{
			card.IsBeingFetched = true;
			FetchResult result = await storable.FetchItem(card);
			card.IsBeingFetched = false;
			return result;
		}
		public UniTask Interact<TState>(InventoryStorable card, DequeRuleset targetRuleset, DequeInteraction<TState> action) => storable.Interact(card, targetRuleset, action);
		public void Load(InventoryStorable card)
		{
			List<InventoryStorable> cards = new(){ card };
			Load(cards);
			if (cards.Count > 0) Debug.Log($"Extra card {card}");
		}
		public void Load(List<InventoryStorable> cards)
		{
			cards.ForEach(SetupCard);
			storable.Load(cards);
			cards.ForEach(TearDownCard);
		}
		public List<InventoryStorable> Save() => storable.Save().Process(TearDownCard).ToList();
		public IEnumerable<Texture2D> GetCardTextures(InventoryStorable card) => storable.GetCardTextures(card);
		public IEnumerator<InventoryStorable> GetEnumerator() => storable.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
