using System;
using SBEPIS.Capturellection.Storage;

namespace SBEPIS.Capturellection.Deques
{
	[Serializable]
	public class CycloneState : DequeRulesetState
	{
		public float time;
		public Storable topStorable;
	}
}
