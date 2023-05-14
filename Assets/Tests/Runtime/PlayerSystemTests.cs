using System.Collections;
using NUnit.Framework;
using SBEPIS.Utils;
using SBEPIS.Tests.Scenes;
using UnityEngine.TestTools;

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
		
		[UnityTest]
		public IEnumerator StartingDeque_AttachesToHip()
		{
			yield return new WaitUntilOrTimeout(() => Scene.startingDequePlug.CoupledSocket == Scene.hipSocket, 3);
			Assert.That(Scene.startingDequePlug.CoupledSocket, Is.EqualTo(Scene.hipSocket));
		}
	}
}