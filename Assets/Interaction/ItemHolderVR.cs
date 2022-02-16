using SBEPIS.Alchemy;
using System.Collections.Generic;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction
{
	public class ItemHolderVR : MonoBehaviour
	{
		public Item heldItem { get; private set; }

		private List<Item> collidingItems = new List<Item>();

		/// <summary>
		/// Handles both picking up items and pressing buttons
		/// </summary>
		public void OnPickUp(CallbackContext context)
		{
			bool isPressed = context.performed;
			
			/*if (isPressed && !heldItem && Cast(out hit))
			{
				Item hitItem = hit.rigidbody.GetComponent<Item>();
				if (hitItem && hitItem.canPickUp)
					(heldItem = hitItem).OnPickedUp(this);
			}
			else if (!isPressed && heldItem)
			{
				Item droppedItem = DropItem();
				if (RaycastPlacementHelper(out PlacementHelper placement, droppedItem.itemkind))
					placement.Adopt(droppedItem);
			}*/
		}

		private void OnCollisionEnter(Collision collision)
		{
			Item hitItem = collision.gameObject.GetComponent<Item>();
			if (hitItem && hitItem.canPickUp)
				collidingItems.Add(hitItem);
		}

		private void OnCollisionExit(Collision collision)
		{
			Item hitItem = collision.gameObject.GetComponent<Item>();
			if (hitItem && hitItem.canPickUp)
				collidingItems.Remove(hitItem);
		}
	}
}