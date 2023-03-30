using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace SBEPIS.Utils
{
	public class TestSceneSuite<TScene> : InputTestFixture where TScene : MonoBehaviour
	{
		protected TScene Scene { get; private set; }
		
		[SetUp]
		public override void Setup()
		{
			base.Setup();
			
			Scene = GetTestingPrefab();
			Assert.That(Scene, Is.Not.Null);
		}
		
		[TearDown]
		public override void TearDown()
		{
			base.TearDown();

			if (Scene)
			{
				List<GameObject> leftoverGameObjects = Scene.gameObject.scene.GetRootGameObjects().Where(gameObject =>
					gameObject &&
					gameObject != Scene.gameObject &&
					gameObject.name != "Code-based tests runner").ToList();
				if (leftoverGameObjects.Count > 0)
					Debug.LogError($"Test didn't clean itself up! Still had {leftoverGameObjects.ToDelimString()}");
				
				Object.Destroy(Scene.gameObject);
				Scene = null;
			}
		}
		
		private static TScene GetTestingPrefab() => Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Tests/Runtime/{typeof(TScene).Name}.prefab")).GetComponent<TScene>();
	}
}
