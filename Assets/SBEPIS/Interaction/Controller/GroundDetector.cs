using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction.Controller
{
	public class GroundDetector : MonoBehaviour
	{
		public Transform groundCheck;
		public float groundCheckDistance = 0.3f;
		public LayerMask groundCheckMask;

		private readonly Collider[] groundedColliders = new Collider[1];

		public bool isGrounded { get; private set; }
		public Collider groundCollider { get; private set; }
		public Rigidbody groundRigidbody => groundCollider ? groundCollider.attachedRigidbody : null;

		private void FixedUpdate()
		{
			isGrounded = UnityEngine.Physics.OverlapSphereNonAlloc(groundCheck.position, groundCheckDistance, groundedColliders, groundCheckMask) > 0;
			groundCollider = isGrounded ? groundedColliders[0] : null;
		}
	}
}
