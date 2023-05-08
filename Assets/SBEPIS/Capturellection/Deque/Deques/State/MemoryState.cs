using System;
using System.Collections.Generic;
using SBEPIS.Capturellection.Storage;

namespace SBEPIS.Capturellection.Deques
{
	[Serializable]
	public class MemoryState : DequeRulesetState
	{
		public Storable flippedCard;
		public Dictionary<Storable, Storable> pairs = new();
		public Storable FlippedPair => flippedCard ? pairs[flippedCard] : null;
	}
}
