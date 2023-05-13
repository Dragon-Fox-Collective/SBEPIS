using NUnit.Framework;
using SBEPIS.Utils;
using SBEPIS.Tests.Scenes;

namespace SBEPIS.Tests
{
	public class PlayerSystemTests : TestSceneSuite<PlayerSystemScene>
	{
		[Test]
		public void UsingDeque_ChangesCapturellectorInventory()
		{
			Scene.grabber.Rigidbody.MovePosition(Scene.dequeBoxGrabbable.transform.position);
			Scene.grabber.GrabManually(Scene.dequeBoxGrabbable);
			Scene.grabber.UseHeldItem();
			Assert.That(Scene.capturellector.Inventory, Is.EqualTo(Scene.dequeBoxInventory));
		}
	}
}