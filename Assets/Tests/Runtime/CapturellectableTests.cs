using NUnit.Framework;
using SBEPIS.Utils;
using SBEPIS.Tests.Scenes;

namespace SBEPIS.Tests
{
	public class CapturellectableTests : TestSceneSuite<CapturellectableScene>
	{
		[Test]
		public void CapturingItem_PutsItInAContainer()
		{
			Scene.capturellectainer.Capture(Scene.capturellectable);
			Assert.That(Scene.capturellectainer.CapturedItem, Is.EqualTo(Scene.capturellectable));
		}
		
		[Test]
		public void FetchingItem_RemovesItFromAContainer()
		{
			Scene.capturellectainer.Capture(Scene.capturellectable);
			Scene.capturellectainer.Fetch();
			Assert.That(Scene.capturellectainer.CapturedItem, Is.Null);
		}
	}
}