using System.Linq;
using KBCore.Refs;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(SplitTextureSetup))]
	public class SplitTextureSetupCardDequeSetter : MonoBehaviour
	{
		[SerializeField, Self]
		private SplitTextureSetup split;
		
		private void OnValidate() => this.ValidateRefs();
		
		public void UpdateTextures(DequeElement card, Deque deque)
		{
			split.Textures = deque.Definition.Ruleset.GetCardTextures().ToList();
		}
	}
}