using System.Collections;
using System.Linq;
using NUnit.Framework;
using SBEPIS.Tests.Scenes;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class DiajectorTests : TestSceneSuite<DiajectorScene>
	{
		[UnityTest]
		public IEnumerator CardsGetParented()
		{
			Assert.That(Scene.inventory.First().transform.parent, Is.Not.Null);
			yield break;
		}
		
		[UnityTest]
		public IEnumerator CardsChangeTexture_WhenNotChanged()
		{
			Assert.That(Scene.inventory.First().GetComponent<SplitTextureSetup>().Textures, Is.Not.Null.And.EquivalentTo(Scene.startingDeque.definition.ruleset.GetCardTextures()));
			yield break;
		}
		
		[UnityTest]
		public IEnumerator CardsChangeTexture_WhenChangedBeforeCreation()
		{
			Scene.dequeOwner.Deque = Scene.changeDeque;
			yield return 0;
			Assert.That(Scene.inventory.First().GetComponent<SplitTextureSetup>().Textures, Is.Not.Null.And.EquivalentTo(Scene.changeDeque.definition.ruleset.GetCardTextures()));
		}
		
		[UnityTest]
		public IEnumerator CardsChangeTexture_WhenChangedAfterCreation_WithDiajectorOpen()
		{
			Scene.diajector.ForceOpen();
			yield return 0;
			Scene.dequeOwner.Deque = Scene.changeDeque;
			yield return 0;
			Assert.That(Scene.inventory.First().GetComponent<SplitTextureSetup>().Textures, Is.Not.Null.And.EquivalentTo(Scene.changeDeque.definition.ruleset.GetCardTextures()));
		}
		
		[UnityTest]
		public IEnumerator CardsChangeTexture_WhenChangedAfterCreation_WithDiajectorClosed()
		{
			Scene.diajector.ForceOpen();
			yield return 0;
			Scene.diajector.ForceClose();
			yield return 0;
			Scene.dequeOwner.Deque = Scene.changeDeque;
			yield return 0;
			Assert.That(Scene.inventory.First().GetComponent<SplitTextureSetup>().Textures, Is.Not.Null.And.EquivalentTo(Scene.changeDeque.definition.ruleset.GetCardTextures()));
		}
	}
}