using System;
using KBCore.Refs;
using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Inventory))]
	public class DequeCapturellectorInventorySetter : MonoBehaviour
	{
		[SerializeField, Self]
		private Inventory inventory;
		
		private void OnValidate() => this.ValidateRefs();
		
		public void BindToPlayer(Grabber grabber, Grabbable grabbable)
		{
			if (!grabber.TryGetComponent(out PlayerReference playerReference))
				return;
			CapturellectorInventorySetter inventorySetter = playerReference.GetReferencedComponent<CapturellectorInventorySetter>();
			inventorySetter.SetNewInventory(inventory);
		}
	}
}