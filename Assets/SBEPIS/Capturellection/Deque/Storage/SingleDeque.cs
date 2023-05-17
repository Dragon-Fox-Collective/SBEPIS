using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public abstract class SingleDeque<TState> : MonoBehaviour, DequeRuleset<TState> where TState : InventoryState, DirectionState, new()
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
		
		public virtual void SetupPage(TState state, DiajectorPage page) { }
		
		public abstract void Tick(TState state, float deltaTime);
		public abstract void Layout(TState state);
		public abstract Vector3 GetMaxPossibleSizeOf(TState state);

		public virtual bool CanFetch(TState state, InventoryStorable card) => state.Inventory.Any(storable => storable.CanFetch(card));
		
		public virtual async UniTask<DequeStoreResult> StoreItem(TState state, Capturellectable item)
		{
			int index = Mathf.Max(state.Inventory.FindIndex(storable => !storable.HasAllCardsFull), 0);
			Storable storable = state.Inventory[index];
			StorableStoreResult res = await storable.StoreItem(item);
			return res.ToDequeResult(index, storable);
		}
		public virtual UniTask<DequeStoreResult> StoreItemHook(TState state, Capturellectable item, DequeStoreResult oldResult) => UniTask.FromResult(oldResult);
		
		public virtual UniTask<Capturellectable> FetchItem(TState state, InventoryStorable card) => StorableWithCard(state, card).FetchItem(card);
		public virtual UniTask<Capturellectable> FetchItemHook(TState state, InventoryStorable card, Capturellectable oldItem) => UniTask.FromResult(oldItem);
		
		protected static Storable StorableWithCard(TState state, InventoryStorable card) => state.Inventory.Find(storable => storable.Contains(card));
		
		public virtual UniTask FlushCard(TState state, Storable storable)
		{
			state.Inventory.Add(storable);
			return UniTask.CompletedTask;
		}
		public virtual UniTask<IEnumerable<Storable>> FlushCardPreHook(TState state, Storable storable) => UniTask.FromResult(LoadCardPreHook(state, storable));
		public virtual UniTask FlushCardPostHook(TState state, Storable storable) { LoadCardPostHook(state, storable); return UniTask.CompletedTask; }
		
		public virtual UniTask<InventoryStorable> FetchCard(TState state, InventoryStorable card)
		{
			Storable storable = StorableWithCard(state, card);
			state.Inventory.Remove(storable);
			return UniTask.FromResult(card);
		}
		public virtual UniTask<InventoryStorable> FetchCardHook(TState state, InventoryStorable card) => UniTask.FromResult(SaveCardHook(state, card));
		
		public UniTask Interact<TIState>(TState state, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TIState> action) => ReferenceEquals(targetDeque, this) ? action((TIState)(object)state, card) : StorableWithCard(state, card).Interact(card, targetDeque, action);
		
		public virtual IEnumerable<Storable> LoadCardPreHook(TState state, Storable storable) => ExtensionMethods.EnumerableOf(storable);
		public virtual void LoadCardPostHook(TState state, Storable storable) { }
		
		public virtual InventoryStorable SaveCardHook(TState state, InventoryStorable card) => card;
		
		public TState GetNewState() => new();
		
		public string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural) => isFirst && isLast ? isPlural ? dequeNamePlural : dequeNameSingluar : isFirst ? firstPlaceDequeName : isLast ? isPlural ? lastPlaceDequeNamePlural : lastPlaceDequeNameSingular : middlePlaceDequeName;
		
		public IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast)
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
		
		public IEnumerable<Texture2D> GetCardTextures()
		{
			yield return dequeration.cardTexture;
		}
		
		public IEnumerable<Texture2D> GetBoxTextures()
		{
			yield return dequeration.boxTexture;
		}
	}
}
