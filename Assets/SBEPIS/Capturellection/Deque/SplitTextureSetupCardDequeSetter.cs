using System.Linq;
using KBCore.Refs;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(SplitTextureSetup))]
	public class SplitTextureSetupCardDequeSetter : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private SplitTextureSetup split;
		
		public void UpdateTextures(DequeElement card, Deque deque)
		{
			split.Textures = deque.Definition.Ruleset.GetCardTextures().ToList();
		}
	}
}