using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class StorableGroup : MonoBehaviour, Storable
	{
		[SerializeField] private StorableGroupDefinition definition;
		
		private object state;
		private CallbackList<Storable> Inventory => ((InventoryState)state).Inventory;
		
		public void Init(StorableGroupDefinition definition)
		{
			this.definition = definition;
			state = definition.Ruleset.GetNewState();
			Inventory.onAddItem.AddListener(item => item.ForEach(GetComponentInParent<DiajectorCaptureLayout>().SyncAddNewCard));
			Inventory.onRemoveItem.AddListener(item => item.ForEach(GetComponentInParent<DiajectorCaptureLayout>().SyncRemoveOldCard));
		}
		
		public Vector3 Position
		{
			get => transform.localPosition;
			set => transform.localPosition = value;
		}
		public Quaternion Rotation
		{
			get => transform.localRotation;
			set => transform.localRotation = value;
		}
		public Transform Parent
		{
			set => transform.SetParent(value);
			get => transform.parent;
		}
		
		public Vector3 MaxPossibleSize => definition.Ruleset.GetMaxPossibleSizeOf(state);
		
		public int InventoryCount => Inventory.Count;
		
		public bool HasNoCards => Inventory.Count == 0;
		public bool HasAllCards => Inventory.Count == definition.MaxStorables && Inventory.All(storable => storable.HasAllCards);
		
		public bool HasAllCardsEmpty => Inventory.All(storable => storable.HasAllCardsEmpty);
		public bool HasAllCardsFull => Inventory.All(storable => storable.HasAllCardsFull);
		
		public bool HasNoContainers => Inventory.All(storable => storable.HasNoContainers);
		
		public void InitPage(DiajectorPage page) => definition.Ruleset.InitPage(state, page);
		
		public void Tick(float deltaTime) => definition.Ruleset.Tick(state, deltaTime);
		public void Layout(Vector3 direction)
		{
			((DirectionState)state).Direction = direction;
			definition.Ruleset.Layout(state);
		}
		public void LayoutTarget(InventoryStorable card, CardTarget target) => Inventory.Find(storable => storable.Contains(card)).LayoutTarget(card, target);
		
		public bool CanFetch(InventoryStorable card) => Contains(card) && Inventory.First(storable => storable.Contains(card)).CanFetch(card) && definition.Ruleset.CanFetch(state, card);
		public bool Contains(InventoryStorable card) => Inventory.Any(storable => storable.Contains(card));
		
		public async UniTask<StoreResult> StoreItem(Capturellectable item)
		{
			if (HasNoContainers) return new StoreResult();
			StoreResult res = await definition.Ruleset.StoreItem(state, item);
			res = await definition.Ruleset.StoreItemHook(state, item, res);
			return res;
		}
		
		public async UniTask<FetchResult> FetchItem(InventoryStorable card)
		{
			if (HasNoContainers) return new FetchResult();
			FetchResult res = await definition.Ruleset.FetchItem(state, card);
			res = await definition.Ruleset.FetchItemHook(state, card, res);
			return res;
		}
		
		public UniTask Interact<TState>(InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action) => definition.Ruleset.Interact(state, card, targetDeque, action);
		
		public void Eject()
		{
			Inventory.ForEach(storable => storable.Eject());
		}
		
		public void LoadInit(List<InventoryStorable> cards) => Load(cards, true);
		public void Load(List<InventoryStorable> cards) => Load(cards, false);
		private void Load(List<InventoryStorable> cards, bool init)
		{
			if (init && Inventory.Any())
				throw new InvalidOperationException($"StorableGroup {this} tried to initialize load with existing inventory");
			
			if (!cards.Any()) return;
			
			List<InventoryStorable> newCards = cards.SelectMany(card => definition.Ruleset.LoadCardHook(state, card)).ToList();
			cards.Clear();
			
			LoadExistingStorables(newCards);
			LoadNewStorables(newCards, !init);
			
			cards.AddRange(newCards.SelectMany(card => definition.Ruleset.SaveCardHook(state, card)));
		}
		private void LoadExistingStorables(List<InventoryStorable> cards) => Inventory.ForEach(storable => storable.Load(cards));
		private void LoadNewStorables(List<InventoryStorable> cards, bool useCallbacks = true)
		{
			while (cards.Any() && Inventory.Count < definition.MaxStorables)
			{
				Storable storable = StorableGroupDefinition.GetNewStorable(definition.Subdefinition);
				storable.Parent = transform;
				storable.LoadInit(cards);
				
				IEnumerable<Storable> storables = definition.Ruleset.LoadStorableHook(state, storable);
				if (useCallbacks)
					Inventory.AddRange(storables);
				else
					Inventory.backingList.AddRange(storables);
			}
		}
		
		public IEnumerable<InventoryStorable> Save()
		{
			IEnumerable<InventoryStorable> saved = Inventory.SelectMany(storable => definition.Ruleset.SaveStorableHook(state, storable)).SelectMany(SaveCard);
			
			Inventory.Clear();
			Destroy(gameObject);
			
			return saved;
		}
		private IEnumerable<InventoryStorable> SaveCard(Storable storable) =>
			storable.Save().SelectMany(card => definition.Ruleset.SaveCardHook(state, card));
		
		public Storable GetNewStorableLikeThis() => StorableGroupDefinition.GetNewStorable(definition);
		
		public IEnumerable<Texture2D> GetCardTextures(InventoryStorable card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent)
		{
			if (Contains(card))
			{
				textures = textures.Append(definition.Ruleset.GetCardTextures());
				int index = Inventory.FindIndex(storable => storable.Contains(card));
				Storable storable = Inventory[index];
				return storable.GetCardTextures(card, textures, index);
			}
			else
				return definition.Ruleset.GetCardTextures();
		}
		
		public IEnumerator<InventoryStorable> GetEnumerator() => Inventory.SelectMany(storable => storable).GetEnumerator();
		
		private void OnDrawGizmosSelected() => Storable.DrawSize(MaxPossibleSize, transform, Color.magenta);
	}
}
