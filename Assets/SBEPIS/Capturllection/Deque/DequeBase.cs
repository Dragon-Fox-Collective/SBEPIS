using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeBase<T> : DequeRuleset<T> where T : DequeRulesetState, new()
	{
		public Dequeration dequeration;
		
		public override string dequeName => GetType().Name;

		public override IEnumerable<Texture2D> GetCardTextures()
		{
			yield return dequeration.cardTexture;
		}
		
		public override IEnumerable<Texture2D> GetBoxTextures()
		{
			yield return dequeration.boxTexture;
		}
	}
}
