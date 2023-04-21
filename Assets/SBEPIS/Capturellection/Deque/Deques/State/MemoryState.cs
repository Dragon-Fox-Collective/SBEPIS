using System;
using System.Collections.Generic;
using SBEPIS.Capturellection.Storage;

namespace SBEPIS.Capturellection.Deques
{
	[Serializable]
	public class MemoryState : DequeRulesetState
	{
		public Dictionary<Storable, (Storable, Storable)> backingInventory = new();
	}
}
