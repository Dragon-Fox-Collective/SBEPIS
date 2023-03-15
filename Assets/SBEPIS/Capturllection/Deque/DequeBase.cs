using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
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
				layout.ruleset = this;
				yield return layout;
			}
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
