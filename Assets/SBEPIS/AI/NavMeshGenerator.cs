using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using NaughtyAttributes;
using SBEPIS.Utils.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace SBEPIS.AI
{
	public class NavMeshGenerator : ValidatedMonoBehaviour
	{
		[SerializeField, Child] private MeshFilter[] meshFilters;
		
		[SerializeField, HideInInspector] private NavMesh navMeshComponent;
		
		[Button]
		public void GenerateMesh()
		{
			Mesh mesh = GetChildMeshes();
			mesh = MergeSameVertices(mesh, 0.001f);
			EnsureMeshIsOnCollider(mesh);
		}
		
		private Mesh GetChildMeshes()
		{
			Mesh mesh = new();
			mesh.CombineMeshes(meshFilters.Select(meshFilter => new CombineInstance
				{
					mesh = meshFilter.sharedMesh,
					transform = meshFilter.transform.localToWorldMatrix,
				}
			).ToArray(), true, true, false);
			return mesh;
		}
		
		private static Mesh MergeSameVertices(Mesh mesh, float epsilon)
		{
			List<Vector3> newVertices = new();
			Dictionary<int, int> vertexMapping = new();
			
			bool TryMerge(int vertexIndex, Vector3 vertex)
			{
				foreach ((int newVertexIndex, Vector3 newVertex) in newVertices.Enumerate())
				{
					if (Vector3.Distance(vertex, newVertex) < epsilon)
					{
						vertexMapping.Add(vertexIndex, newVertexIndex);
						return true;
					}
				}
				return false;
			}
			
			foreach ((int vertexIndex, Vector3 vertex) in mesh.vertices.Enumerate())
			{
				if (!TryMerge(vertexIndex, vertex))
				{
					newVertices.Add(vertex);
					vertexMapping.Add(vertexIndex, newVertices.Count - 1);
				}
			}
			
			Mesh newMesh = new();
			newMesh.indexFormat = IndexFormat.UInt32;
			newMesh.vertices = newVertices.ToArray();
			newMesh.triangles = mesh.triangles.Select(index => vertexMapping[index]).ToArray();
			newMesh.RecalculateNormals();
			return newMesh;
		}
		
		private void EnsureMeshIsOnCollider(Mesh mesh)
		{
			if (!navMeshComponent)
				navMeshComponent = gameObject.AddComponent<NavMesh>();
			navMeshComponent.Mesh = mesh;
		}
	}
}
