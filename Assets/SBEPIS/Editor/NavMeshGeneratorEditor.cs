using UnityEditor;
using UnityEngine;

namespace SBEPIS.AI
{
	[CustomEditor(typeof(NavMeshGenerator))]
	public class NavMeshGeneratorEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			NavMeshGenerator navMeshGenerator = (NavMeshGenerator)target;
			
			DrawDefaultInspector();
			
			if (GUILayout.Button("Generate Mesh"))
				navMeshGenerator.GenerateMesh();
		}
	}
}