using System;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction
{
	public class RigidbodyPiece : MonoBehaviour
	{
		public Collider referenceCollider;
		public float mass;

		public Vector3 LocalInertiaTensor {
			get
			{
				switch (referenceCollider)
				{
					case CapsuleCollider capsule:
						float longways = 0.5f * mass * capsule.radius * capsule.radius;
						float sideways = 0.08333333333f * mass * (3 * capsule.radius * capsule.radius + capsule.height * capsule.height);
						return capsule.direction switch
						{
							0 => new Vector3(longways, sideways, sideways),
							1 => new Vector3(sideways, longways, sideways),
							2 => new Vector3(sideways, sideways, longways),
							_ => throw new ArgumentOutOfRangeException("capsule collider direction no?????")
						};

					case BoxCollider box:
					case SphereCollider sphere:
					default:
						throw new InvalidOperationException("Reference collider is an invalid shape");
				}
			}
		}

		private void Awake()
		{
			if (referenceCollider.gameObject != gameObject)
				throw new InvalidOperationException("RigidbodyPiece should be on the same object as its reference collider");
		}
	}
}
