using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using SBEPIS.Physics;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.AI
{
	public class AIBrain : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private Navigator navigator;
		
		[SerializeField, Self] private GravitySum gravitySum;
		
		[SerializeField, Anywhere] private AIPointBehaviour moveTo;
		private Vector3 FinalTarget => moveTo.transform.position;
		
		[SerializeField] private float waypointDistanceThreshold = 0.1f;
		[SerializeField] private float climbingAngle = 45f;
		[SerializeField] private float stepUpHeight = 0.5f;
		
		[SerializeField] private int spinCycleIterations = 8;
		
		private (Vector3, AIRaycastHit)[][] rays = GenerateRays(60, 5, 10, 3, 4).Select(side => side.Select(ray => (ray, default(AIRaycastHit))).ToArray()).ToArray();
		private (Vector3, AIRaycastHit)[] Front => rays[0];
		
		private bool isCheckingForPotentialTargets;
		private bool hasTarget;
		private Vector3 target;
		
		private void Update()
		{
			DecideWhatToDo();
			//CastAllRays();
			//transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(target - transform.position, transform.up), transform.up);
		}
		
		private void CastAllRays()
		{
			foreach ((Vector3, AIRaycastHit)[] side in rays)
				CastRays(side, transform.rotation);
		}
		private void CastFrontRays(Quaternion rotation)
		{
			CastRays(Front, rotation);
		}
		private void CastRays((Vector3, AIRaycastHit)[] side, Quaternion rotation)
		{
			for (int j = 0; j < side.Length; j++)
			{
				Vector3 ray = side[j].Item1;
				UnityEngine.Physics.Raycast(transform.position, rotation * ray, out RaycastHit hit);
				side[j] = (ray, new AIRaycastHit(hit));
			}
		}
		
		private void DecideWhatToDo()
		{
			if (hasTarget && Vector3.Distance(transform.position, target) < waypointDistanceThreshold)
				hasTarget = false;
			
			if (!isCheckingForPotentialTargets && !hasTarget)
				ValidateTarget();
			if (isCheckingForPotentialTargets)
				DoSpinCycle();
			
			navigator.Target = target;
			navigator.enabled = hasTarget;
		}
		
		private void ValidateTarget()
		{
			isCheckingForPotentialTargets = false;
			hasTarget = false;
			navigator.enabled = false;
			
			Vector3 targetDelta = FinalTarget - transform.position;
			
			if (!UnityEngine.Physics.Raycast(transform.position, targetDelta, targetDelta.magnitude))
			{
				isCheckingForPotentialTargets = true;
				return;
			}
			
			Quaternion targetRotation = Quaternion.LookRotation(targetDelta, gravitySum.UpDirection);
			CastFrontRays(targetRotation);
			float targetAngle = Vector3.Angle(-gravitySum.UpDirection, targetDelta);
			
			for (int i = 0; i < Front.Length; i++)
			{
				(Vector3 ray, AIRaycastHit hit) = Front[i];
				AIRaycastHit prevHit = i > 0 ? Front[i - 1].Item2 : default;
				
				if (!hit.IsWalkable(prevHit, climbingAngle, stepUpHeight))
				{
					isCheckingForPotentialTargets = true;
					return;
				}

				if (targetAngle < Vector3.Angle(-gravitySum.UpDirection, targetRotation * ray))
					break;
			}
			
			hasTarget = true;
			target = FinalTarget;
		}
		
		private void DoSpinCycle()
		{
			float randomOffsetAngle = Random.Range(0f, 360f);
			float targetHeuristic = float.NegativeInfinity;
			for (int iteration = 0; iteration < spinCycleIterations; iteration++)
			{
				CastFrontRays(Quaternion.AngleAxis(randomOffsetAngle + 360f * iteration / spinCycleIterations, gravitySum.UpDirection));
				(Vector3 iterationTarget, float iterationHeuristic) = GetSpinCycleHeuristic();
				if (iterationHeuristic > targetHeuristic)
				{
					targetHeuristic = iterationHeuristic;
					target = iterationTarget;
				}
			}
			hasTarget = true;
			isCheckingForPotentialTargets = false;
		}
		
		private (Vector3, float) GetSpinCycleHeuristic()
		{
			AIRaycastHit heuristicHit = default;
			for (int i = 0; i < Front.Length; i++)
			{
				AIRaycastHit hit = Front[i].Item2;
				heuristicHit = i > 0 ? Front[i - 1].Item2 : default;
				if (!hit.IsWalkable(heuristicHit, climbingAngle, stepUpHeight))
					break;
			}
			
			if (!heuristicHit.DidHit)
				return (Vector3.zero, float.NegativeInfinity);
			
			Vector3 finalTargetDelta = FinalTarget - heuristicHit.Point;
			
			return (heuristicHit.Point,
				- Vector3.Distance(transform.position, heuristicHit.Point)
				- Vector3.Distance(FinalTarget, heuristicHit.Point)
				+ (UnityEngine.Physics.Raycast(heuristicHit.Point, finalTargetDelta, finalTargetDelta.magnitude) ? 0 : 10)
				);
		}
		
		private static IEnumerable<IEnumerable<Vector3>> GenerateRays(float downAngle, float upAngle, int numRaysForward, int numRaysSide, int numSides)
		{
			for (float side = 0; side < numSides + 1; side++)
			{
				float sideAngle = side.Map(0, numSides + 1, 0, 360);
				yield return GenerateSideRays(downAngle, upAngle, side == 0 ? numRaysForward : numRaysSide).Select(ray => Quaternion.Euler(0, sideAngle, 0) * ray);
			}
		}
		private static IEnumerable<Vector3> GenerateSideRays(float downAngle, float upAngle, int numRays)
		{
			for (float i = 0; i < numRays; i++)
				yield return Quaternion.Euler(i.Map(0, numRays - 1, downAngle, upAngle), 0, 0) * Vector3.forward;
		}
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			if (hasTarget)
			{
				Gizmos.DrawLine(transform.position, target);
				Gizmos.DrawLine(target, FinalTarget);
			}
			else
			{
				Gizmos.DrawLine(transform.position, FinalTarget);
			}
			
			foreach ((Vector3, AIRaycastHit)[] side in rays)
			{
				bool isCurrentlyAlongValidPath = true;
				for (int i = 0; i < side.Length; i++)
				{
					(Vector3 ray, AIRaycastHit hit) = side[i];
					bool hasPrevHit = i > 0;
					AIRaycastHit prevHit = hasPrevHit ? side[i - 1].Item2 : default;
					if (hit.DidHit)
					{
						Gizmos.color = Color.cyan;
						Gizmos.DrawRay(hit.Point, hit.Normal * 0.1f);
						Gizmos.color = Color.magenta;
						Gizmos.DrawRay(hit.Point, hit.Gravity * -0.01f); // Also shows gravity magnitude
						
						Gizmos.color = hit.IsFreelyWalkable(climbingAngle) ? Color.green
							: hit.IsBump(prevHit, climbingAngle, stepUpHeight) ? Color.green + Color.white * 0.5f
							: Color.red;
						
						Gizmos.DrawWireSphere(hit.Point, 0.05f);
						
						if (isCurrentlyAlongValidPath && !hit.IsWalkable(prevHit, climbingAngle, stepUpHeight))
							isCurrentlyAlongValidPath = false;
						
						if (isCurrentlyAlongValidPath)
							Gizmos.DrawLine(hasPrevHit ? prevHit.Point : transform.position, hit.Point);
					}
					else
					{
						Gizmos.color = Color.yellow;
					}
					
					Gizmos.DrawRay(transform.position, transform.TransformDirection(ray) * 0.3f);
				}
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
			public Vector3 Up => Gravity * -1;
			
			public float Inclination => Vector3.Angle(Up, Normal);
			
			public bool IsFreelyWalkable(float climbingAngle) => Inclination < climbingAngle;
			public bool IsBump(AIRaycastHit prevHit, float climbingAngle, float stepUpHeight) => prevHit.DidHit
			                                                                                     && Vector3.Project(Point - prevHit.Point, Gravity).magnitude <= stepUpHeight
			                                                                                     && 90 - Vector3.Angle(Up, Point - prevHit.Point) < climbingAngle;
			public bool IsWalkable(AIRaycastHit prevHit, float climbingAngle, float stepUpHeight) => IsFreelyWalkable(climbingAngle) || IsBump(prevHit, climbingAngle, stepUpHeight);
			
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
