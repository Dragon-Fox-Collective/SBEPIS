using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public abstract class DequeBase<T> : DequeRuleset<T> where T : DequeRulesetState, new()
	{
		public string dequeNameSingluar;
		public string dequeNamePlural;
		public string firstPlaceDequeName;
		public string middlePlaceDequeName;
		public string lastPlaceDequeNameSingular;
		public string lastPlaceDequeNamePlural;
		
		public Dequeration dequeration;
		
		[Tooltip("Layout, capture, and fetch settings")]
		public DequeSettingsPageLayout settingsPagePrefab;
		[Tooltip("Layout and fetch settings only")]
		public DequeSettingsPageLayout firstPlaceSettingsPagePrefab;
		[Tooltip("Fetch settings only")]
		public DequeSettingsPageLayout middlePlaceSettingsPagePrefab;
		[Tooltip("Capture and fetch settings only")]
		public DequeSettingsPageLayout lastPlaceSettingsPagePrefab;
		
		public override string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural) => isFirst && isLast ? isPlural ? dequeNamePlural : dequeNameSingluar : isFirst ? firstPlaceDequeName : isLast ? isPlural ? lastPlaceDequeNamePlural : lastPlaceDequeNameSingular : middlePlaceDequeName;
		
		public override IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast)
		{
			DequeSettingsPageLayout prefab = isFirst && isLast ? settingsPagePrefab : isFirst ? firstPlaceSettingsPagePrefab : isLast ? lastPlaceSettingsPagePrefab : middlePlaceSettingsPagePrefab;
			if (prefab)
			{
				DequeSettingsPageLayout layout = Instantiate(prefab);
				layout.Ruleset = this;
				yield return layout;
			}
		}
		
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, T state) => GetSizeFromExistingLayout(inventory);
		public static Vector3 GetSizeFromExistingLayout(IEnumerable<Storable> inventory) =>
			inventory.Select(storable => new Bounds(storable.Position, storable.MaxPossibleSize)).Aggregate(new Bounds(), (current, bounds) => current.Containing(bounds)).size;
		
		public override bool CanFetchFrom(List<Storable> inventory, T state, InventoryStorable card) => inventory.Any(storable => storable.CanFetch(card));
		
		public override async UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, T state, Capturellectable item)
		{
			int index = Mathf.Max(inventory.FindIndex(storable => !storable.HasAllCardsFull), 0);
			Storable storable = inventory[index];
			StorableStoreResult res = await storable.StoreItem(item);
			return res.ToDequeResult(index, storable);
		}
		
		public override async UniTask<Capturellectable> FetchItem(List<Storable> inventory, T state, InventoryStorable card)
		{
			Storable storable = inventory.Find(storable => storable.Contains(card));
			Capturellectable item = await storable.FetchItem(card);
			return item;
		}
		
		public override UniTask FlushCard(List<Storable> inventory, T state, Storable storable)
		{
			inventory.Add(storable);
			return UniTask.CompletedTask;
		}
		
		public override UniTask<InventoryStorable> FetchCard(List<Storable> inventory, T state, InventoryStorable card)
		{
			Storable storable = inventory.Find(storable => storable.Contains(card));
			inventory.Remove(storable);
			return UniTask.FromResult(card);
		}
		
		public override IEnumerable<Texture2D> GetCardTextures()
		{
			yield return dequeration.cardTexture;
		}
		
		public override IEnumerable<Texture2D> GetBoxTextures()
		{
			yield return dequeration.boxTexture;
		}
	}
}
