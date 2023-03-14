using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class StorableGroupDefinition : MonoBehaviour
	{
		public DequeRuleset ruleset;
		public int maxStorables;
		public StorableGroupDefinition subdefinition;
		
		public string dequeName => ruleset.dequeName + (subdefinition ? " of " + subdefinition.dequeName : "");
		
		public static Storable GetNewStorable(StorableGroupDefinition definition)
		{
			GameObject childGameObject = new();
			if (definition)
			{
				childGameObject.name = definition.dequeName;
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
				layout.title.text = dequeName;
				yield return layout;
			}
			
			if (subdefinition)
				foreach (DequeSettingsPageLayout layout in subdefinition.GetNewSettingsPageLayouts())
					yield return layout;
		}
	}
}
