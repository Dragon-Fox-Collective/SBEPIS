using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WrightWay.SBEPIS.Util;

namespace WrightWay.SBEPIS.Player
{
	public class ItemHolder : MonoBehaviour
	{
		public new Transform camera;
		public LayerMask raycastMask;

		public Item heldItem { get; private set; }
		private Quaternion cardForcedRotTarget = Quaternion.identity;
		private float holdDistance = 2;

		private void Update()
		{
			if (!heldItem)
				return;

			heldItem.OnHeld(this);

			if (RaycastPlacementHelper(out PlacementHelper placement, heldItem.itemkind))
				UpdateItemSnapToPlacementHelper(placement);
			else
				UpdateItem(heldItem);
		}

		public void UpdateItem(Item item)
		{
			Vector3 velocity = item.rigidbody.velocity;
			item.transform.position = Vector3.SmoothDamp(item.transform.position, camera.position + camera.forward * holdDistance, ref velocity, 0.1f);
			item.rigidbody.velocity = velocity;

			if (item.GetComponent<CaptchalogueCard>()) // Make these face either forward or backward to the player
			{
				Quaternion lookRot = Quaternion.LookRotation(camera.position - item.transform.position, camera.up);
				Quaternion upRot = lookRot * Quaternion.Euler(0, 180, 0); // Front facing player
				Quaternion downRot = lookRot; // Back facing player

				if (cardForcedRotTarget != Quaternion.identity && Quaternion.Angle(item.transform.rotation, cardForcedRotTarget) < 90)
					cardForcedRotTarget = Quaternion.identity;

				Quaternion deriv = QuaternionUtil.AngVelToDeriv(item.transform.rotation, item.rigidbody.angularVelocity);
				if (cardForcedRotTarget == Quaternion.identity)
					item.transform.rotation = QuaternionUtil.SmoothDamp(item.transform.rotation, Quaternion.Angle(item.transform.rotation, upRot) < 90 ? upRot : downRot, ref deriv, 0.1f);
				else
					item.transform.rotation = QuaternionUtil.SmoothDamp(item.transform.rotation, cardForcedRotTarget, ref deriv, 0.2f);
				item.rigidbody.angularVelocity = QuaternionUtil.DerivToAngVel(item.transform.rotation, deriv);
			}
			else // Make these just go to 0
			{
				Quaternion deriv = QuaternionUtil.AngVelToDeriv(item.transform.rotation, item.rigidbody.angularVelocity);
				item.transform.rotation = QuaternionUtil.SmoothDamp(item.transform.rotation, Quaternion.identity, ref deriv, 0.2f);
				item.rigidbody.angularVelocity = QuaternionUtil.DerivToAngVel(item.transform.rotation, deriv);
			}
		}

		private void OnFlipCard()
		{
			if (!heldItem)
				return;

			Quaternion lookRot = Quaternion.LookRotation(camera.position - heldItem.transform.position, camera.up);
			Quaternion upRot = lookRot * Quaternion.Euler(0, 180, 0); // Front facing player
			Quaternion downRot = lookRot; // Back facing player

			if (cardForcedRotTarget == Quaternion.identity)
				cardForcedRotTarget = Quaternion.Angle(heldItem.transform.rotation, upRot) > 90 ? upRot : downRot;
			else
				cardForcedRotTarget = cardForcedRotTarget == downRot ? upRot : downRot;
		}

		private void OnZoom(InputValue value)
		{
			if (value.Get<float>() < 0)
				holdDistance = 0.7f;
			else if (value.Get<float>() > 0)
				holdDistance = 2;
		}

		/// <summary>
		/// Handles both picking up items and pressing buttons
		/// </summary>
		private void OnUse(InputValue value)
		{
			if (value.isPressed && !heldItem && Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, 10f, raycastMask))
			{
				if (hit.rigidbody)
				{
					Item hitItem = hit.rigidbody.GetComponent<Item>();
					if (hitItem && hitItem.canPickUp)
						(heldItem = hitItem).OnPickedUp(this);
				}
				else
				{
					Button button = hit.collider.GetComponent<Button>();
					if (button)
						button.onPressed.Invoke(this);
				}
			}
			else if (!value.isPressed && heldItem)
			{
				Item droppedItem = DropItem();
				if (RaycastPlacementHelper(out PlacementHelper placement, droppedItem.itemkind))
					placement.Adopt(droppedItem);
			}
		}

		public Item DropItem()
		{
			Item droppedItem = heldItem;
			heldItem = null;
			cardForcedRotTarget = Quaternion.identity;
			droppedItem.OnDropped(this);
			return droppedItem;
		}

		private void UpdateItemSnapToPlacementHelper(PlacementHelper placement)
		{
			Vector3 velocity = heldItem.rigidbody.velocity;
			heldItem.transform.position = Vector3.SmoothDamp(heldItem.transform.position, placement.itemParent.position, ref velocity, 0.3f);
			heldItem.rigidbody.velocity = velocity;

			Quaternion deriv = QuaternionUtil.AngVelToDeriv(heldItem.transform.rotation, heldItem.rigidbody.angularVelocity);
			heldItem.transform.rotation = QuaternionUtil.SmoothDamp(heldItem.transform.rotation, placement.itemParent.rotation, ref deriv, 0.2f);
			heldItem.rigidbody.angularVelocity = QuaternionUtil.DerivToAngVel(heldItem.transform.rotation, deriv);
		}

		private bool RaycastPlacementHelper(out PlacementHelper placement, Itemkind itemkind)
		{
			placement = null;
			return Physics.Raycast(camera.position, camera.forward, out RaycastHit placementHit, 10f, LayerMask.GetMask("Placement Helper")) && (placement = placementHit.collider.GetComponent<PlacementHelper>()).itemkind == itemkind && placement.isAdopting;
		}
	}
}