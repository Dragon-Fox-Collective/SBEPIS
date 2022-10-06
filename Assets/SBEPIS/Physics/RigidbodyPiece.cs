using System;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Physics
{
	public class RigidbodyPiece : MonoBehaviour
	{
		public Collider referenceCollider;
		public float mass = 1;

		public Vector3 WorldCenter => transform.TransformPoint(referenceCollider switch
		{
			CapsuleCollider capsule => capsule.center,
			BoxCollider box => box.center,
			SphereCollider sphere => sphere.center,
			_ => throw new InvalidOperationException($"Reference collider {referenceCollider} is an invalid shape")
		});

		public Matrix4x4 LocalInertiaTensor {
			get
			{
				switch (referenceCollider)
				{
					case CapsuleCollider capsule:
						float longways = 0.5f * mass * capsule.radius * capsule.radius;
						float sideways = 0.08333333333f * mass * (3 * capsule.radius * capsule.radius + capsule.height * capsule.height);
						Matrix4x4 capsuleTensor = Matrix4x4.identity;
						capsuleTensor[0, 0] = capsule.direction == 0 ? longways : sideways;
						capsuleTensor[1, 1] = capsule.direction == 1 ? longways : sideways;
						capsuleTensor[2, 2] = capsule.direction == 2 ? longways : sideways;
						return capsuleTensor;

					case BoxCollider box:
						Matrix4x4 boxTensor = Matrix4x4.identity;
						boxTensor[0, 0] = 0.08333333333f * mass * (box.size.y * box.size.y + box.size.z * box.size.z);
						boxTensor[1, 1] = 0.08333333333f * mass * (box.size.z * box.size.z + box.size.x * box.size.x);
						boxTensor[2, 2] = 0.08333333333f * mass * (box.size.x * box.size.x + box.size.y * box.size.y);
						return boxTensor;

					case SphereCollider sphere:
						float inertia = 0.4f * mass * sphere.radius * sphere.radius;
						Matrix4x4 sphereTensor = Matrix4x4.identity;
						sphereTensor[0, 0] = inertia;
						sphereTensor[1, 1] = inertia;
						sphereTensor[2, 2] = inertia;
						return sphereTensor;

					default:
						throw new InvalidOperationException($"Reference collider {referenceCollider} is an invalid shape");
				}
			}
		}

		private void Awake()
		{
			if (referenceCollider.gameObject != gameObject)
				throw new InvalidOperationException($"RigidbodyPiece {this} should be on the same object as its reference collider");
		}
	}
}
