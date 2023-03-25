using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SBEPIS.Utils
{
	public class TestSceneSuite<TScene> : InputTestFixture where TScene : MonoBehaviour
	{
		protected TScene scene { get; private set; }
		
		[SetUp]
		public override void Setup()
		{
			base.Setup();
			
			scene = GetTestingPrefab();
			Assert.IsNotNull(scene);
		}
		
		[TearDown]
		public override void TearDown()
		{
			base.TearDown();

			if (scene)
			{
				Object.Destroy(scene.gameObject);
				scene = null;
			}
		}
		
		private static TScene GetTestingPrefab() => Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Tests/Runtime/{typeof(TScene).Name}.prefab")).GetComponent<TScene>();
	}
}
