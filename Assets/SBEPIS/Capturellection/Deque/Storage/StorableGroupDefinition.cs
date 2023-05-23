using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class StorableGroupDefinition : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private InterfaceRef<DequeRuleset> ruleset;
		public DequeRuleset Ruleset => ruleset.Value;
		[SerializeField] private int maxStorables;
		public int MaxStorables => maxStorables;
		public int MaxCardsPerStorable => subdefinition ? subdefinition.MaxStorables * subdefinition.MaxCardsPerStorable : 1;
		[SerializeField, Anywhere(Flag.Optional)] private StorableGroupDefinition subdefinition;
		public StorableGroupDefinition Subdefinition => subdefinition;
		
		public string DequeName => Ruleset.GetDequeNamePart(true, true, false) + (subdefinition ? " of " + subdefinition.DequeNamePlural : "");
		public string DequeNamePlural => Ruleset.GetDequeNamePart(true, true, true) + (subdefinition ? " of " + subdefinition.DequeNamePlural : "");
		
		public static Storable GetNewStorable(StorableGroupDefinition definition) => definition ? GetNewStorableGroup(definition) : GetNewStorableSlot();
		private static Storable GetNewStorableGroup(StorableGroupDefinition definition)
		{
			GameObject childGameObject = new();
			childGameObject.name = definition.DequeName;
			StorableGroup group = childGameObject.AddComponent<StorableGroup>();
			group.Init(definition);
			return group;
		}
		private static Storable GetNewStorableSlot()
		{
			GameObject childGameObject = new();
			childGameObject.name = "Slot";
			StorableSlot slot = childGameObject.AddComponent<StorableSlot>();
			return slot;
		}
		
		public IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts()
		{
			foreach (DequeSettingsPageLayout layout in Ruleset.GetNewSettingsPageLayouts(true, true))
			{
				layout.title.text = DequeName;
				yield return layout;
			}
			
			if (subdefinition)
				foreach (DequeSettingsPageLayout layout in subdefinition.GetNewSettingsPageLayouts())
					yield return layout;
		}
	}
}
