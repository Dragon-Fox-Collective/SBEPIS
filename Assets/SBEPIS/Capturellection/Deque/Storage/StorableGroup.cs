using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class StorableGroup : MonoBehaviour, Storable
	{
		[SerializeField] private StorableGroupDefinition definition;
		public StorableGroupDefinition Definition => definition;
		
		private object state;
		private List<Storable> Inventory => ((InventoryState)state).Inventory;
		
		public void Init(StorableGroupDefinition definition)
		{
			this.definition = definition;
			state = definition.Ruleset.GetNewState();
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
		
		public void InitPage(DiajectorPage page) => definition.Ruleset.InitPage(state, page);
		
		public void Tick(float deltaTime) => definition.Ruleset.Tick(state, deltaTime);
		public void Layout(Vector3 direction)
		{
			((DirectionState)state).Direction = direction;
			definition.Ruleset.Layout(state);
		}
		public void LayoutTarget(InventoryStorable card, CardTarget target) => Inventory.Find(storable => storable.Contains(card)).LayoutTarget(card, target);
		
		public bool CanFetch(InventoryStorable card) => definition.Ruleset.CanFetch(state, card);
		public bool Contains(InventoryStorable card) => Inventory.Any(storable => storable.Contains(card));
		
		public async UniTask<StorableStoreResult> StoreItem(Capturellectable item)
		{
			DequeStoreResult res = await definition.Ruleset.StoreItem(state, item);
			res = await definition.Ruleset.StoreItemHook(state, item, res);
			
			if (res.ejectedItem && res.ejectedItem.TryGetComponent(out InventoryStorable flushedCard))
			{
				List<InventoryStorable> cards = new(){ flushedCard };
				await FlushCards(cards, res.flushIndex);
				if (cards.Count == 0)
					res.ejectedItem = null;
			}
			
			return res.ToStorableResult();
		}

		public async UniTask<Capturellectable> FetchItem(InventoryStorable card)
		{
			Capturellectable item = await definition.Ruleset.FetchItem(state, card);
			item = await definition.Ruleset.FetchItemHook(state, card, item);
			return item;
		}
		
		public async UniTask FlushCards(List<InventoryStorable> cards) => await FlushCards(cards, 0);
		private async UniTask FlushCards(List<InventoryStorable> cards, int originalIndex)
		{
			if (HasAllCards || cards.Count == 0)
				return;
			
			foreach (Storable storable in Inventory.Skip(originalIndex).Concat(Inventory.Take(originalIndex)))
			{
				await storable.FlushCards(cards);
				
				if (cards.Count == 0)
					break;
			}
			
			while (Inventory.Count < definition.MaxStorables)
			{
				Storable storable = StorableGroupDefinition.GetNewStorable(definition.Subdefinition);
				storable.Parent = transform;
				await storable.FlushCards(cards);
				
				IEnumerable<Storable> hookedStorables = await definition.Ruleset.FlushCardPreHook(state, storable);
				foreach (Storable hookedStorable in hookedStorables)
				{
					Inventory.Add(storable);
					await definition.Ruleset.FlushCardPostHook(state, hookedStorable);
				}
				
				if (cards.Count == 0)
					break;
			}
		}
		
		public async UniTask<InventoryStorable> FetchCard(InventoryStorable card)
		{
			Inventory.Remove(storableWithCard);
			card = await definition.Ruleset.FetchCardPostHook(state, card);
			return card;
		}
		
		public UniTask Interact<TState>(InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action) => definition.Ruleset.Interact(state, card, targetDeque, action);
		
		public IEnumerable<InventoryStorable> Load(IEnumerable<InventoryStorable> cards)
		{
			cards = LoadExistingStorables(cards);
			cards = LoadNewStorables(cards);
			return cards;
		}
		private IEnumerable<InventoryStorable> LoadExistingStorables(IEnumerable<InventoryStorable> cards) =>
			Inventory.Aggregate(cards, (current, storable) => storable.Load(current));
		private IEnumerable<InventoryStorable> LoadNewStorables(IEnumerable<InventoryStorable> cards)
		{
			while (cards.Any() && Inventory.Count < definition.MaxStorables)
			{
				Storable storable = StorableGroupDefinition.GetNewStorable(definition.Subdefinition);
				storable.Parent = transform;
				
				asdf
				cards = storable.Load(cards);
				
				IEnumerable<Storable> hookedStorables = definition.Ruleset.LoadStorablePreHook(state, storable);
				Inventory.AddRange(hookedStorables);
			}
			return cards;
		}
		
		public IEnumerable<InventoryStorable> Save()
		{
			IEnumerable<InventoryStorable> saved = Inventory.SelectMany(storable => definition.Ruleset.SaveStorablePostHook(state, storable)).SelectMany(SaveCard);
			
			Inventory.Clear();
			Destroy(gameObject);
			
			return saved;
		}
		private IEnumerable<InventoryStorable> SaveCard(Storable storable) =>
			storable.Save().SelectMany(card => definition.Ruleset.SaveCardPostHook(state, card));
		
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
