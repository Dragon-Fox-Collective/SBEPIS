using System.Collections;
using NUnit.Framework;
using SBEPIS.Utils;
using SBEPIS.Tests.Scenes;
using UnityEngine;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class DiajectorTests : TestSceneSuite<DiajectorScene>
	{
		[Test]
		public void OpeningDiajector_CreatesCards()
		{
			Scene.diajector1.StartAssembly(Vector3.zero, Quaternion.identity);
			Assert.That(Scene.cardTarget.Card, Is.Not.Null);
		}
		
		[Test]
		public void ForceOpeningDiajector_CreatesCards()
		{
			Scene.diajector1.ForceOpen();
			Assert.That(Scene.cardTarget.Card, Is.Not.Null);
		}
		
		[Test]
		public void OpeningDiajector_OpensDiajector()
		{
			Scene.diajector1.StartAssembly(Vector3.zero, Quaternion.identity);
			Assert.That(Scene.diajector1.IsOpen);
		}
		
		[Test]
		public void ClosingDiajector_ClosesDiajector()
		{
			Scene.diajector1.ForceOpen();
			Scene.diajector1.StartDisassembly();
			Assert.That(Scene.diajector1.IsOpen, Is.False);
		}
		
		[UnityTest]
		public IEnumerator OpeningDiajector_GetsCardToBoard()
		{
			Scene.diajector1.StartAssembly(Vector3.zero, Quaternion.identity);
			
			bool reached = false;
			Scene.cardTarget.Card.Animator.AddListenerOnMoveTo(Scene.endCardTarget, _ => reached = true);
			Assert.That(reached, Is.False);
			yield return new WaitUntilOrTimeout(() => reached, 3);
			Assert.That(reached);
		}
		
		[UnityTest]
		public IEnumerator ClosingDiajector_GetsCardToDeque()
		{
			Scene.diajector1.ForceOpen();
			yield return 0;
			Scene.diajector1.StartDisassembly();
			
			bool reached = false;
			Scene.cardTarget.Card.Animator.AddListenerOnMoveTo(Scene.startCardTarget, _ => reached = true);
			Assert.That(reached, Is.False);
			yield return new WaitUntilOrTimeout(() => reached, 10);
			Assert.That(reached);
		}
		
		[Test]
		public void ForceOpeningDiajector_OpensDiajector()
		{
			Scene.diajector1.ForceOpen();
			Assert.That(Scene.diajector1.IsOpen);
		}
		
		[Test]
		public void ForceClosingDiajector_ClosesDiajector()
		{
			Scene.diajector1.ForceOpen();
			Scene.diajector1.ForceClose();
			Assert.That(Scene.diajector1.IsOpen, Is.False);
		}
		
		[Test]
		public void ForceRestartingDiajector_OpensDiajector()
		{
			Scene.diajector1.ForceOpen();
			Scene.diajector1.ForceRestart();
			Assert.That(Scene.diajector1.IsOpen);
		}
		
		[UnityTest]
		public IEnumerator ForceOpeningDiajector_GetsCardToBoard()
		{
			Scene.diajector1Page.CreateCardsIfNeeded();
			
			bool reached = false;
			Scene.cardTarget.Card.Animator.AddListenerOnMoveTo(Scene.endCardTarget, _ => reached = true);
			Scene.diajector1.ForceOpen();
			yield return 0;
			Assert.That(reached);
		}
		
		[UnityTest]
		public IEnumerator ForceClosingDiajector_GetsCardToDeque()
		{
			Scene.diajector1Page.CreateCardsIfNeeded();
			Scene.diajector1.ForceOpen();
			yield return 0;
			
			bool reached = false;
			Scene.cardTarget.Card.Animator.AddListenerOnMoveTo(Scene.startCardTarget, _ => reached = true);
			Scene.diajector1.ForceClose();
			yield return 0;
			Assert.That(reached);
		}
	}
}