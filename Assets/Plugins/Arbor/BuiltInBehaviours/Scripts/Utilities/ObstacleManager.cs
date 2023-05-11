//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Arbor
{
	public static class ObstacleManager
	{
		private static NavMeshPath s_NavMeshPath = new NavMeshPath();

		sealed class ObstacleObject
		{
			private NavMeshObstacle _Obstacle;
			private NavMeshAgent _Agent;

			public Transform transform
			{
				get;
				private set;
			}

			public NavMeshObstacle obstacle
			{
				get
				{
					return _Obstacle;
				}
				set
				{
					_Obstacle = value;
					transform = _Obstacle?.transform;
					_Agent = null;
				}
			}

			public NavMeshAgent agent
			{
				get
				{
					return _Agent;
				}
				set
				{
					_Agent = value;
					transform = _Agent?.transform;
					_Obstacle = null;
				}
			}

			public float distanceSqrToAgent;

			public bool TryGetHidingPosition(Vector3 targetPos, float agentRadius, out Vector3 hidingPosition)
			{
				if (_Obstacle != null)
				{
					return TryGetHidingPositionObstacle(targetPos, agentRadius, out hidingPosition);
				}
				else if (_Agent != null)
				{
					return TryGetHidingPositionAgent(targetPos, agentRadius, out hidingPosition);
				}

				hidingPosition = default;
				return false;
			}

			bool TryGetHidingPositionObstacle(Vector3 targetPos, float agentRadius, out Vector3 hidingPosition)
			{
				Vector3 center = _Obstacle.center;
				Transform obstacleTransform = transform;
				targetPos = obstacleTransform.InverseTransformPoint(targetPos) - center;

				Vector3 closestPoint = Vector3.zero;

				switch (_Obstacle.shape)
				{
					case NavMeshObstacleShape.Capsule:
						{
							float halfHeight = _Obstacle.height;
							Vector3 p = new Vector3(0f, -halfHeight, 0.0f);
							Vector3 q = new Vector3(0f, halfHeight, 0.0f);

							if (!IntersectSegmentCapsule(p, q, _Obstacle.radius, targetPos, Vector3.zero, out closestPoint))
							{
								hidingPosition = default;
								return false;
							}
							closestPoint = -closestPoint;
						}
						break;
					case NavMeshObstacleShape.Box:
						{
							if (!IntersectRayBox(_Obstacle.size, targetPos, -targetPos.normalized, out closestPoint))
							{
								hidingPosition = default;
								return false;
							}
							closestPoint = -closestPoint;
						}
						break;
				}

				Vector3 dir = obstacleTransform.TransformVector(closestPoint).normalized;
				hidingPosition = obstacleTransform.TransformPoint(closestPoint + center) + dir * agentRadius;
				return true;
			}

			bool TryGetHidingPositionAgent(Vector3 targetPos, float agentRadius, out Vector3 hidingPosition)
			{
				Transform agentTransform = transform;
				float halfHeight = _Agent.height * 0.5f;
				Vector3 center = new Vector3(0f, halfHeight, 0f);
				targetPos = agentTransform.InverseTransformPoint(targetPos) - center;

				Vector3 p = new Vector3(0f, -halfHeight, 0.0f);
				Vector3 q = new Vector3(0f, halfHeight, 0.0f);

				Vector3 closestPoint;
				if (!IntersectSegmentCapsule(p, q, _Agent.radius, targetPos, Vector3.zero, out closestPoint))
				{
					hidingPosition = default;
					return false;
				}
				closestPoint = -closestPoint;
				Vector3 dir = agentTransform.TransformVector(closestPoint).normalized;
				hidingPosition = agentTransform.TransformPoint(closestPoint + center) + dir * agentRadius;
				return true;
			}
		}

		class ObstacleClosestOrder : IComparer<ObstacleObject>
		{
			public int Compare(ObstacleObject a, ObstacleObject b)
			{
				return a.distanceSqrToAgent.CompareTo(b.distanceSqrToAgent);
			}
		}

		static readonly ObstacleClosestOrder s_ClosestOrder = new ObstacleClosestOrder();

		public static bool TryGetHidingPosition(AgentController agentController, Vector3 targetPos, float minDistanceToTarget, ObstacleTargetFlags obstacleTargetFlags, ObstacleSearchFlags obstacleSearchFlags, int obstacleLayers, out Vector3 hidingPos)
		{
			float minDistance = float.MaxValue;
			bool find = false;
			hidingPos = Vector3.zero;

			Transform agentTransform = agentController.agentTransform;
			NavMeshAgent agentSelf = agentController.agent;
			float agentRadius = agentSelf.radius;
			Vector3 agentPos = agentTransform.position;
			GameObject self = agentTransform.gameObject;

			Vector3 targetToAgent = agentPos - targetPos;

			float minDistanceSqrToTarget = minDistanceToTarget * minDistanceToTarget;

			using (Pool.ListPool<ObstacleObject>.Get(out var obstacleObjects))
			{
				if ((obstacleTargetFlags & ObstacleTargetFlags.NavMeshObstacle) == ObstacleTargetFlags.NavMeshObstacle)
				{
					using (Pool.ListPool<NavMeshObstacle>.Get(out var obstacles))
					{
						FindComponentsOfType<NavMeshObstacle>(obstacles);
						for (int i = 0, count = obstacles.Count; i < count; i++)
						{
							var obstacle = obstacles[i];
							var obstacleGameObject = obstacle.gameObject;
							if (obstacleGameObject == self)
							{
								continue;
							}

							if (!HasLayerFlags(obstacleLayers, obstacleGameObject.layer))
							{
								continue;
							}

							var obstacleObject = Pool.GenericPool<ObstacleObject>.Get();
							obstacleObject.obstacle = obstacle;
							obstacleObject.distanceSqrToAgent = (obstacleObject.transform.position - agentPos).sqrMagnitude;

							obstacleObjects.Add(obstacleObject);
						}
					}
				}

				if ((obstacleTargetFlags & ObstacleTargetFlags.NavMeshAgent) == ObstacleTargetFlags.NavMeshAgent)
				{
					using (Pool.ListPool<NavMeshAgent>.Get(out var agents))
					{
						FindComponentsOfType<NavMeshAgent>(agents);
						for (int i = 0, count = agents.Count; i < count; i++)
						{
							var agent = agents[i];
							var agentGameObject = agent.gameObject;
							if (agentGameObject == self)
							{
								continue;
							}

							if (!HasLayerFlags(obstacleLayers, agentGameObject.layer))
							{
								continue;
							}

							var obstacleObject = Pool.GenericPool<ObstacleObject>.Get();
							obstacleObject.agent = agent;
							obstacleObject.distanceSqrToAgent = (obstacleObject.transform.position - agentPos).sqrMagnitude;

							obstacleObjects.Add(obstacleObject);
						}
					}
				}

				obstacleObjects.Sort(s_ClosestOrder);

				for (int i = 0; i < obstacleObjects.Count; i++)
				{
					var obstacleObject = obstacleObjects[i];

					try
					{
						if (obstacleObject.TryGetHidingPosition(targetPos, agentRadius, out var pos))
						{
							float distanceSqrToTarget = (pos - targetPos).sqrMagnitude;
							if (distanceSqrToTarget < minDistanceSqrToTarget)
							{
								continue;
							}

							if ((obstacleSearchFlags & ObstacleSearchFlags.Path) != 0)
							{
								if (NavMesh.CalculatePath(agentPos, pos, agentSelf.areaMask, s_NavMeshPath))
								{
									float distance = GetPathCost(agentPos, targetPos, s_NavMeshPath);
									if (distance < minDistance)
									{
										minDistance = distance;
										hidingPos = pos;
										find = true;
									}
								}
							}
							else
							{
								float distance = Vector3.Distance(pos, agentPos);
								if (distance < minDistance)
								{
									minDistance = distance;
									hidingPos = pos;
									find = true;
								}
							}

							if ((obstacleSearchFlags & ObstacleSearchFlags.ScanAll) == 0 && find)
							{
								break;
							}
						}
					}
					finally
					{
						Pool.GenericPool<ObstacleObject>.Release(obstacleObject);
					}
				}
			}

			return find;
		}

		static float GetPathCost(Vector3 agentPos, Vector3 targetPos, NavMeshPath path)
		{
			Vector3 agentToTarget = targetPos - agentPos;
			float atd = Vector3.Dot(agentToTarget, agentToTarget);

			Vector3 agentToTargetDir = agentToTarget.normalized;
			Vector3 currentPos = agentPos;
			float totalCost = 0f;
			var corners = path.corners;
			for (int i = 0; i < corners.Length; i++)
			{
				var corner = corners[i];

				Vector3 cornerVec = corner - currentPos;
				float d = Vector3.Dot(cornerVec.normalized, agentToTargetDir); // Correct the direction of cornerVec between -1 and 1
				float cost = (d + 1f) / 2f + 1f; // Correct the direction of cornerVec between 1 and 2

				totalCost += cornerVec.magnitude * cost;
				currentPos = corner;
			}

			return totalCost;
		}

		static void FindComponentsOfType<T>(List<T> result)
		{
			for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
			{
				Scene scene = SceneManager.GetSceneAt(sceneIndex);
				if (!scene.IsValid())
				{
					continue;
				}

				using (Pool.ListPool<GameObject>.Get(out var gameObjects))
				{
					scene.GetRootGameObjects(gameObjects);

					for (int gameObjectIndex = 0; gameObjectIndex < gameObjects.Count; gameObjectIndex++)
					{
						GameObject gameObject = gameObjects[gameObjectIndex];
						if (gameObject.activeInHierarchy)
						{
							using (Pool.ListPool<T>.Get(out var components))
							{
								gameObject.GetComponentsInChildren(components);
								result.AddRange(components);
							}
						}
					}
				}
			}
		}

		static bool HasLayerFlags(int layerMask, int layer)
		{
			return ((1 << layer) & layerMask) != 0;
		}

		static bool IntersectSegmentCapsule(Vector3 p, Vector3 q, float radius, Vector3 sa, Vector3 sb, out Vector3 hitPos)
		{
			Vector3 d = q - p;
			Vector3 m = sa - p;
			Vector3 n = sb - sa;

			float md = Vector3.Dot(m, d);
			float nd = Vector3.Dot(n, d);
			float dd = Vector3.Dot(d, d);

			if (md < 0f && md + nd < 0f)
			{
				return IntersectRaySphere(sa, n.normalized, p, radius, out hitPos);
			}
			if (md > dd && md + nd > dd)
			{
				return IntersectRaySphere(sa, n.normalized, q, radius, out hitPos);
			}

			float nn = Vector3.Dot(n, n);
			float mn = Vector3.Dot(m, n);
			float a = dd * nn - nd * nd;
			float k = Vector3.Dot(m, m) - radius * radius;
			float c = dd * k - md * md;
			if (Mathf.Abs(a) < Mathf.Epsilon)
			{
				if (c > 0f)
				{
					hitPos = default;
					return false;
				}

				if (md < 0f)
				{
					return IntersectRaySphere(sa, n.normalized, p, radius, out hitPos);
				}
				else if ((md > dd))
				{
					return IntersectRaySphere(sa, n.normalized, q, radius, out hitPos);
				}

				hitPos = sa;
				return true;
			}
			float b = dd * mn - nd * md;
			float discr = b * b - a * c;
			if (discr < 0f)
			{
				hitPos = default;
				return false;
			}

			float t = (-b - Mathf.Sqrt(discr)) / a;
			if (md + t * nd < 0f)
			{
				return IntersectRaySphere(sa, n.normalized, p, radius, out hitPos);
			}
			else if (md + t * nd > dd)
			{
				return IntersectRaySphere(sa, n.normalized, q, radius, out hitPos);
			}

			hitPos = sa + t * n;
			return true;
		}

		static bool IntersectRaySphere(Vector3 p, Vector3 r, Vector3 center, float radius, out Vector3 hitPos)
		{
			Vector3 m = p - center;
			float b = Vector3.Dot(m, r);
			float c = Vector3.Dot(m, m) - radius * radius;
			if (c > 0f && b > 0f)
			{
				hitPos = default;
				return false;
			}

			float discr = b * b - c;
			if (discr < 0f)
			{
				hitPos = default;
				return false;
			}

			float t = -b - Mathf.Sqrt(discr);
			if (t < 0f)
			{
				t = 0f;
			}

			hitPos = p + r * t;
			return true;
		}

		static bool IntersectRayBox(Vector3 size, Vector3 pos, Vector3 rayDir, out Vector3 hitPos)
		{
			Vector3 halfSize = size * 0.5f;
			Vector3 min = -halfSize;
			Vector3 max = halfSize;

			float tmin = 0.0f;
			float tmax = float.MaxValue;

			for (int i = 0; i < 3; i++)
			{
				if (Mathf.Abs(rayDir[i]) < Mathf.Epsilon)
				{
					if (pos[i] < min[i] || pos[i] > max[i])
					{
						hitPos = default;
						return false;
					}
					continue;
				}

				float ood = 1f / rayDir[i];

				float t1 = (min[i] - pos[i]) * ood;
				float t2 = (max[i] - pos[i]) * ood;

				if (t1 > t2)
				{
					float tmp = t1;
					t1 = t2;
					t2 = tmp;
				}
				tmin = Mathf.Max(t1, tmin);
				tmax = Mathf.Min(t2, tmax);
				if (tmin > tmax)
				{
					hitPos = default;
					return false;
				}
			}

			hitPos = pos + rayDir * tmin;
			return true;
		}
	}
}