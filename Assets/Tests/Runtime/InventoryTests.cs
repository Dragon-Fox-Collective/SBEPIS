using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using SBEPIS.Capturellection;
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
			(DequeStorable card, _, _) = await Scene.inventory.Store(Scene.item);
			Assert.That(card, Is.Not.Null);
		});
		
		[UnityTest]
		public IEnumerator FetchingItem_GetsOriginalItem() => UniTask.ToCoroutine(async () =>
		{
			(DequeStorable card, _, _) = await Scene.inventory.Store(Scene.item);
			Capturellectable item = await Scene.inventory.Fetch(card);
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
			(DequeStorable card, _, _) = await Scene.inventory.Store(Scene.item);
			Assert.That(card.transform.parent, Is.EqualTo(Scene.inventory.cardParent));
		});
		
		[UnityTest]
		public IEnumerator FetchingItem_UnsetsCardParent() => UniTask.ToCoroutine(async () =>
		{
			(DequeStorable card, _, _) = await Scene.inventory.Store(Scene.item);
			await Scene.inventory.Fetch(card);
			Assert.That(card.transform.parent, Is.Null);
		});
	}
}