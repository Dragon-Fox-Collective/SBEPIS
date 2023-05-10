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
		public Vector3 Direction
		{
			get => ((DirectionState)state).Direction;
			set => ((DirectionState)state).Direction = value;
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
		
		public void Tick(float deltaTime) => definition.Ruleset.Tick(state, deltaTime);
		public void LayoutTarget(InventoryStorable card, CardTarget target) => Inventory.Find(storable => storable.Contains(card)).LayoutTarget(card, target);
		
		public bool CanFetch(InventoryStorable card) => definition.Ruleset.CanFetchFrom(state, card);
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
					await definition.Ruleset.FlushCard(state, hookedStorable);
					await definition.Ruleset.FlushCardPostHook(state, hookedStorable);
				}
				
				if (cards.Count == 0)
					break;
			}
		}
		
		public async UniTask<InventoryStorable> FetchCard(InventoryStorable card)
		{
			card = await definition.Ruleset.FetchCard(state, card);
			card = await definition.Ruleset.FetchCardHook(state, card);
			return card;
		}
		
		public UniTask Interact<TState>(InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action) => definition.Ruleset.Interact(state, card, targetDeque, action);
		
		public void Load(List<InventoryStorable> cards)
		{
			if (HasAllCards || cards.Count == 0)
				return;
			
			foreach (Storable storable in Inventory)
			{
				storable.Load(cards);
				if (cards.Count == 0)
					break;
			}
			
			while (Inventory.Count < definition.MaxStorables)
			{
				Storable storable = StorableGroupDefinition.GetNewStorable(definition.Subdefinition);
				storable.Parent = transform;
				storable.Load(cards);
				
				IEnumerable<Storable> hookedStorables = definition.Ruleset.LoadCardPreHook(state, storable);
				foreach (Storable hookedStorable in hookedStorables)
				{
					Inventory.Add(hookedStorable);
					definition.Ruleset.LoadCardPostHook(state, hookedStorable);
				}

				if (cards.Count == 0)
					break;
			}
		}
		public void Save(List<InventoryStorable> cards)
		{
			if (HasNoCards)
				return;
			
			foreach (Storable storable in Inventory.ToList())
			{
				int initialCount = cards.Count;
				
				storable.Save(cards);
				Inventory.Remove(storable);
				
				for (int i = initialCount; i < cards.Count; i++)
				{
					InventoryStorable newCard = cards[i] = definition.Ruleset.SaveCardHook(state, cards[i]);
					if (!newCard) cards.RemoveAt(i--);
				}
			}
			
			Destroy(gameObject);
		}
		
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
				return definition.Ruleset.GetCardTextures().ToList();
		}
		
		public IEnumerator<InventoryStorable> GetEnumerator() => Inventory.SelectMany(storable => storable).GetEnumerator();
		
		private void OnDrawGizmosSelected() => Storable.DrawSize(MaxPossibleSize, transform, Color.magenta);
	}
}
