using System.Linq;
using KBCore.Refs;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(SplitTextureSetup))]
	public class SplitTextureSetupCardInventorySetter : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private SplitTextureSetup split;
		
		public void UpdateTextures(InventoryStorable card, Inventory inventory)
		{
			split.Textures = inventory.GetCardTextures(card).ToList();
		}
	}
}