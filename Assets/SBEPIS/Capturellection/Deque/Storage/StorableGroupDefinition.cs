using System;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class StorableGroupDefinition : MonoBehaviour
	{
		[SerializeField] private DequeRuleset ruleset;
		public DequeRuleset Ruleset => ruleset;
		[SerializeField] private int maxStorables;
		public int MaxStorables
		{
			get => maxStorables;
			set => maxStorables = value;
		}
		//public int MaxCardsPerStorable => subdefinition ? subdefinition.MaxStorables * subdefinition.MaxCardsPerStorable : 1;
		[SerializeField] private StorableGroupDefinition subdefinition;
		public StorableGroupDefinition Subdefinition => subdefinition;
		
		public string DequeName => Ruleset.GetDequeNamePart(true, true, false) + (subdefinition ? " of " + subdefinition.DequeNamePlural : "");
		public string DequeNamePlural => Ruleset.GetDequeNamePart(true, true, true) + (subdefinition ? " of " + subdefinition.DequeNamePlural : "");
		
		public IEnumerable<IEnumerable<DequeRuleset>> Layers
		{
			get
			{
				yield return Ruleset.Layer;
				
				if (subdefinition)
					foreach (IEnumerable<DequeRuleset> layer in subdefinition.Layers)
						yield return layer;
			}
		}
		
		public void Init(DequeRuleset ruleset, int maxStorables, StorableGroupDefinition subdefinition)
		{
			if (this.ruleset) throw new InvalidOperationException($"Definition {this} has already been initialized");
			this.ruleset = Instantiate(ruleset, transform);
			this.maxStorables = maxStorables;
			this.subdefinition = subdefinition;
		}
		
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
