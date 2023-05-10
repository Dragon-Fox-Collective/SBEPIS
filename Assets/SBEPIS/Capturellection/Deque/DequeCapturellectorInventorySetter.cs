using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Inventory))]
	public class DequeCapturellectorInventorySetter : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private Inventory inventory;
		
		public void BindToPlayer(PlayerReference playerReference)
		{
			CapturellectorInventorySetter inventorySetter = playerReference.GetReferencedComponent<CapturellectorInventorySetter>();
			inventorySetter.SetNewInventory(inventory);
		}
	}
}