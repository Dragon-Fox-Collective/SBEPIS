using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using SBEPIS.Utils.Linq;
using UnityEngine;

namespace SBEPIS.AI
{
	public class AIBrain : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private AIPointBehaviour moveTo;
		
		[SerializeField, Anywhere] private NavMesh navMesh;
		[SerializeField] private float speed = 1;
		[SerializeField] private float waypointDistanceThreshold = 0.1f;
		
		private List<Vector3> waypoints = new();
		
		private void Start()
		{
			waypoints = navMesh.PathFromTo(transform.position, moveTo.transform.position).ToList();
		}
		
		private void FixedUpdate()
		{
			if (waypoints.Any())
				FollowWaypoints();
		}
		
		private void FollowWaypoints()
		{
			Vector3 waypoint = waypoints.First();
			transform.position += speed * Time.fixedDeltaTime * (waypoint - transform.position).normalized;
			if (Vector3.Distance(transform.position, waypoint) < waypointDistanceThreshold)
				waypoints.Pop();
		}
	}
}
