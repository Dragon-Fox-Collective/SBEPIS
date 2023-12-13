using System;
using UnityEngine;

namespace SBEPIS.Physics
{
	public class RigidbodyPiece : MonoBehaviour
	{
		[SerializeField]
		private Collider referenceCollider;
		[SerializeField]
		private float mass = 1;
		public float Mass => mass;

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
				Vector3 lossyScale = referenceCollider.transform.lossyScale;
				if (!Mathf.Approximately(lossyScale.x, lossyScale.y) || !Mathf.Approximately(lossyScale.x, lossyScale.z))
					Debug.LogWarning($"Rigidbody piece {name} (part of {referenceCollider.GetComponentInParent<CompoundRigidbody>().name}) has an uneven lossy scale {lossyScale}. Avoid this if possible.");
				float scale = lossyScale.x;
				
				switch (referenceCollider)
				{
					case CapsuleCollider capsule:
					{
						float radius = capsule.radius * scale;
						float height = capsule.height * scale;
						float longways = 0.5f * mass * radius * radius;
						float sideways = 0.08333333333f * mass * (3 * radius * radius + height * height);
						Matrix4x4 capsuleTensor = Matrix4x4.identity;
						capsuleTensor[0, 0] = capsule.direction == 0 ? longways : sideways;
						capsuleTensor[1, 1] = capsule.direction == 1 ? longways : sideways;
						capsuleTensor[2, 2] = capsule.direction == 2 ? longways : sideways;
						return capsuleTensor;
					}

					case BoxCollider box:
					{
						float width = box.size.x * scale;
						float height = box.size.y * scale;
						float depth = box.size.z * scale;
						Matrix4x4 boxTensor = Matrix4x4.identity;
						boxTensor[0, 0] = 0.08333333333f * mass * (height * height + depth * depth);
						boxTensor[1, 1] = 0.08333333333f * mass * (depth * depth + width * width);
						boxTensor[2, 2] = 0.08333333333f * mass * (width * width + height * height);
						return boxTensor;
					}

					case SphereCollider sphere:
					{
						float radius = sphere.radius * scale;
						float inertia = 0.4f * mass * radius * radius;
						Matrix4x4 sphereTensor = Matrix4x4.identity;
						sphereTensor[0, 0] = inertia;
						sphereTensor[1, 1] = inertia;
						sphereTensor[2, 2] = inertia;
						return sphereTensor;
					}

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
