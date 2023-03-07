using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeBase : DequeRuleset
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
