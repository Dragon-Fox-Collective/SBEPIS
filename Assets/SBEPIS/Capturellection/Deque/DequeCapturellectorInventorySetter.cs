using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Inventory))]
	public class DequeCapturellectorInventorySetter : MonoBehaviour
	{
		[SerializeField, Self]
		private Inventory inventory;
		
		private void OnValidate() => this.ValidateRefs();
		
		public void BindToPlayer(PlayerReference playerReference)
		{
			CapturellectorInventorySetter inventorySetter = playerReference.GetReferencedComponent<CapturellectorInventorySetter>();
			inventorySetter.SetNewInventory(inventory);
		}
	}
}