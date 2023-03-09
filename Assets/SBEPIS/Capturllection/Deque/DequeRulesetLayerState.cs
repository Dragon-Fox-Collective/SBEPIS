using System;
using System.Collections.Generic;

namespace SBEPIS.Capturllection
{
	[Serializable]
	public class DequeRulesetLayerState : DequeRulesetState
	{
		public List<DequeRulesetState> states;
	}
}
