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

	private CharacterController controller;
	private float camRot;
	private float yVelocity;
	private bool isGrounded;

	private void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		Vector2 look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;

		transform.Rotate(Vector3.up * look.x);

		camRot = Mathf.Clamp(camRot - look.y, -90, 90);
		camera.transform.localRotation = Quaternion.Euler(camRot, 0, 0);


		Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

		isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundCheckMask);
		if (isGrounded && yVelocity < 0)
			yVelocity = -2f; // Still go down a bit
		if (Input.GetButtonDown("Jump") && isGrounded)
			yVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
		yVelocity += gravity * Time.deltaTime;

		controller.Move(move * speed * Time.deltaTime + Vector3.up * yVelocity * Time.deltaTime);
	}
}
