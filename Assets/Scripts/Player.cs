using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
	public new Camera camera;
	public float sensitivity = 1000f;
	public float speed = 12f;
	public float jumpHeight = 3f;
	public float gravity = -9.81f;
	public Transform groundCheck;
	public float groundCheckDistance = 0.3f;
	public LayerMask groundCheckMask;
	public LayerMask raycastMask;

	public Item heldItem { get; private set; }
	public float holdDistance { get; set; }
	private CharacterController controller;
	private float camRot;
	private float yVelocity;
	private bool isGrounded;
	private Quaternion forceFlip = Quaternion.identity;

	private void Awake()
	{
		controller = GetComponent<CharacterController>();
		holdDistance = 2;
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		UpdateLook();
		UpdateMove();
		UpdateHeldItem();
	}

	private void UpdateLook()
	{
		Vector2 look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;

		transform.Rotate(Vector3.up * look.x);

		camRot = Mathf.Clamp(camRot - look.y, -90, 90);
		camera.transform.localRotation = Quaternion.Euler(camRot, 0, 0);
	}

	private void UpdateMove()
	{
		Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

		isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundCheckMask);
		if (isGrounded && yVelocity < 0)
			yVelocity = -2f; // Still go down a bit
		if (Input.GetButtonDown("Jump") && isGrounded)
			yVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
		yVelocity += gravity * Time.deltaTime;

		controller.Move(move * speed * Time.deltaTime + Vector3.up * yVelocity * Time.deltaTime);
	}

	private void UpdateHeldItem()
	{
		CheckPickUp();
		CheckDrop();
		CheckHold();
	}

	private void CheckPickUp()
	{
		if (heldItem)
			return;

		if (Input.GetMouseButtonDown(0) && Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, 10f, raycastMask) && hit.rigidbody)
		{
			Item hitItem = hit.rigidbody.GetComponent<Item>();
			if (hitItem && hitItem.canPickUp)
			{
				heldItem = hitItem;
				heldItem.OnPickedUp(this);
			}
		}
	}

	private void CheckHold()
	{
		if (!heldItem)
			return;
		
		heldItem.OnHeld(this);

		if (RaycastPlacementHelper(out PlacementHelper placement, heldItem.itemType))
		{
			UpdateItemSnapToPlacementHelper(placement);
		}
		else
		{
			UpdateItemMove();
			UpdateItemRotate();
		}
	}

	private void CheckDrop()
	{
		if (heldItem && Input.GetMouseButtonUp(0))
		{
			Item droppedItem = heldItem;
			DropItem();
			if (RaycastPlacementHelper(out PlacementHelper placement, droppedItem.itemType))
				placement.Adopt(droppedItem);
		}
	}

	public void DropItem()
	{
		heldItem.OnDropped(this);
		heldItem = null;
		forceFlip = Quaternion.identity;
	}

	private void UpdateItemMove()
	{
		Vector3 velocity = heldItem.rigidbody.velocity;
		heldItem.transform.position = Vector3.SmoothDamp(heldItem.transform.position, camera.transform.position + camera.transform.forward * holdDistance, ref velocity, 0.1f);
		heldItem.rigidbody.velocity = velocity;
	}

	private void UpdateItemRotate()
	{
		if (heldItem.GetComponent<CaptchalogueCard>())
		{
			Quaternion lookRot = Quaternion.LookRotation(camera.transform.position - heldItem.transform.position, camera.transform.up);
			Quaternion upRot = lookRot * Quaternion.Euler(0, 180, 0);
			Quaternion downRot = lookRot;

			if (Input.GetMouseButtonDown(2))
				if (forceFlip == Quaternion.identity)
					forceFlip = Quaternion.Angle(heldItem.transform.rotation, upRot) > 90 ? upRot : downRot;
				else
					forceFlip = forceFlip == downRot ? upRot : downRot;
			if (forceFlip != Quaternion.identity && Quaternion.Angle(heldItem.transform.rotation, forceFlip) < 90)
				forceFlip = Quaternion.identity;

			Quaternion deriv = QuaternionUtil.AngVelToDeriv(heldItem.transform.rotation, heldItem.rigidbody.angularVelocity);
			if (forceFlip == Quaternion.identity)
				heldItem.transform.rotation = QuaternionUtil.SmoothDamp(heldItem.transform.rotation, Quaternion.Angle(heldItem.transform.rotation, upRot) < 90 ? upRot : downRot, ref deriv, 0.1f);
			else
				heldItem.transform.rotation = QuaternionUtil.SmoothDamp(heldItem.transform.rotation, forceFlip, ref deriv, 0.2f);
			heldItem.rigidbody.angularVelocity = QuaternionUtil.DerivToAngVel(heldItem.transform.rotation, deriv);
		}
		else
		{
			Quaternion deriv = QuaternionUtil.AngVelToDeriv(heldItem.transform.rotation, heldItem.rigidbody.angularVelocity);
			heldItem.transform.rotation = QuaternionUtil.SmoothDamp(heldItem.transform.rotation, Quaternion.identity, ref deriv, 0.2f);
			heldItem.rigidbody.angularVelocity = QuaternionUtil.DerivToAngVel(heldItem.transform.rotation, deriv);
		}
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

	private bool RaycastPlacementHelper(out PlacementHelper placement, ItemType itemType)
	{
		placement = null;
		return Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit placementHit, 10f, LayerMask.GetMask("Placement Helper")) && (placement = placementHit.collider.GetComponent<PlacementHelper>()).itemType == itemType && placement.isAdopting;
	}
}
