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
		
		public int NumEmptySlots => Inventory.Select(storable => storable.NumEmptySlots).Sum() + (definition.MaxStorables - Inventory.Count) * definition.MaxCardsPerStorable;
		
		public bool HasNoCards => Inventory.Count == 0;
		public bool HasAllCards => NumEmptySlots == 0;
		
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
		
		public async UniTask<StoreResult> StoreItem(Capturellectable item)
		{
			StoreResult res = await definition.Ruleset.StoreItem(state, item);
			res = await definition.Ruleset.StoreItemHook(state, item, res);
			return res;
		}
		
		public async UniTask<FetchResult> FetchItem(InventoryStorable card)
		{
			FetchResult res = await definition.Ruleset.FetchItem(state, card);
			res = await definition.Ruleset.FetchItemHook(state, card, res);
			return res;
		}
		
		public UniTask Interact<TState>(InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action) => definition.Ruleset.Interact(state, card, targetDeque, action);
		
		public void Load(List<InventoryStorable> cards)
		{
			if (!cards.Any()) return;
			int numCardsToTake = Mathf.Min(cards.Count, NumEmptySlots);
			List<InventoryStorable> newCards = cards.Take(numCardsToTake).SelectMany(card => definition.Ruleset.LoadCardHook(state, card)).ToList();
			cards.RemoveRange(0, numCardsToTake);
			
			LoadExistingStorables(newCards);
			LoadNewStorables(newCards);
		}
		private void LoadExistingStorables(List<InventoryStorable> cards) => Inventory.ForEach(storable => storable.Load(cards));
		private void LoadNewStorables(List<InventoryStorable> cards)
		{
			while (cards.Any())
			{
				Storable storable = StorableGroupDefinition.GetNewStorable(definition.Subdefinition);
				storable.Parent = transform;
				storable.Load(cards);
				Inventory.AddRange(definition.Ruleset.LoadStorableHook(state, storable));
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
