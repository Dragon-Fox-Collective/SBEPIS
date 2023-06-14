using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SBEPIS.Utils.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace SBEPIS.Utils
{
	public class TestSceneSuite<TScene> where TScene : MonoBehaviour
	{
		protected TScene Scene { get; private set; }
		
		private bool requiresStart = false;
		
		public TestSceneSuite() : this(false) { }
		public TestSceneSuite(bool requiresStart)
		{
			this.requiresStart = requiresStart;
		}
		
		[UnitySetUp]
		public IEnumerator Setup()
		{
			Scene = GetTestingPrefab();
			Assert.That(Scene, Is.Not.Null);
			
			if (requiresStart) yield return 0;
		}
		
		[UnityTearDown]
		public IEnumerator TearDown()
		{
			if (Scene)
			{
				GetRootObjects().ForEach(Object.Destroy);
				Scene = null;
			}
			
			yield return 0;
		}
		
		private IEnumerable<GameObject> GetRootObjects() => Scene.gameObject.scene.GetRootGameObjects().Where(gameObject => gameObject.name != "Code-based tests runner");
		
		private static TScene GetTestingPrefab()
		{
			GameObject obj = (GameObject) PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Tests/Runtime/{typeof(TScene).Name}.prefab"));
			PrefabUtility.UnpackPrefabInstance(obj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
			return obj.GetComponent<TScene>();
		}
	}
}
