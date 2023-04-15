using System.Linq;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(SplitTextureSetup))]
	public class SplitTextureSetupCardInventorySetter : MonoBehaviour
	{
		private SplitTextureSetup split;
		
		private void Awake()
		{
			split = GetComponent<SplitTextureSetup>();
		}
		
		public void UpdateTextures(InventoryStorable card, Inventory inventory)
		{
			split.Textures = inventory.GetCardTextures(card).ToList();
		}
	}
}