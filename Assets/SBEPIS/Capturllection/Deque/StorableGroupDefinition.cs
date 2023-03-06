using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[Serializable]
	public class StorableGroupDefinition
	{
		public DequeRuleset ruleset;
		public int maxStorables;
		public StorableGroupDefinition subdefinition;

		public Storable GetNewStorable() => subdefinition != null ? new StorableGroup(subdefinition) : new StorableSlot();
	}
}
