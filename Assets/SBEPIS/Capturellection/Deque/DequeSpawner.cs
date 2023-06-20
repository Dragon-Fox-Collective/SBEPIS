using System.Collections.Generic;
using KBCore.Refs;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DequeSpawner : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private GameObject dequeBoxPrefab;
		[SerializeField] private List<DequeRuleset> rulesets;
		
		private void Start()
		{
			rulesets.ForEach(SpawnDeque);
		}
		
		private void SpawnDeque(DequeRuleset ruleset)
		{
			GameObject systemGameObject = Instantiate(dequeBoxPrefab, transform);
			DequeBox dequeBox = systemGameObject.GetComponentInChildren<DequeBox>();
			
			GameObject definitionGameObject = new("Definition");
			definitionGameObject.transform.SetParent(dequeBox.transform);
			
			StorableGroupDefinition definition = definitionGameObject.AddComponent<StorableGroupDefinition>();
			definition.Init(ruleset, 128, null);

			systemGameObject.name = definition.DequeName;
			dequeBox.Deque.Init(definition);
		}
	}
}
