using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public abstract class SingleDeque<TSettingsData, TState> : MonoBehaviour, DequeRuleset<TState> where TState : DirectionState, new()
	{
		public string dequeNameSingluar;
		public string dequeNamePlural;
		public string firstPlaceDequeName;
		public string middlePlaceDequeName;
		public string lastPlaceDequeNameSingular;
		public string lastPlaceDequeNamePlural;
		
		public Dequeration dequeration;
		
		[Tooltip("Layout, capture, and fetch settings")]
		public DequeSettingsPageLayout<TSettingsData> settingsPagePrefab;
		[Tooltip("Layout and fetch settings only")]
		public DequeSettingsPageLayout<TSettingsData> firstPlaceSettingsPagePrefab;
		[Tooltip("Fetch settings only")]
		public DequeSettingsPageLayout<TSettingsData> middlePlaceSettingsPagePrefab;
		[Tooltip("Capture and fetch settings only")]
		public DequeSettingsPageLayout<TSettingsData> lastPlaceSettingsPagePrefab;
		
		public abstract void Tick(List<Storable> inventory, TState state, float deltaTime);
		public abstract Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, TState state);

		public virtual bool CanFetchFrom(List<Storable> inventory, TState state, InventoryStorable card) => inventory.Any(storable => storable.CanFetch(card));
		
		public virtual async UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, TState state, Capturellectable item)
		{
			int index = Mathf.Max(inventory.FindIndex(storable => !storable.HasAllCardsFull), 0);
			Storable storable = inventory[index];
			StorableStoreResult res = await storable.StoreItem(item);
			return res.ToDequeResult(index, storable);
		}
		public virtual UniTask<DequeStoreResult> StoreItemHook(List<Storable> inventory, TState state, Capturellectable item, DequeStoreResult oldResult) => UniTask.FromResult(oldResult);
		
		public virtual async UniTask<Capturellectable> FetchItem(List<Storable> inventory, TState state, InventoryStorable card)
		{
			Storable storable = StorableWithCard(inventory, card);
			Capturellectable item = await storable.FetchItem(card);
			return item;
		}
		public virtual UniTask<Capturellectable> FetchItemHook(List<Storable> inventory, TState state, InventoryStorable card, Capturellectable oldItem) => UniTask.FromResult(oldItem);
		
		protected static Storable StorableWithCard(List<Storable> inventory, InventoryStorable card) => inventory.Find(storable => storable.Contains(card));
		
		public virtual UniTask FlushCard(List<Storable> inventory, TState state, Storable storable)
		{
			inventory.Add(storable);
			return UniTask.CompletedTask;
		}
		public virtual UniTask<IEnumerable<Storable>> FlushCardPreHook(List<Storable> inventory, TState state, Storable storable) => UniTask.FromResult(LoadCardPreHook(inventory, state, storable));
		public virtual UniTask FlushCardPostHook(List<Storable> inventory, TState state, Storable storable) { LoadCardPostHook(inventory, state, storable); return UniTask.CompletedTask; }
		
		public virtual UniTask<InventoryStorable> FetchCard(List<Storable> inventory, TState state, InventoryStorable card)
		{
			Storable storable = StorableWithCard(inventory, card);
			inventory.Remove(storable);
			return UniTask.FromResult(card);
		}
		public virtual UniTask<InventoryStorable> FetchCardHook(List<Storable> inventory, TState state, InventoryStorable card) => UniTask.FromResult(SaveCardHook(inventory, state, card));
		
		public virtual IEnumerable<Storable> LoadCardPreHook(List<Storable> inventory, TState state, Storable storable) => ExtensionMethods.EnumerableOf(storable);
		public virtual void LoadCardPostHook(List<Storable> inventory, TState state, Storable storable) { }
		
		public virtual InventoryStorable SaveCardHook(List<Storable> inventory, TState state, InventoryStorable card) => card;
		
		public TState GetNewState() => new();
		
		public string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural) => isFirst && isLast ? isPlural ? dequeNamePlural : dequeNameSingluar : isFirst ? firstPlaceDequeName : isLast ? isPlural ? lastPlaceDequeNamePlural : lastPlaceDequeNameSingular : middlePlaceDequeName;
		
		public IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast)
		{
			DequeSettingsPageLayout<TSettingsData> prefab = isFirst && isLast ? settingsPagePrefab : isFirst ? firstPlaceSettingsPagePrefab : isLast ? lastPlaceSettingsPagePrefab : middlePlaceSettingsPagePrefab;
			if (prefab)
			{
				DequeSettingsPageLayout<TSettingsData> layout = Instantiate(prefab);
				layout.Object = SettingsPageLayoutData;
				yield return layout;
			}
		}
		protected abstract TSettingsData SettingsPageLayoutData { get; }
		
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
