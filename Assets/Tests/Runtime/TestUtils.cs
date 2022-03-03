using UnityEditor;
using UnityEngine;

namespace SBEPIS.Utils
{
	public static class TestUtils
	{
		public static T GetTestingPrefab<T>()
		{
			return Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Tests/Runtime/{typeof(T).Name}.prefab")).GetComponent<T>();
		}
	}
}
