using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using SBEPIS.Utils.VectorLinq;
using UnityEngine.Events;

namespace SBEPIS.Physics
{
	public class GravitySum : ValidatedMonoBehaviour
	{
		[SerializeField, Self(Flag.Optional)] private new Rigidbody rigidbody;
		
		[SerializeField] private Transform customCenterOfMass;
		public UnityEvent<Vector3> onGravityChanged = new();
		
		public Vector3 UpDirection { get; private set; } = Vector3.up;
		public float GravityAcceleration { get; private set; } = 0;
		public Vector3 WorldCenterOfMass { get; private set; }
		
		private readonly List<MassiveBody> massiveBodies = new();
		
		private void FixedUpdate()
		{
			UpdateGravity();
			ApplyGravity();
		}
		
		private static Collider[] defaultLayerColliders = new Collider[10];
		public static Vector3 GetGravityAtOnDefaultLayer(Vector3 point, float overlapRadius = 0.1f)
		{
			int numColliders = UnityEngine.Physics.OverlapSphereNonAlloc(point, overlapRadius, defaultLayerColliders, LayerMask.GetMask("Gravity"), QueryTriggerInteraction.Collide);
			return GetGravityAt(point, defaultLayerColliders.Take(numColliders).Select(collider => collider.GetComponent<MassiveBody>()));
		}
		
		public static Vector3 GetGravityAt(Vector3 point, IEnumerable<MassiveBody> massiveBodies) => massiveBodies
			.Distinct()
			.Where(body => body)
			.GroupBy(body => body.priority)
			.OrderBy(group => group.Key)
			.Aggregate(Vector3.zero, (lowerProrityGravity, group) =>
			{
				List<Vector3> localCentersOfMass = group.Select(body => body.transform.InverseTransformPoint(point)).ToList();
				List<float> priorities = group.Zip(localCentersOfMass, (body, localCenterOfMass) => body.GetPriority(localCenterOfMass).Aggregate(1, (product, x) => product * x)).ToList();
				return Vector3.Lerp(lowerProrityGravity,
					group.Zip(localCentersOfMass, (body, localCenterOfMass) => body.transform.TransformDirection(body.GetGravity(localCenterOfMass)))
						.Zip(priorities, (gravity, priority) => Vector3.LerpUnclamped(Vector3.zero, gravity, priority)).Sum(),
					priorities.Sum());
			});
		
		public Vector3 GetGravityAt(Vector3 point) => massiveBodies.Count == 0 ? Vector3.zero : GetGravityAt(point, massiveBodies);
		
		private void UpdateGravity()
		{
			WorldCenterOfMass = customCenterOfMass ? customCenterOfMass.position : rigidbody ? rigidbody.worldCenterOfMass : transform.position;
			Vector3 gravity = GetGravityAt(WorldCenterOfMass);
			GravityAcceleration = gravity.magnitude;
			if (GravityAcceleration > 0)
			{
				UpDirection = -gravity.normalized;
				onGravityChanged.Invoke(UpDirection);
			}
		}
		
		private void ApplyGravity()
		{
			if (rigidbody && !rigidbody.isKinematic)
				rigidbody.velocity += -GravityAcceleration * Time.fixedDeltaTime * UpDirection;
		}
		
		public void Accumulate(MassiveBody body)
		{
			massiveBodies.Add(body);
		}
		
		public void Deaccumulate(MassiveBody body)
		{
			massiveBodies.Remove(body);
		}
	}
}
