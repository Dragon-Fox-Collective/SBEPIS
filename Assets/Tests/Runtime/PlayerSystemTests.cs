using System.Collections;
using NUnit.Framework;
using SBEPIS.Utils;
using SBEPIS.Tests.Scenes;
using UnityEngine;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class PlayerSystemTests : TestSceneSuite<PlayerSystemScene>
	{
		[Test]
		public void GrabbingDeque_ChangesCapturellectorInventory()
		{
			Scene.grabber.Rigidbody.MovePosition(Scene.dequeBoxGrabbable.transform.position);
			Scene.grabber.Grab(Scene.dequeBoxGrabbable);
			Assert.That(Scene.capturellector.Inventory, Is.EqualTo(Scene.dequeBoxInventory));
		}
	}
}