using SBEPIS.Alchemy;
using SBEPIS.Captchalogue;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction
{
	public class ItemHolder : MonoBehaviour
	{
		public new Transform camera;
		public LayerMask raycastMask;
		public float maxDistance = 10f;

		public Item heldItem { get; private set; }
		private Quaternion cardForcedRotTarget = Quaternion.identity;
		private float holdDistance = 2;

		private void FixedUpdate()
		{
			if (!heldItem)
				return;

			heldItem.OnHeld(this);

			if (RaycastPlacementHelper(out PlacementHelper placement, heldItem.itemkind))
				UpdateItemSnapToPlacementHelper(placement);
			else
				UpdateItem(heldItem, true, 0.05f, 0.05f);
		}

		public void UpdateItem(Item item, bool physicsWillApply, float posTime, float rotTime)
		{
			Vector3 velocity = item.rigidbody.velocity;
			Vector3 newPos = Vector3.SmoothDamp(item.transform.position, camera.position + camera.forward * holdDistance, ref velocity, posTime);
			if (!physicsWillApply)
				item.transform.position = newPos;
			item.rigidbody.velocity = velocity;

			if (item.GetComponent<CaptchalogueCard>()) // Make these face either forward or backward to the player
			{
				Quaternion lookRot = Quaternion.LookRotation(camera.position - item.transform.position, camera.up);
				Quaternion upRot = lookRot * Quaternion.Euler(0, 180, 0); // Front facing player
				Quaternion downRot = lookRot; // Back facing player

				if (cardForcedRotTarget != Quaternion.identity && Quaternion.Angle(item.transform.rotation, cardForcedRotTarget) < 90)
					cardForcedRotTarget = Quaternion.identity;

				Quaternion deriv = QuaternionUtil.AngVelToDeriv(item.transform.rotation, item.rigidbody.angularVelocity);
				Quaternion newRot;
				if (cardForcedRotTarget == Quaternion.identity)
					newRot = QuaternionUtil.SmoothDamp(item.transform.rotation, Quaternion.Angle(item.transform.rotation, upRot) < 90 ? upRot : downRot, ref deriv, rotTime);
				else
					newRot = QuaternionUtil.SmoothDamp(item.transform.rotation, cardForcedRotTarget, ref deriv, rotTime);
				if (!physicsWillApply)
					item.transform.rotation = newRot;
				item.rigidbody.angularVelocity = QuaternionUtil.DerivToAngVel(item.transform.rotation, deriv);
			}
			else // Make these just go to 0
			{
				Quaternion deriv = QuaternionUtil.AngVelToDeriv(item.transform.rotation, item.rigidbody.angularVelocity);
				Quaternion newRot = QuaternionUtil.SmoothDamp(item.transform.rotation, Quaternion.identity, ref deriv, rotTime);
				if (!physicsWillApply)
					item.transform.rotation = newRot;
				item.rigidbody.angularVelocity = QuaternionUtil.DerivToAngVel(item.transform.rotation, deriv);
			}
		}

		public void OnFlipCard(CallbackContext context)
		{
			if (!context.performed || !heldItem)
				return;

			Quaternion lookRot = Quaternion.LookRotation(camera.position - heldItem.transform.position, camera.up);
			Quaternion upRot = lookRot * Quaternion.Euler(0, 180, 0); // Front facing player
			Quaternion downRot = lookRot; // Back facing player

			if (cardForcedRotTarget == Quaternion.identity)
				cardForcedRotTarget = Quaternion.Angle(heldItem.transform.rotation, upRot) > 90 ? upRot : downRot;
			else
				cardForcedRotTarget = cardForcedRotTarget == downRot ? upRot : downRot;
		}

		public void OnZoom(CallbackContext context)
		{
			if (context.ReadValue<float>() < 0)
				holdDistance = 0.7f;
			else if (context.ReadValue<float>() > 0)
				holdDistance = 2;
		}

		/// <summary>
		/// Handles both picking up items and pressing buttons
		/// </summary>
		public void OnPickUp(CallbackContext context)
		{
			bool isPressed = context.performed;
			if (isPressed && !heldItem && Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, maxDistance, LayerMask.GetMask("Button")))
				hit.collider.GetComponent<Button>().onPressed.Invoke(this);

			if (isPressed && !heldItem && Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, maxDistance, raycastMask) && hit.rigidbody)
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
			return Physics.Raycast(camera.position, camera.forward, out RaycastHit placementHit, maxDistance, LayerMask.GetMask("Placement Helper")) && (placement = placementHit.collider.GetComponent<PlacementHelper>()).itemkind == itemkind && placement.isAdopting;
		}
	}
}