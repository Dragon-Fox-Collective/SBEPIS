using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SBEPIS.Capturellection;
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
			Scene.grabber.GrabManually(Scene.dequeBoxGrabbable);
			Assert.That(Scene.grabber.HeldGrabbable, Is.EqualTo(Scene.dequeBoxGrabbable));
		}
		
		[Test]
		public void DroppingDeque_StartsTrigger()
		{
			Scene.grabber.Rigidbody.MovePosition(Scene.dequeBoxGrabbable.transform.position);
			Scene.grabber.GrabManually(Scene.dequeBoxGrabbable);
			Scene.grabber.Rigidbody.MovePosition(Scene.dropPoint.position);
			Scene.grabber.DropManually();
			Assert.That(Scene.dequeBoxTrigger.IsDelaying);
		}
		
		[UnityTest]
		public IEnumerator DroppingDeque_OpensDiajector()
		{
			Scene.grabber.transform.position = Scene.dequeBoxGrabbable.transform.position;
			Scene.grabber.GrabManually(Scene.dequeBoxGrabbable);
			Scene.grabber.transform.position = Scene.dropPoint.position;
			yield return 0;
			Scene.grabber.DropManually();
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
		public void NewCards_HavePage()
		{
			Scene.diajector.StartAssembly(Vector3.zero, Quaternion.identity);
			Assert.That(Scene.inventory.First().DequeElement.Page);
		}

		[UnityTest]
		public IEnumerator OpeningDiajector_LaysOutCards()
		{
			Assert.That(Scene.cardTargets.Count, Is.GreaterThan(0));
			
			Scene.diajector.StartAssembly(Vector3.zero, Quaternion.identity);
			
			Dictionary<CardTarget, bool> reached = new();
			foreach (CardTarget cardTarget in Scene.cardTargets)
			{
				reached[cardTarget] = false;
				cardTarget.Card.Animator.AddListenerOnMoveTo(cardTarget.LerpTarget, _ => reached[cardTarget] = true);
			}
			Assert.That(reached.Any(pair => pair.Value), Is.False);
			yield return new WaitUntilOrTimeout(() => reached.All(pair => pair.Value), 3);
			Assert.That(reached.All(pair => pair.Value), Is.True);
		}
	}
}