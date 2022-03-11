using SBEPIS.Alchemy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction.XR
{
	[RequireComponent(typeof(Rigidbody))]
	public class ItemHolderVR : ItemHolder
	{
		public Transform model;

		public Item heldItem { get; private set; }
		private new Rigidbody rigidbody;

		private List<Item> collidingItems = new List<Item>();
		private List<PlacementHelper> collidingPlacements = new List<PlacementHelper>();

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			//FixedJoint joint = model.gameObject.AddComponent<FixedJoint>();
			//joint.connectedBody = rigidbody;
		}

		private void FixedUpdate()
		{
			if (!heldItem)
				return;

			heldItem.OnHeld(this);

			if (OverlapPlacementHelper(out PlacementHelper placement, heldItem.itemkind))
				Flatscreen.ItemHolderFlatscreen.UpdateItemSnapToPlacementHelper(heldItem, placement);
			else
				UpdateItem(heldItem);
		}

		private void UpdateItem(Item item)
		{
			//item.rigidbody.velocity = transform.position - item.transform.position;
			//item.rigidbody.angularVelocity = transform.rotation - item.transform.rotation;
		}

		public void OnPickUp(CallbackContext context)
		{
			if (!gameObject.activeInHierarchy)
				return;

			bool isPressed = context.performed;
			
			if (isPressed && !heldItem && collidingItems.Count > 0)
			{
				foreach (Item collidingItem in collidingItems)
				{
					print($"Attempting picking up {collidingItem}");
					if (collidingItem.canPickUp)
					{
						print($"Picking up {collidingItem}");

						FixedJoint joint = collidingItem.gameObject.AddComponent<FixedJoint>();
						joint.connectedBody = rigidbody;

						(heldItem = collidingItem).OnPickedUp(this);
						break;
					}
				}
			}
			else if (!isPressed && heldItem)
			{
				Item droppedItem = heldItem;
				heldItem = null;

				Destroy(droppedItem.GetComponent<FixedJoint>());
				droppedItem.rigidbody.WakeUp();

				droppedItem.OnDropped(this);

				if (OverlapPlacementHelper(out PlacementHelper placement, droppedItem.itemkind))
					placement.Adopt(droppedItem);
			}
		}

		private bool OverlapPlacementHelper(out PlacementHelper placement, Itemkind itemkind)
		{
			placement = null;

			foreach (PlacementHelper collidingPlacement in collidingPlacements)
				if (collidingPlacement.itemkind == itemkind && placement.isAdopting)
				{
					placement = collidingPlacement;
					return true;
				}

			return false;
		}

		private void OnTriggerEnter(Collider other)
		{
			Item hitItem = other.GetComponent<Item>();
			if (hitItem && !collidingItems.Contains(hitItem))
			{
				print($"Colliding with {hitItem}");
				collidingItems.Add(hitItem);
			}

			PlacementHelper hitPlacement = other.GetComponent<PlacementHelper>();
			if (hitPlacement && hitPlacement.gameObject.IsOnLayer(LayerMask.GetMask("Placement Helper")) && !collidingPlacements.Contains(hitPlacement))
				collidingPlacements.Add(hitPlacement);
		}

		private void OnTriggerExit(Collider other)
		{
			Item hitItem = other.GetComponent<Item>();
			if (hitItem)
				collidingItems.Remove(hitItem);

			PlacementHelper hitPlacement = other.GetComponent<PlacementHelper>();
			if (hitPlacement && hitPlacement.gameObject.IsOnLayer(LayerMask.GetMask("Placement Helper")))
				collidingPlacements.Remove(hitPlacement);
		}

		public void OnControlsChanged(PlayerInput input)
		{
			switch (input.currentControlScheme)
			{
				case "Keyboard":
					gameObject.SetActive(false);
					model.gameObject.SetActive(false);
					break;
				case "OpenXR":
					print($"Activating {input.currentControlScheme} input");
					gameObject.SetActive(true);
					model.gameObject.SetActive(true);
					model.position = transform.position;
					model.rotation = transform.rotation;
					break;
			}
		}
	}
}