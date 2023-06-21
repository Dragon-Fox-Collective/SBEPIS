using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.AI
{
	public class AIPointBehaviour : MonoBehaviour
	{
		private AIPoint point;
		private AIPointState state;
		
		[SerializeField] private List<AIPointBehaviour> connectedPoints = new();
		
		private void Awake()
		{
			state = new AIPointState();
			point = new AIPoint(name, state);
		}
		
		private void Start()
		{
			foreach (AIPointBehaviour connectedPoint in connectedPoints.Where(connectedPoint => connectedPoint))
			{
				//point.Connect(connectedPoint);
			}
		}
		
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, 0.1f);
			
			Gizmos.color = Color.red;
			foreach (AIPointBehaviour connectedPoint in connectedPoints.Where(connectedPoint => connectedPoint))
				DrawArrow(transform.position, connectedPoint.transform.position);
		}

		private static void DrawArrow(Vector3 position1, Vector3 position2)
		{
			Gizmos.DrawLine(position1, position2);
			
			const float arrowWidth = 0.1f;
			
			Vector3 forward = (position2 - position1).normalized;
			Vector3 right = Vector3.Cross(forward, Vector3.up);
			Vector3 up = Vector3.Cross(forward, right);
			
			forward *= arrowWidth;
			right *= arrowWidth;
			up *= arrowWidth;
			
			Vector3 midpoint = Vector3.Lerp(position1, position2, 0.5f) + forward;
			
			Gizmos.DrawLine(midpoint + up + right, midpoint + up - right);
			Gizmos.DrawLine(midpoint + up - right, midpoint - up - right);
			Gizmos.DrawLine(midpoint - up - right, midpoint - up + right);
			Gizmos.DrawLine(midpoint - up + right, midpoint + up + right);
			
			Gizmos.DrawLine(midpoint + up + right, midpoint + forward);
			Gizmos.DrawLine(midpoint + up - right, midpoint + forward);
			Gizmos.DrawLine(midpoint - up - right, midpoint + forward);
			Gizmos.DrawLine(midpoint - up + right, midpoint + forward);
		}
	}
}
