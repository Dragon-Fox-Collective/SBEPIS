using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Physics;
using SBEPIS.Utils.Linq;
using UnityEngine;

namespace SBEPIS.AI
{
	public class AIBrain : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private Navigator navigator;
		
		[SerializeField, Anywhere] private AIPointBehaviour moveTo;
		
		[SerializeField, Anywhere] private NavMesh navMesh;
		[SerializeField] private float waypointDistanceThreshold = 0.1f;
		[SerializeField] private float pathfindingClimbingAngle = 45f;
		
		private List<Vector3> waypoints;
		
		private void Start()
		{
			SetWaypointsAfterDelay().Forget();
		}
		
		private async UniTask SetWaypointsAfterDelay()
		{
			navigator.enabled = false;
			
			await UniTask.NextFrame();
			await UniTask.NextFrame();
			
			if (!navMesh.TryGetComponent(out GravitySum navMeshGravitySum))
				Debug.LogWarning($"NavMesh {navMesh} has no GravitySum");
			waypoints = navMesh.PathFromTo(transform.position, moveTo.transform.position, node => !navMeshGravitySum || Vector3.Angle(node.Normal, -navMeshGravitySum.GetGravityAt(node.Position)) <= pathfindingClimbingAngle)?.ToList();
			if (waypoints == null)
				Debug.LogWarning($"AI {this} could not pathfind on {navMesh} to {moveTo}");
			
			navigator.enabled = true;
		}
		
		private void FixedUpdate()
		{
			if (waypoints != null && waypoints.Any())
				FollowWaypoints();
			else
				navigator.Target = moveTo.transform.position;
		}
		
		private void FollowWaypoints()
		{
			Vector3 waypoint = waypoints.First();
			navigator.Target = waypoint;
			if (Vector3.Distance(transform.position, waypoint) < waypointDistanceThreshold)
				waypoints.Pop();
		}
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			if (waypoints != null)
				foreach ((Vector3 a, Vector3 b) in waypoints.Prepend(transform.position).Zip(waypoints.Append(moveTo.transform.position)))
					Gizmos.DrawLine(a, b);
			else
				Gizmos.DrawLine(transform.position, moveTo.transform.position);
		}
	}
}
