using System.Collections;
using System.Linq;
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
			Assert.That(scene.inventory.First().GetComponent<SplitTextureSetup>().textures, Is.EquivalentTo(scene.startingDeque.definition.ruleset.GetCardTextures()));
			yield break;
		}
		
		[UnityTest]
		public IEnumerator CardsChangeTexture_WhenChangedBeforeCreation()
		{
			scene.dequeOwner.Deque = scene.changeDeque;
			Assert.That(scene.inventory.First().GetComponent<SplitTextureSetup>().textures, Is.EquivalentTo(scene.changeDeque.definition.ruleset.GetCardTextures()));
			yield break;
		}
		
		[UnityTest]
		public IEnumerator CardsChangeTexture_WhenChangedAfterCreation_WithDiajectorOpen()
		{
			scene.diajector.ForceOpen();
			yield return 0;
			scene.dequeOwner.Deque = scene.changeDeque;
			Assert.That(scene.inventory.First().GetComponent<SplitTextureSetup>().textures, Is.EquivalentTo(scene.changeDeque.definition.ruleset.GetCardTextures()));
		}
		
		[UnityTest]
		public IEnumerator CardsChangeTexture_WhenChangedAfterCreation_WithDiajectorClosed()
		{
			scene.diajector.ForceOpen();
			yield return 0;
			scene.diajector.ForceClose();
			yield return 0;
			scene.dequeOwner.Deque = scene.changeDeque;
			Assert.That(scene.inventory.First().GetComponent<SplitTextureSetup>().textures, Is.EquivalentTo(scene.changeDeque.definition.ruleset.GetCardTextures()));
		}
	}
}