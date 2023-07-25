using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Physics;
using SBEPIS.Utils;
using SBEPIS.Utils.Linq;
using UnityEngine;

namespace SBEPIS.AI
{
	public class AIBrain : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private Navigator navigator;
		
		[SerializeField, Anywhere] private AIPointBehaviour moveTo;
		
		[SerializeField] private float waypointDistanceThreshold = 0.1f;
		[SerializeField] private float pathfindingClimbingAngle = 45f;
		
		private List<Vector3> waypoints;
		
		private (Vector3, AIRaycastHit)[] rays = GenerateRays(0, 60, 5).Select(ray => (ray, default(AIRaycastHit))).ToArray();
		
		private void FixedUpdate()
		{
			for (int i = 0; i < rays.Length; i++)
			{
				UnityEngine.Physics.Raycast(transform.position, transform.TransformDirection(rays[i].Item1), out RaycastHit hit);
				rays[i] = (rays[i].Item1, new AIRaycastHit(hit));
			}
		}
		
		private static IEnumerable<Vector3> GenerateRays(float downAngle, float upAngle, int numRays)
		{
			for (float i = 0; i < numRays; i++)
				yield return Quaternion.Euler(i.Map(0, numRays - 1, downAngle, upAngle), 0, 0) * Vector3.forward;
		}
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			if (waypoints != null)
				foreach ((Vector3 a, Vector3 b) in waypoints.Prepend(transform.position).Zip(waypoints.Append(moveTo.transform.position)))
					Gizmos.DrawLine(a, b);
			else
				Gizmos.DrawLine(transform.position, moveTo.transform.position);
			
			foreach ((Vector3 ray, AIRaycastHit hit) in rays)
				if (hit.DidHit)
				{
					Gizmos.color = hit.Inclination < pathfindingClimbingAngle ? Color.green : Color.red;
					
					Gizmos.DrawRay(transform.position, transform.TransformDirection(ray) * hit.Distance);
					Gizmos.DrawWireSphere(hit.Point, 0.05f);
					Gizmos.color = Color.cyan;
					Gizmos.DrawRay(hit.Point, hit.Normal * 0.1f);
					Gizmos.color = Color.magenta;
					Gizmos.DrawRay(hit.Point, hit.Gravity * -0.01f); // Also shows gravity magnitude
				}
				else
				{
					Gizmos.color = Color.yellow;
					Gizmos.DrawRay(transform.position, transform.TransformDirection(ray) * 0.3f);
				}
		}
		
		private struct AIRaycastHit
		{
			public bool DidHit { get; }
			
			public Vector3 Point { get; }
			public Vector3 Normal { get; }
			public float Distance { get; }
			
			private bool calculatedGravity;
			private Vector3 gravity;
			public Vector3 Gravity
			{
				get
				{
					if (!calculatedGravity)
					{
						gravity = GravitySum.GetGravityAtOnDefaultLayer(Point);
						calculatedGravity = true;
					}
					return gravity;
				}
			}
			
			public float Inclination => Vector3.Angle(Gravity * -1, Normal);
			
			public AIRaycastHit(RaycastHit hit)
			{
				DidHit = hit.collider;
				Point = hit.point;
				Normal = hit.normal;
				Distance = hit.distance;
				
				calculatedGravity = false;
				gravity = Vector3.zero;
			}
		}
	}
}
