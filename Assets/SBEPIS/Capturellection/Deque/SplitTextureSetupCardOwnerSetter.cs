using System.Linq;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(SplitTextureSetup))]
	public class SplitTextureSetupCardOwnerSetter : MonoBehaviour
	{
		public Inventory inventory;
		
		private SplitTextureSetup split;
		
		private void Awake()
		{
			split = GetComponent<SplitTextureSetup>();
		}
		
		public void UpdateTexture(DequeStorable card, DequeOwner owner)
		{
			if (owner && owner.Deque)
				split.textures = inventory.GetCardTextures(card).ToList();
		}
	}
}