using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Utils.Linq;
using UnityEngine;

namespace SBEPIS.AI
{
	public class NavMesh : MonoBehaviour
	{
		[SerializeField] private bool showNodes;
		
		[SerializeField] private Mesh mesh;
		public Mesh Mesh
		{
			get => mesh;
			set
			{
				mesh = value;
				RegenerateNodes();
			}
		}
		
		[SerializeReference, HideInInspector] private List<Node> nodes = new();
		
		[SerializeField] private List<Vector3[]> debugPaths = new();
		
		private void RegenerateNodes()
		{
			List<(int, int, int)> triangles = mesh.triangles.Chunk(3).Select(SortedIndices).ToList();
			
			Dictionary<(int, int, int), Node> triangleNodes = triangles.ToDictionary(triangle => triangle, triangle => new Node(mesh, triangle));
			
			Dictionary<(int, int), Node> singleNodeEdges = new();
			foreach (((int, int, int) triangle, Node node) in triangleNodes)
			foreach ((int, int) edge in EdgesOf(triangle))
				if (!singleNodeEdges.TryAdd(edge, node))
				{
					node.AddConnection(singleNodeEdges[edge]);
					singleNodeEdges[edge].AddConnection(node);
					singleNodeEdges.Remove(edge);
				}
			
			nodes = triangleNodes.Select(pair => pair.Value).ToList();
		}
		
		private static IEnumerable<(int, int)> EdgesOf((int, int, int) tri) => LINQ.Of((tri.Item1, tri.Item2), (tri.Item1, tri.Item3), (tri.Item2, tri.Item3));
		
		private static (int, int, int) SortedIndices(IEnumerable<int> chunk)
		{
			IEnumerator<int> enumerator = chunk.GetEnumerator();
			enumerator.MoveNext();
			int a = enumerator.Current;
			enumerator.MoveNext();
			int b = enumerator.Current;
			enumerator.MoveNext();
			int c = enumerator.Current;
			enumerator.Dispose();
			return SortedIndices((a, b, c));
		}
		private static (int, int, int) SortedIndices((int, int, int) triangle)
		{
			int first = Mathf.Min(Mathf.Min(triangle.Item1, triangle.Item2), triangle.Item3);
			int last = Mathf.Max(Mathf.Max(triangle.Item1, triangle.Item2), triangle.Item3);
			bool IsFirstOrLast(int index) => index == first || index == last;
			int middle = IsFirstOrLast(triangle.Item1) && IsFirstOrLast(triangle.Item2) ? triangle.Item3
				: IsFirstOrLast(triangle.Item1) && IsFirstOrLast(triangle.Item3) ? triangle.Item2
				: triangle.Item1;
			return (first, middle, last);
		}
		
		public async UniTask<IEnumerable<Vector3>> PathFromTo(Vector3 from, Vector3 to, Predicate<Node> predicate = null)
		{
			Node fromNode = GetClosestNodeTo(from);
			Node toNode = GetClosestNodeTo(to);
			
			HashSet<Node> accountedFor = new();
			List<PathingNode> toCheck = new(){ new PathingNode { node = fromNode, target = toNode } };
			
			while (toCheck.Any())
			{
				PathingNode node = PopShortestPathingNode(toCheck);
				
				if (node.node == toNode)
					return node.GetWaypoints(from, to);
				
				List<PathingNode> newNodesToExplore = node.node.ConnectedNodes.Where(connectedNode => !accountedFor.Contains(connectedNode)).Where(connectedNode => predicate == null || predicate(connectedNode)).Select(connectedNode => new PathingNode
					{
						node = connectedNode,
						previousNode = node,
						target = toNode,
					}
				).ToList();
				toCheck.AddRange(newNodesToExplore);
				accountedFor.AddRange(newNodesToExplore.Select(connectedNode => connectedNode.node));
				
				Vector3[] debugPath = node.Path.Select(debugNode => debugNode.Position).ToArray();
				debugPaths.Add(debugPath);
				await UniTask.Delay(100);
				debugPaths.Remove(debugPath);
			}
			
			return null;
		}
		
		private Node GetClosestNodeTo(Vector3 position) => nodes.CompareBy(node => Vector3.Distance(position, node.Position), float.MaxValue, (current, distance) => distance < current);
		
		private static PathingNode PopShortestPathingNode(List<PathingNode> nodes)
		{
			PathingNode node = nodes.Min();
			nodes.Remove(node);
			return node;
		}
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color32(145, 244, 139, 210);
			if (mesh)
				Gizmos.DrawWireMesh(mesh);
			
			if (showNodes)
			{
				foreach (Node node in nodes)
				{
					Gizmos.color = Color.green;
					Gizmos.DrawWireSphere(node.Position, 0.04f);
					
					Gizmos.color = Color.red;
					foreach (Node connectedNode in node.ConnectedNodes)
						Gizmos.DrawLine(node.Position, connectedNode.Position);
					
					Gizmos.color = Color.blue;
					Gizmos.DrawRay(node.Position, node.Normal * 0.2f);
				}
			}
			
			Gizmos.color = Color.green;
			foreach (Vector3[] debugPath in debugPaths)
				foreach ((Vector3 a, Vector3 b) in debugPath[..^1].Zip(debugPath[1..]))
					Gizmos.DrawLine(a, b);
		}
		
		[Serializable]
		public class Node
		{
			[SerializeField] private Vector3 position;
			public Vector3 Position => position;
			[SerializeField] private Vector3 normal;
			public Vector3 Normal => normal;
			[SerializeReference] private List<Node> connectedNodes = new();
			public ReadOnlyCollection<Node> ConnectedNodes => connectedNodes.AsReadOnly();
			
			public Node(Vector3 position, Vector3 normal)
			{
				this.position = position;
				this.normal = normal;
			}
			
			public Node(Mesh mesh, (int, int, int) triangle) : this(
				(mesh.vertices[triangle.Item1] + mesh.vertices[triangle.Item2] + mesh.vertices[triangle.Item3]) / 3f,
				Vector3.Cross(mesh.vertices[triangle.Item1] - mesh.vertices[triangle.Item2], mesh.vertices[triangle.Item3] - mesh.vertices[triangle.Item2])) {}
			
			public void AddConnection(Node node)
			{
				connectedNodes.Add(node);
			}
			
			public void AddConnections(IEnumerable<Node> nodes)
			{
				connectedNodes.AddRange(nodes);
			}
		}
		
		[Serializable]
		private class PathingNode : IComparable<PathingNode>
		{
			public Node node;
			public PathingNode previousNode;
			public Node target;
			
			public float DistanceTravelled => previousNode == null ? 0 : previousNode.DistanceTravelled + Vector3.Distance(node.Position, previousNode.node.Position);
			public float DistanceToGo => Vector3.Distance(node.Position, target.Position);
			public float Heuristic => DistanceTravelled + DistanceToGo;
			
			public IEnumerable<Node> Path
			{
				get
				{
					if (previousNode != null)
						foreach (Node prevNode in previousNode.Path)
							yield return prevNode;
					
					yield return node;
				}
			}
			
			public IEnumerable<Vector3> GetWaypoints(Vector3 from, Vector3 to) => Path.Select(pathNode => pathNode.Position).Prepend(from).Append(to);
			
			public int CompareTo(PathingNode other) => Heuristic.CompareTo(other.Heuristic);
		}
	}
}
