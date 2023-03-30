using System;
using System.Collections;
using NUnit.Framework;
using SBEPIS.Bits;
using SBEPIS.Capturellection;
using SBEPIS.Tests.Scenes;
using SBEPIS.Thaumergy;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class ThaumergyTests : TestSceneSuite<ThaumergyScene>
	{
		[UnityTest]
		public IEnumerator PunchingCaptureCardsMakesHoles()
		{
			/*Punchable card = Object.Instantiate(scene.captureCardPrefab, scene.transform).GetComponent<Punchable>();
			card.Punch(BitSet.FromCode("BAAAAAAA"));

			for (int i = 0; i < card.punchHoles.sharedMesh.blendShapeCount; i++)
			{
				string blendshape = card.punchHoles.sharedMesh.GetBlendShapeName(i);
				Debug.Log($"Checking {blendshape}");
				if (blendshape == "Key 1" || blendshape.StartsWith("Key 1 +"))
					Assert.AreEqual(100, card.punchHoles.GetBlendShapeWeight(i));
				else
					Assert.AreEqual(0, card.punchHoles.GetBlendShapeWeight(i));
			}

			yield return null;*/
			throw new NotImplementedException();
		}
		
		[UnityTest]
		public IEnumerator CaptureCameraTakesPicturesOfObjects()
		{
			Assert.IsNotNull(CaptureCamera.instance.TakePictureOfObject(Scene.obj));

			yield return null;
		}
	}
}