using KBCore.Refs;
using SBEPIS.Capturellection;
using UnityEngine;

namespace SBEPIS.Tests.Scenes
{
	public class InventoryScene : MonoBehaviour
	{
		[Anywhere] public Inventory inventory;
		[Anywhere] public Capturellectable item;
		
		private void OnValidate() => this.ValidateRefs();
	}
}