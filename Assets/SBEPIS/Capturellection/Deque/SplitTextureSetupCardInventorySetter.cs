using System.Linq;
using KBCore.Refs;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(SplitTextureSetup))]
	public class SplitTextureSetupCardInventorySetter : MonoBehaviour
	{
		[SerializeField, Self]
		private SplitTextureSetup split;
		
		private void OnValidate() => this.ValidateRefs();
		
		public void UpdateTextures(InventoryStorable card, Inventory inventory)
		{
			split.Textures = inventory.GetCardTextures(card).ToList();
		}
	}
}