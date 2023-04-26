using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using SBEPIS.Capturellection;
using SBEPIS.Capturellection.Storage;
using SBEPIS.Utils;
using SBEPIS.Tests.Scenes;
using UnityEngine;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class InventoryTests : TestSceneSuite<InventoryScene>
	{
		[UnityTest]
		public IEnumerator StoringItem_GetsCard() => UniTask.ToCoroutine(async () =>
		{
			StorableStoreResult result = await Scene.inventory.Store(Scene.item);
			Assert.That(result.card, Is.Not.Null);
		});
		
		[UnityTest]
		public IEnumerator FetchingItem_GetsOriginalItem() => UniTask.ToCoroutine(async () =>
		{
			StorableStoreResult result = await Scene.inventory.Store(Scene.item);
			Capturellectable item = await Scene.inventory.Fetch(result.card);
			Assert.That(item, Is.EqualTo(Scene.item));
		});

		[Test]
		public void Inventory_HasStartingCards()
		{
			Assert.That(Scene.inventory.Count(), Is.EqualTo(1));
		}
		
		[UnityTest]
		public IEnumerator StoringItem_SetsCardParent() => UniTask.ToCoroutine(async () =>
		{
			StorableStoreResult result = await Scene.inventory.Store(Scene.item);
			Assert.That(result.card.transform.parent, Is.EqualTo(Scene.inventory.CardParent));
		});
		
		[UnityTest]
		public IEnumerator FetchingItem_UnsetsCardParent() => UniTask.ToCoroutine(async () =>
		{
			StorableStoreResult result = await Scene.inventory.Store(Scene.item);
			await Scene.inventory.Fetch(result.card);
			Assert.That(result.card.transform.parent, Is.Null);
		});
	}
}