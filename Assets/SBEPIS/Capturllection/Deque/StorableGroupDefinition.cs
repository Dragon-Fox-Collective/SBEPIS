using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[Serializable]
	public struct StorableGroupDefinition
	{
		public DequeRuleset ruleset;
		public int maxStorables;
	}
}
