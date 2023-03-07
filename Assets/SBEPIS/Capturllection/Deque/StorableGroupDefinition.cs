using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class StorableGroupDefinition : MonoBehaviour
	{
		public DequeRuleset ruleset;
		public int maxStorables;
		public StorableGroupDefinition subdefinition;
		
		public string dequeName => ruleset.dequeName + (subdefinition ? $"of {subdefinition.name}" : "");
		
		public static Storable GetNewStorable(StorableGroupDefinition definition)
		{
			GameObject childGameObject = new();
			if (definition)
			{
				childGameObject.name = definition.dequeName;
				StorableGroup group = childGameObject.AddComponent<StorableGroup>();
				group.definition = definition;
				return group;
			}
			else
			{
				childGameObject.name = "Slot";
				StorableSlot slot = childGameObject.AddComponent<StorableSlot>();
				return slot;
			}
		}
	}
}
