using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Utils.Linq;
using SBEPIS.Utils.VectorLinq;
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
		
		private void RegenerateNodes()
		{
			IEnumerable<(int, int)> EdgesOf((int, int, int) tri) => LINQ.Of((tri.Item1, tri.Item2), (tri.Item1, tri.Item3), (tri.Item2, tri.Item3));
			
			List<(int, int, int)> triangles = mesh.triangles.Chunk(3).Select(tri =>
			{
				List<int> indices = tri.ToList();
				indices.Sort();
				return (indices[0], indices[1], indices[2]);
			}).ToList();
			
			Dictionary<(int, int), ((int, int, int), (int, int, int))> pairedTriangles = new();
			foreach ((int, int, int) tri in triangles)
			foreach ((int, int) edge in EdgesOf(tri))
				if (!pairedTriangles.TryAdd(edge, (tri, (-1, -1, -1))))
					pairedTriangles[edge] = (pairedTriangles[edge].Item1, tri);
			
			Dictionary<(int, int, int), Node> triNodes = triangles.Zip(mesh.triangles.Chunk(3).Select(chunk => chunk.Select(vert => mesh.vertices[vert]).ToArray())).ToDictionary(zip => zip.Item1, zip => new Node(
				zip.Item2.Sum() / 3f,
				Vector3.Cross(zip.Item2[0] - zip.Item2[1], zip.Item2[2] - zip.Item2[1])));
			nodes = triNodes.Process(pair => pair.Value.AddConnections(EdgesOf(pair.Key).Select(edge => triNodes[pairedTriangles[edge].Item1] == pair.Value ? triNodes[pairedTriangles[edge].Item2] : triNodes[pairedTriangles[edge].Item1]))).Select(pair => pair.Value).ToList();
		}
		
		public IEnumerable<Vector3> PathFromTo(Vector3 from, Vector3 to, Predicate<Node> predicate = null)
		{
			Node GetClosestNodeTo(Vector3 position) => nodes.CompareBy(node => Vector3.Distance(position, node.Position), float.MaxValue, (current, distance) => distance < current);
			
			Node fromNode = GetClosestNodeTo(from);
			Node toNode = GetClosestNodeTo(to);
			
			HashSet<Node> accountedFor = new();
			List<PathingNode> toCheck = new(){ new PathingNode { node = fromNode, target = toNode } };
			
			while (toCheck.Any())
			{
				PathingNode node = toCheck.Min();
				toCheck.Remove(node);
				
				if (node.node == toNode)
					return node.Path.Select(pathNode => pathNode.Position).Prepend(from).Append(to);
				
				int toCheckBeforeCount = toCheck.Count;
				toCheck.AddRange(node.node.ConnectedNodes.Where(connectedNode => !accountedFor.Contains(connectedNode)).Where(connectedNode => predicate == null || predicate(connectedNode)).Select(connectedNode => new PathingNode
					{
						node = connectedNode,
						previousNode = node,
						target = toNode,
					}
				));
				accountedFor.AddRange(toCheck.Skip(toCheckBeforeCount).Select(connectedNode => connectedNode.node));
			}
			
			return null;
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
			
			public int CompareTo(PathingNode other) => Heuristic.CompareTo(other.Heuristic);
		}
	}
}
