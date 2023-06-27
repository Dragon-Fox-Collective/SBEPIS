using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using MarchingCubesProject;
using NaughtyAttributes;
using SBEPIS.Utils.Linq;
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
		
		[SerializeField] private bool addVoxelizedMesh = false;
		[SerializeField, ShowIf("addVoxelizedMesh")] private Mesh voxelMesh;
		
		[SerializeField] private bool addMarchedMesh = true;
		
		[SerializeField] private bool useGPU = false;
		[SerializeField, ShowIf("useGPU")] private ComputeShader GPUVoxelizerShader;
		
		[SerializeField, HideInInspector] private MeshCollider meshCollider;
		
		[Button]
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
			VoxelizeMesh(mesh, voxelSize, out IEnumerable<Voxel_t> voxels, out float scale, out Voxel_t[,,] voxelField, out Vector3 min, useGPU);
			Mesh voxelizedMesh = GenerateVoxelBoxMesh(voxelField.Cast<Voxel_t>(), voxelMesh, scale);
			Mesh marchedMesh = MarchVoxels(voxelField, min, scale);
			return CombineMeshes(transform.worldToLocalMatrix,
				addVoxelizedMesh ? voxelizedMesh : null,
				addMarchedMesh ? marchedMesh : null);
		}
		
		private void VoxelizeMesh(Mesh mesh, float voxelSize, out IEnumerable<Voxel_t> voxels, out float scale, out Voxel_t[,,] voxelField, out Vector3 min, bool useGPU = false)
		{
			int resolution = Mathf.RoundToInt(mesh.bounds.size.Max() / voxelSize);
			
			if (useGPU)
			{
				GPUVoxelData voxelData = GPUVoxelizer.Voxelize(GPUVoxelizerShader, mesh, resolution, true, false);
				voxels = voxelData.GetData();
				scale = voxelData.UnitLength;
				voxelField = new Voxel_t[0, 0, 0];
				min = voxelData.Start;
			}
			else
			{
				CPUVoxelizer.Voxelize(mesh, resolution, out List<Voxel_t> voxelList, out scale, out voxelField, out min);
				voxels = voxelList;
			}
		}
		
		private static Mesh MarchVoxels(Voxel_t[,,] voxelField, Vector3 min, float scale)
		{
			List<Vector3> vertices = new();
			List<int> indices = new();
			Marching marching = new MarchingCubes();
			marching.Surface = 0.5f;
			marching.Generate(voxelField.Cast<Voxel_t>().Select(voxel => voxel.IsEmpty() ? 0f : 1f).ToList(),
				voxelField.GetLength(0), voxelField.GetLength(1), voxelField.GetLength(2),
				vertices, indices);
			
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
			mesh.CombineMeshes(voxels.Where(voxel => !voxel.IsEmpty()).Select(voxel => new CombineInstance
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
