using System.Collections;
using System.Linq;
using NUnit.Framework;
using SBEPIS.Utils;
using SBEPIS.Tests.Scenes;
using UnityEngine;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class DequeSystemTests : TestSceneSuite<DequeSystemScene>
	{
		[Test]
		public void GrabbingDeque_GrabsDeque()
		{
			Scene.grabber.Rigidbody.MovePosition(Scene.dequeBoxGrabbable.transform.position);
			Scene.grabber.Grab(Scene.dequeBoxGrabbable);
			Assert.That(Scene.grabber.HeldGrabbable, Is.EqualTo(Scene.dequeBoxGrabbable));
		}
		
		[Test]
		public void DroppingDeque_StartsTrigger()
		{
			Scene.grabber.Rigidbody.MovePosition(Scene.dequeBoxGrabbable.transform.position);
			Scene.grabber.Grab(Scene.dequeBoxGrabbable);
			Scene.grabber.Rigidbody.MovePosition(Scene.dropPoint.position);
			Scene.grabber.Drop();
			Assert.That(Scene.dequeBoxTrigger.IsDelaying);
		}
		
		[UnityTest]
		public IEnumerator DroppingDeque_OpensDiajector()
		{
			Scene.grabber.transform.position = Scene.dequeBoxGrabbable.transform.position;
			Scene.grabber.Grab(Scene.dequeBoxGrabbable);
			Scene.grabber.transform.position = Scene.dropPoint.position;
			Scene.grabber.Drop();
			yield return new WaitUntilOrTimeout(() => Scene.diajector.IsOpen, 3);
			Assert.That(Scene.diajector.IsOpen);
		}
		
		[Test]
		public void NewCards_GetStored()
		{
			Assert.That(Scene.inventory.First().DequeElement.IsStored);
		}
		
		[Test]
		public void NewCards_HaveDeque()
		{
			Assert.That(Scene.inventory.First().DequeElement.Deque);
		}
		
		[Test]
		public void NewCards_HaveDiajector()
		{
			Scene.diajector.StartAssembly(null, Vector3.zero, Quaternion.identity);
			Assert.That(Scene.inventory.First().DequeElement.Diajector);
		}
	}
}