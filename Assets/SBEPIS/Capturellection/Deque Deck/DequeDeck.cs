using System;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using SBEPIS.Capturellection;
using SBEPIS.Capturellection.Storage;
using SBEPIS.Utils.Linq;
using TMPro;
using UnityEngine;

namespace SBEPIS.Utils
{
	public class DequeDeck : ValidatedMonoBehaviour
	{
		[SerializeField, Child] private DequeSensor[] slots;
		[SerializeField, Child] private Counter cardCount;
		
		[SerializeField, Anywhere] private TMP_Text dequeLabel;
		
		[SerializeField, Anywhere] private GameObject dequeBoxPrefab;
		
		[SerializeField, Anywhere] private Transform spawnPosition;
		
		[SerializeField] private string noDequeString = "No deques slotted";
		
		private StorableGroupDefinition definitionPrefab;
		
		private void Awake()
		{
			dequeLabel.text = noDequeString;
		}
		
		public void RefreshDeque()
		{
			if (definitionPrefab)
			{
				Destroy(definitionPrefab.gameObject);
				definitionPrefab = null;
			}
			
			List<List<StorableGroupDefinition>> definitions = slots.GroupBy(slot => slot.GroupIndex).Select(group => (group.Key, group.OrderBy(slot => slot.LayerIndex))).OrderBy(group => group.Key).Select(group => group.Item2.Select(slot => slot.DequeBox).Where(dequeBox => dequeBox).Select(dequeBox => dequeBox.Deque.Definition).ToList()).Where(layer => layer.Any()).ToList();
			if (definitions.Any())
			{
				GameObject definitionGameObject = new("Definition");
				
				IEnumerable<List<DequeRuleset>> rulesets = definitions.SelectMany(layer => layer.Select(definition => definition.Layers).Zip(LINQ.Flatten).Select(realLayer => realLayer.ToList()));
				definitionPrefab = rulesets.Reverse().Select(layer =>
				{
					if (layer.Count > 1)
					{
						GameObject layerRulesetGameObject = new();
						layerRulesetGameObject.transform.SetParent(definitionGameObject.transform);
						
						LayerRuleset layerRuleset = layerRulesetGameObject.AddComponent<LayerRuleset>();
						layerRuleset.Init(layer.Select(ruleset => ruleset.Duplicate(layerRulesetGameObject.transform)));
						
						layerRuleset.name = layerRuleset.GetDequeNamePart(true, true, false);
						return layerRuleset;
					}
					else
						return layer[0].Duplicate(definitionGameObject.transform);
				}).Aggregate((StorableGroupDefinition)null, (topDefinition, ruleset) =>
				{
					StorableGroupDefinition definition = definitionGameObject.AddComponent<StorableGroupDefinition>();
					definition.Init(ruleset, 8, topDefinition);
					return definition;
				});
				definitionPrefab.MaxStorables = 128;
				
				dequeLabel.text = definitionPrefab.DequeName;
			}
			else
				dequeLabel.text = noDequeString;
		}
		
		public void CreateDeque()
		{
			if (!definitionPrefab) return;
			
			DequeBox dequeBox = Instantiate(dequeBoxPrefab).GetComponentInChildren<DequeBox>();
			dequeBox.transform.SetPositionAndRotation(spawnPosition);
			
			Inventory inventory = dequeBox.GetComponentInChildren<Inventory>();
			inventory.InitialCardCount = cardCount.Count;
			
			StorableGroupDefinition newDefinition = Instantiate(definitionPrefab, dequeBox.transform);
			dequeBox.Deque.Init(newDefinition);
		}
	}
}
