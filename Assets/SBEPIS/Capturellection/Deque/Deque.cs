using System;
using KBCore.Refs;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class Deque : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private StorableGroupDefinition definition;
		public StorableGroupDefinition Definition => definition;
		
		public void Init(StorableGroupDefinition definition)
		{
			if (this.definition && this.definition.Ruleset) throw new InvalidOperationException($"Deque {this} is already initialized");
			this.definition = definition;
		}
	}
}
