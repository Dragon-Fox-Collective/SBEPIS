using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SBEPIS.Thaumaturgy;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class AlchemyTestSuite
	{
		private AlchemyScene scene;

		[SetUp]
		public void Setup()
		{
			scene = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SBEPIS/TestScenes/AlchemyScene.prefab")).GetComponent<AlchemyScene>();
		}

		[TearDown]
		public void Teardown()
		{
			Object.Destroy(scene.gameObject);
		}

		[UnityTest]
		public IEnumerator PunchingCaptureCardsMakesHoles()
		{
			Punchable card = Object.Instantiate(scene.captureCard).GetComponent<Punchable>();
			card.Punch(CaptchaUtil.HashCaptcha("10000000"));

			for (int i = 0; i < card.punchHoles.sharedMesh.blendShapeCount; i++)
			{
				string blendshape = card.punchHoles.sharedMesh.GetBlendShapeName(i);
				Debug.Log($"Checking {blendshape}");
				if (blendshape == "Key 1" || blendshape.StartsWith("Key 1 +"))
					Assert.AreEqual(100, card.punchHoles.GetBlendShapeWeight(i));
				else
					Assert.AreEqual(0, card.punchHoles.GetBlendShapeWeight(i));
			}

			yield return null;
		}
	}
}