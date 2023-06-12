using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public abstract class SingleRuleset<TState> : DequeRuleset<TState> where TState : InventoryState, DirectionState, new()
	{
		[SerializeField] private string dequeNameSingluar;
		[SerializeField] private string dequeNamePlural;
		[SerializeField] private string firstPlaceDequeName;
		[SerializeField] private string middlePlaceDequeName;
		[SerializeField] private string lastPlaceDequeNameSingular;
		[SerializeField] private string lastPlaceDequeNamePlural;
		
		[SerializeField] private Dequeration dequeration;
		
		[Tooltip("Layout, capture, and fetch settings")]
		[SerializeField] private DequeSettingsPageLayout settingsPagePrefab;
		[Tooltip("Layout and fetch settings only")]
		[SerializeField] private DequeSettingsPageLayout firstPlaceSettingsPagePrefab;
		[Tooltip("Fetch settings only")]
		[SerializeField] private DequeSettingsPageLayout middlePlaceSettingsPagePrefab;
		[Tooltip("Capture and fetch settings only")]
		[SerializeField] private DequeSettingsPageLayout lastPlaceSettingsPagePrefab;
		
		public override IEnumerable<DequeRuleset> Layer => ExtensionMethods.EnumerableOf(this);
		
		private bool initializedPage;
		protected override void InitPage(TState state, DiajectorPage page)
		{
			if (!initializedPage)
			{
				initializedPage = true;
				InitPageOnce(state, page);
			}
			state.Inventory.ForEach(storable => storable.InitPage(page));
		}
		protected virtual void InitPageOnce(TState state, DiajectorPage page) { }
		
		protected override void Tick(TState state, float deltaTime) => state.Inventory.ForEach(storable => storable.Tick(deltaTime));
		
		protected override bool CanFetch(TState state, InventoryStorable card) => state.Inventory.Any(storable => storable.CanFetch(card));
		
		protected override UniTask<StoreResult> StoreItem(TState state, Capturellectable item) => state.Inventory[Mathf.Max(state.Inventory.FindIndex(storable => !storable.HasAllCardsFull), 0)].StoreItem(item);
		protected override UniTask<StoreResult> StoreItemHook(TState state, Capturellectable item, StoreResult oldResult) => UniTask.FromResult(oldResult);
		
		protected override UniTask<FetchResult> FetchItem(TState state, InventoryStorable card) => StorableWithCard(state, card).FetchItem(card);
		protected override UniTask<FetchResult> FetchItemHook(TState state, InventoryStorable card, FetchResult oldResult) => UniTask.FromResult(oldResult);
		
		protected static Storable StorableWithCard(TState state, InventoryStorable card) => state.Inventory.Find(storable => storable.Contains(card));
		
		protected override UniTask Interact<TIState>(TState state, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TIState> action) => ReferenceEquals(targetDeque, this) ? action((TIState)(object)state, card) : StorableWithCard(state, card).Interact(card, targetDeque, action);
		
		protected override IEnumerable<InventoryStorable> LoadCardHook(TState state, InventoryStorable card) => ExtensionMethods.EnumerableOf(card);
		protected override IEnumerable<InventoryStorable> SaveCardHook(TState state, InventoryStorable card) => ExtensionMethods.EnumerableOf(card);
		
		protected override IEnumerable<Storable> LoadStorableHook(TState state, Storable storable) => ExtensionMethods.EnumerableOf(storable);
		protected override IEnumerable<Storable> SaveStorableHook(TState state, Storable storable) => ExtensionMethods.EnumerableOf(storable);
		
		public override object GetNewState() => new TState();
		
		public override string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural) => isFirst && isLast ? isPlural ? dequeNamePlural : dequeNameSingluar : isFirst ? firstPlaceDequeName : isLast ? isPlural ? lastPlaceDequeNamePlural : lastPlaceDequeNameSingular : middlePlaceDequeName;
		
		public override IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast)
		{
			DequeSettingsPageLayout prefab = isFirst && isLast ? settingsPagePrefab : isFirst ? firstPlaceSettingsPagePrefab : isLast ? lastPlaceSettingsPagePrefab : middlePlaceSettingsPagePrefab;
			if (prefab)
			{
				DequeSettingsPageLayout settingsModule = Instantiate(prefab);
				settingsModule.Settings = DequeSettings;
				yield return settingsModule;
			}
		}
		protected abstract object DequeSettings { get; }
		
		public override IEnumerable<Texture2D> GetCardTextures()
		{
			yield return dequeration.cardTexture;
		}
		
		public override IEnumerable<Texture2D> GetBoxTextures()
		{
			yield return dequeration.boxTexture;
		}
		
		public override DequeRuleset Duplicate(Transform parent) => Instantiate(this, parent);
	}
}
