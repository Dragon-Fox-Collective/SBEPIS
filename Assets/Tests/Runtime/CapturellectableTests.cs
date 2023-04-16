using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using SBEPIS.Capturellection;
using SBEPIS.Utils;
using SBEPIS.Tests.Scenes;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class CapturellectableTests : TestSceneSuite<CapturellectableScene>
	{
		[Test]
		public void Capturellectainer_CapturingItem_PutsItIn()
		{
			Scene.container.Capture(Scene.capturellectable);
			
			Assert.That(Scene.container.CapturedItem, Is.EqualTo(Scene.capturellectable));
		}
		
		[Test]
		public void Capturellectainer_FetchingItem_RemovesIt()
		{
			Scene.container.Capture(Scene.capturellectable);
			Capturellectable item = Scene.container.Fetch();
			
			Assert.That(Scene.container.CapturedItem, Is.Null);
			Assert.That(item, Is.EqualTo(Scene.capturellectable));
		}

		[UnityTest]
		public IEnumerator Capturellector_CapturingItem_PutsItInInventory() => UniTask.ToCoroutine(async () =>
		{
			Scene.capturellector.Inventory = Scene.inventory1;
			CaptureContainer container = await Scene.capturellector.CaptureAndGrabCard(Scene.capturellectable);
			InventoryStorable inventoryCard = Scene.inventory1.FirstOrDefault();
			
			Assert.That(container, Is.Not.Null);
			Assert.That(inventoryCard, Is.Not.Null);
			Assert.That(container.gameObject, Is.EqualTo(inventoryCard.gameObject));
		});

		[UnityTest]
		public IEnumerator Capturellector_FetchingItem_RemovesItFromInventory() => UniTask.ToCoroutine(async () =>
		{
			Scene.capturellector.Inventory = Scene.inventory1;
			CaptureContainer container = await Scene.capturellector.CaptureAndGrabCard(Scene.capturellectable);
			Capturellectable item = await Scene.capturellector.RetrieveAndGrabItem(container);
			
			Assert.That(item, Is.Not.Null);
			Assert.That(item, Is.EqualTo(Scene.capturellectable));
		});
		
		[UnityTest]
		public IEnumerator Capturellector_ChangingInventories_UsesCorrectInventory() => UniTask.ToCoroutine(async () =>
		{
			Scene.capturellector.Inventory = Scene.inventory1;
			Scene.capturellector.Inventory = Scene.inventory2;
			CaptureContainer container = await Scene.capturellector.CaptureAndGrabCard(Scene.capturellectable);
			InventoryStorable inventoryCard = Scene.inventory2.FirstOrDefault();
			
			Assert.That(container, Is.Not.Null);
			Assert.That(inventoryCard, Is.Not.Null);
			Assert.That(container.gameObject, Is.EqualTo(inventoryCard.gameObject));
		});
	}
}