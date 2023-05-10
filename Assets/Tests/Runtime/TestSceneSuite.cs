using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace SBEPIS.Utils
{
	public class TestSceneSuite<TScene> where TScene : MonoBehaviour
	{
		protected TScene Scene { get; private set; }
		
		[SetUp]
		public void Setup()
		{
			Scene = GetTestingPrefab();
			Assert.That(Scene, Is.Not.Null);
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
		
		private static TScene GetTestingPrefab() => Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Tests/Runtime/{typeof(TScene).Name}.prefab")).GetComponent<TScene>();
	}
}
