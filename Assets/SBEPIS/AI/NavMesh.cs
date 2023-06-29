using System;
using System.Collections.Generic;
using System.Linq;
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
			
			Dictionary<(int, int, int), Node> triNodes = triangles.ToDictionary(tri => tri, tri => new Node { position = LINQ.Of(tri.Item1, tri.Item2, tri.Item3).Select(vert => mesh.vertices[vert]).Sum() / 3f });
			nodes = triNodes.Select(pair =>
			{
				pair.Value.connectedNodes = EdgesOf(pair.Key).Select(edge => triNodes[pairedTriangles[edge].Item1] == pair.Value ? triNodes[pairedTriangles[edge].Item2] : triNodes[pairedTriangles[edge].Item1]).ToList();
				return pair.Value;
			}).ToList();
		}
		
		public Vector3[] PathFromTo(Vector3 from, Vector3 to)
		{
			Node GetClosestNodeTo(Vector3 position) => nodes.CompareBy(node => Vector3.Distance(position, node.position), float.MaxValue, (current, distance) => distance < current);
			
			Node fromNode = GetClosestNodeTo(from);
			Node toNode = GetClosestNodeTo(to);
			
			Dictionary<Node, PathingNode> alreadyChecked = new();
			List<PathingNode> toCheck = new(){ new PathingNode { node = fromNode } };
			
			while (toCheck.Any())
			{
				
			}
		}
		
		private void OnDrawGizmos()
		{
			Gizmos.color = new Color(0.2f, 0.8f, 0.1f, 0.125f);
			if (mesh)
				Gizmos.DrawWireMesh(mesh);

			if (showNodes)
			{
				foreach (Node node in nodes)
				{
					Gizmos.color = Color.green;
					Gizmos.DrawWireSphere(node.position, 0.04f);

					Gizmos.color = Color.red;
					foreach (Node connectedNode in node.connectedNodes)
						Gizmos.DrawLine(node.position, connectedNode.position);
				}
			}
		}
		
		[Serializable]
		private class Node
		{
			public Vector3 position;
			[SerializeReference] public List<Node> connectedNodes = new();
		}
		
		[Serializable]
		private class PathingNode
		{
			public Node node;
			public PathingNode previousNode;
			
			public float Distance => previousNode == null ? 0 : previousNode.Distance + Vector3.Distance(node.position, previousNode.node.position);
		}
	}
}
