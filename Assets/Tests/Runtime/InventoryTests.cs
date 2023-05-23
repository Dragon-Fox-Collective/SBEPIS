using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using SBEPIS.Capturellection;
using SBEPIS.Capturellection.Storage;
using SBEPIS.Utils;
using SBEPIS.Tests.Scenes;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class InventoryTests : TestSceneSuite<InventoryScene>
	{
		[UnityTest]
		public IEnumerator StoringItem_GetsCard() => UniTask.ToCoroutine(async () =>
		{
			StoreResult result = await Scene.inventory.StoreItem(Scene.item);
			Assert.That(result.card, Is.Not.Null);
		});
		
		[UnityTest]
		public IEnumerator FetchingItem_GetsOriginalItem() => UniTask.ToCoroutine(async () =>
		{
			StoreResult storeResult = await Scene.inventory.StoreItem(Scene.item);
			FetchResult fetchResult = await Scene.inventory.FetchItem(storeResult.card);
			Assert.That(fetchResult.fetchedItem, Is.EqualTo(Scene.item));
		});

		[Test]
		public void Inventory_HasStartingCards()
		{
			Assert.That(Scene.inventory.Count(), Is.EqualTo(1));
		}
	}
}