using System.Collections;
using System.Collections.Generic;
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
	public class DequeSystemTests : TestSceneSuite<DequeSystemScene>
	{
		public DequeSystemTests() : base(true) {}
		
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
			yield return new WaitForFixedUpdate();
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
		
		[UnityTest]
		public IEnumerator CapturingItem_WithClosedDeque_ActivatesCard() => UniTask.ToCoroutine(async () =>
		{
			Assert.That(!Scene.inventory.First().gameObject.activeSelf);
			await Scene.capturellector.CaptureAndGrabCard(Scene.capturellectable);
			Assert.That(Scene.inventory.First().gameObject.activeSelf);
		});
		
		[Test]
		public void OpeningDiajector_AfterInventoryMakesTargets_DoesntAddDuplicateCardTargets()
		{
			Scene.diajector.StartAssembly(Vector3.zero, Quaternion.identity);
			List<CardTarget> cardTargets = Scene.diajector.CurrentPage.CardTargets.ToList();
			Assert.That(cardTargets.Count, Is.EqualTo(cardTargets.Distinct().Count()));
		}
	}
}