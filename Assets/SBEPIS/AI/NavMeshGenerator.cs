using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using MarchingCubesProject;
using SBEPIS.Utils.VectorLinq;
using UnityEngine;
using UnityEngine.Rendering;
using VoxelSystem;

namespace SBEPIS.AI
{
	public class NavMeshGenerator : ValidatedMonoBehaviour
	{
		[SerializeField, Child] private MeshFilter[] meshFilters;
		
		[SerializeField] private float voxelSize;
		[SerializeField] private Mesh voxelMesh;
		
		[SerializeField] private bool addVoxelizedMesh = false;
		[SerializeField] private bool addMarchedMesh = true;
		
		[SerializeField, HideInInspector] private MeshCollider meshCollider;
		
		public void GenerateMesh()
		{
			Mesh mesh = GetChildMeshes();
			mesh = ProcessMesh(mesh);
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
		
		private Mesh ProcessMesh(Mesh mesh)
		{
			VoxelizeMesh(mesh, voxelSize, out List<Voxel_t> voxels, out float scale, out float[,,] voxelField, out Vector3 min);
			Mesh voxelizedMesh = GenerateVoxelBoxMesh(voxels, voxelMesh, scale);
			Mesh marchedMesh = MarchVoxels(voxelField, min, scale);
			return CombineMeshes(transform.worldToLocalMatrix,
				addVoxelizedMesh ? voxelizedMesh : null,
				addMarchedMesh ? marchedMesh : null);
		}
		
		private static void VoxelizeMesh(Mesh mesh, float voxelSize, out List<Voxel_t> voxels, out float scale, out float[,,] voxelField, out Vector3 min)
		{
			int resolution = Mathf.RoundToInt(mesh.bounds.size.Max() / voxelSize);
			CPUVoxelizer.Voxelize(mesh, resolution, out voxels, out scale, out voxelField, out min);
		}
		
		private static Mesh MarchVoxels(float[,,] voxelField, Vector3 min, float scale)
		{
			List<Vector3> vertices = new();
			List<int> indices = new();
			Marching marching = new MarchingCubes();
			marching.Surface = 0.5f;
			marching.Generate(voxelField, vertices, indices);
			
			Mesh mesh = new();
			mesh.indexFormat = IndexFormat.UInt32;
			mesh.vertices = vertices.Select(Matrix4x4.TRS(min, Quaternion.identity, Vector3.one * scale).MultiplyPoint).ToArray();
			mesh.triangles = indices.ToArray();
			return mesh;
		}
		
		private static Mesh GenerateVoxelBoxMesh(IEnumerable<Voxel_t> voxels, Mesh voxelMesh, float scale)
		{
			Mesh mesh = new();
			mesh.indexFormat = IndexFormat.UInt32;
			mesh.CombineMeshes(voxels.Select(voxel => new CombineInstance
				{
					mesh = voxelMesh,
					transform = Matrix4x4.TRS(voxel.position, Quaternion.identity, Vector3.one * scale),
				}
			).ToArray(), true, true, false);
			return mesh;
		}
		
		private static Mesh CombineMeshes(Matrix4x4 transformMatrix, params Mesh[] meshes)
		{
			Mesh totalMesh = new();
			totalMesh.indexFormat = IndexFormat.UInt32;
			totalMesh.CombineMeshes(meshes.Where(mesh => mesh).Select(mesh =>
				new CombineInstance
				{
					mesh = mesh,
					transform = transformMatrix,
				}
			).ToArray(), true, true, false);
			return totalMesh;
		}
		
		private void EnsureMeshIsOnCollider(Mesh mesh)
		{
			if (!meshCollider)
				meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = mesh;
		}
	}
}
