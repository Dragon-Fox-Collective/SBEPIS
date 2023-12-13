using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using SBEPIS.Capturellection.Storage;
using SBEPIS.Utils;
using SBEPIS.Tests.Scenes;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class InventoryTests : TestSceneSuite<InventoryScene>
	{
		public InventoryTests() : base(true) {}
		
		[UnityTest]
		public IEnumerator StoringItem_GetsCard() => UniTask.ToCoroutine(async () =>
		{
			StoreResult result = await Scene.inventory.StoreItem(Scene.item);
			Assert.That(result.Card, Is.Not.Null);
		});
		
		[UnityTest]
		public IEnumerator FetchingItem_GetsOriginalItem() => UniTask.ToCoroutine(async () =>
		{
			StoreResult storeResult = await Scene.inventory.StoreItem(Scene.item);
			FetchResult fetchResult = await Scene.inventory.FetchItem(storeResult.Card);
			Assert.That(fetchResult.FetchedItem, Is.EqualTo(Scene.item));
		});

		[Test]
		public void Inventory_HasStartingCards()
		{
			Assert.That(Scene.inventory.Count(), Is.EqualTo(1));
		}
	}
}