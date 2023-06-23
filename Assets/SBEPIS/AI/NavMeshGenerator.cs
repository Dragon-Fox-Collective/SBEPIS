using System;
using System.Linq;
using KBCore.Refs;
using SBEPIS.Utils.Linq;
using UnityEngine;
using UnityMeshSimplifier;

namespace SBEPIS.AI
{
	public class NavMeshGenerator : ValidatedMonoBehaviour
	{
		[SerializeField, Child] private MeshFilter[] meshFilters;
		
		[SerializeField, Range(0f, 1f)] private float quality = 0.5f;
		[SerializeField] private SimplificationOptions simplificationOptions = SimplificationOptions.Default;
		
		[SerializeField, HideInInspector] private MeshCollider meshCollider;
		
		public void GenerateMesh()
		{
			Mesh mesh = new();
			mesh.CombineMeshes(meshFilters.Select(meshFilter => new CombineInstance
			{
				mesh = meshFilter.sharedMesh,
				transform = meshFilter.transform.localToWorldMatrix * transform.worldToLocalMatrix,
			}
			).ToArray(), true, true, false);
			
			MeshSimplifier simplifier = new();
			simplifier.Initialize(mesh);
			
			simplifier.SimplificationOptions = simplificationOptions;
			simplifier.SimplifyMesh(quality);
			
			mesh = simplifier.ToMesh();
			
			if (!meshCollider)
				meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = mesh;
		}
	}
}
