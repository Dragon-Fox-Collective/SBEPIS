using System.Linq;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(SplitTextureSetup))]
	public class SplitTextureSetupCardDequeSetter : MonoBehaviour
	{
		private SplitTextureSetup split;
		
		private void Awake()
		{
			split = GetComponent<SplitTextureSetup>();
		}
		
		public void UpdateTextures(DequeElement card, Deque deque)
		{
			split.Textures = deque.definition.ruleset.GetCardTextures().ToList();
		}
	}
}