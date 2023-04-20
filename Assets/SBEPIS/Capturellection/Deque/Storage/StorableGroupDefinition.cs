using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class StorableGroupDefinition : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private DequeRuleset ruleset;
		public DequeRuleset Ruleset => ruleset;
		[SerializeField] private int maxStorables;
		public int MaxStorables => maxStorables;
		[SerializeField, Anywhere(Flag.Optional)] private StorableGroupDefinition subdefinition;
		public StorableGroupDefinition Subdefinition => subdefinition;
		
		public string DequeName => ruleset.GetDequeNamePart(true, true, false) + (subdefinition ? " of " + subdefinition.DequeNamePlural : "");
		public string DequeNamePlural => ruleset.GetDequeNamePart(true, true, true) + (subdefinition ? " of " + subdefinition.DequeNamePlural : "");
		
		public static Storable GetNewStorable(StorableGroupDefinition definition)
		{
			GameObject childGameObject = new();
			if (definition)
			{
				childGameObject.name = definition.DequeName;
				StorableGroup group = childGameObject.AddComponent<StorableGroup>();
				group.definition = definition;
				group.state = definition.ruleset.GetNewState();
				return group;
			}
			else
			{
				childGameObject.name = "Slot";
				StorableSlot slot = childGameObject.AddComponent<StorableSlot>();
				slot.state = new BaseState();
				return slot;
			}
		}
		
		public IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts()
		{
			foreach (DequeSettingsPageLayout layout in ruleset.GetNewSettingsPageLayouts(true, true))
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
