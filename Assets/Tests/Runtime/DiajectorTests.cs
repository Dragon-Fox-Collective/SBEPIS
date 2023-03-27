using System.Collections;
using NUnit.Framework;
using SBEPIS.Tests.Scenes;
using SBEPIS.Utils;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class DiajectorTests : TestSceneSuite<DiajectorScene>
	{
		[UnityTest]
		public IEnumerator CardsChangeTexture_WhenNotChanged()
		{
			Assert.That(scene.card.GetComponent<SplitTextureSetup>().textures, Is.EquivalentTo(scene.startingDeque.definition.ruleset.GetCardTextures()));
			yield break;
		}
		
		[UnityTest]
		public IEnumerator CardsChangeTexture_WhenChangedBeforeCreation()
		{
			scene.dequeOwner.Deque = scene.changeDeque;
			Assert.That(scene.card.GetComponent<SplitTextureSetup>().textures, Is.EquivalentTo(scene.changeDeque.definition.ruleset.GetCardTextures()));
			yield break;
		}
		
		[UnityTest]
		public IEnumerator CardsChangeTexture_WhenChangedAfterCreation_WithDiajectorOpen()
		{
			scene.diajector.ForceOpen();
			scene.dequeOwner.Deque = scene.changeDeque;
			Assert.That(scene.card.GetComponent<SplitTextureSetup>().textures, Is.EquivalentTo(scene.changeDeque.definition.ruleset.GetCardTextures()));
			yield break;
		}
		
		[UnityTest]
		public IEnumerator CardsChangeTexture_WhenChangedAfterCreation_WithDiajectorClosed()
		{
			scene.diajector.ForceOpen();
			scene.diajector.ForceClose();
			scene.dequeOwner.Deque = scene.changeDeque;
			Assert.That(scene.card.GetComponent<SplitTextureSetup>().textures, Is.EquivalentTo(scene.changeDeque.definition.ruleset.GetCardTextures()));
			yield break;
		}
	}
}