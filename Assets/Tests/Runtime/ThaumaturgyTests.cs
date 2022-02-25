using System.Collections;
using NUnit.Framework;
using SBEPIS.Thaumaturgy;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.TestTools;

namespace SBEPIS.Tests.PlayMode
{
	public class ThaumaturgyTests
	{
		private ThaumaturgyScene scene;

		[SetUp]
		public void Setup()
		{
			scene = TestUtils.GetTestingPrefab<ThaumaturgyScene>();
		}

		[TearDown]
		public void Teardown()
		{
			Object.Destroy(scene.gameObject);
		}

		[UnityTest]
		public IEnumerator PunchingCaptureCardsMakesHoles()
		{
			Punchable card = Object.Instantiate(scene.captureCardPrefab).GetComponent<Punchable>();
			card.Punch(CaptureCodeUtils.HashCaptureCode("10000000"));

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