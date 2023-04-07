using NUnit.Framework;
using SBEPIS.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using OculusTouchController = Unity.XR.Oculus.Input.OculusTouchController;
using SBEPIS.Capturellection;
using SBEPIS.Bits;
using SBEPIS.Items;
using SBEPIS.Tests.Scenes;

namespace SBEPIS.Tests
{
	public class CapturellectTests : TestSceneSuite<CapturellectScene>
	{
		private OculusTouchController controller;
		private InputAction grabAction;
		private InputAction capturllectAction;
		private InputAction toggleDequeAction;

		public override void Setup()
		{
			base.Setup();
			
			controller = InputSystem.AddDevice<OculusTouchController>();
			grabAction = new InputAction("Grab", InputActionType.Button, "<XRController>/gripPressed");
			grabAction.performed += Scene.grabber.OnGrab;
			grabAction.canceled += Scene.grabber.OnGrab;
			grabAction.Enable();
			capturllectAction = new InputAction("Capturllect", InputActionType.Button, "<XRController>/primaryButton", "tap");
			capturllectAction.performed += Scene.dequeBoxOwner.OnToggleDeque;
			capturllectAction.canceled += Scene.dequeBoxOwner.OnToggleDeque;
			capturllectAction.Enable();
			toggleDequeAction = new InputAction("Toggle Deque", InputActionType.Button, "<XRController>/primaryButton", "hold");
			toggleDequeAction.performed += Scene.dequeBoxOwner.OnToggleDeque;
			toggleDequeAction.canceled += Scene.dequeBoxOwner.OnToggleDeque;
			toggleDequeAction.Enable();
		}
		
		[UnityTest]
		public IEnumerator CapturllectTogglesDeque()
		{
			bool wasActive = Scene.dequeBoxOwner.DequeBox.Deque.gameObject.activeSelf;
			
			Press(controller.primaryButton);
			yield return new WaitForSeconds(0.5f);
			
			Assert.That(Scene.dequeBoxOwner.DequeBox.Deque.gameObject.activeSelf, Is.Not.EqualTo(wasActive));
			
			Release(controller.primaryButton);
			yield return null;
			
			Press(controller.primaryButton);
			yield return new WaitForSeconds(0.5f);
			
			Assert.That(Scene.dequeBoxOwner.DequeBox.Deque.gameObject.activeSelf, Is.EqualTo(wasActive));
		}
		
		[UnityTest]
		public IEnumerator CapturllectCapturllectsItem_WhenHoldingItem()
		{
			Assert.IsNull(Scene.emptyCard.CapturedItem);
			Assert.IsTrue(Scene.obj.activeInHierarchy);

			Scene.grabber.transform.position = Scene.obj.transform.position;
			yield return new WaitForFixedUpdate();

			Press(controller.gripPressed);
			yield return null;

			Press(controller.primaryButton);
			yield return null;

			Release(controller.primaryButton);
			yield return null;

			Assert.AreEqual(Scene.obj, Scene.emptyCard.CapturedItem);
			Assert.IsFalse(Scene.obj.activeInHierarchy);
		}

		[UnityTest]
		public IEnumerator CapturllectFetchesItem_WhenHoldingCard()
		{
			Capturellectable capturedItem = Scene.fullCard.CapturedItem;
			Assert.IsNotNull(capturedItem);
			Assert.IsFalse(capturedItem.gameObject.activeInHierarchy);

			Scene.grabber.transform.position = Scene.fullCard.transform.position;
			yield return new WaitForFixedUpdate();

			Press(controller.gripPressed);
			yield return null;

			Press(controller.primaryButton);
			yield return null;

			Release(controller.primaryButton);
			yield return null;

			Assert.IsNull(Scene.fullCard.CapturedItem);
			Assert.IsTrue(capturedItem.gameObject.activeInHierarchy);
		}

		/*
		[UnityTest]
		public IEnumerator CapturllectEjectsItem_WhenCardIsFull()
		{
			// [] -> [item] -> [other item]

			Assert.AreEqual(0, scene.dequeHolder.deque.Count);

			Assert.AreEqual(1, scene.dequeHolder.deque.Count);
			Assert.IsNotNull(scene.dequeHolder.deque[0].capturedItem);
			GameObject firstItem = scene.dequeHolder.deque[0].capturedItem.gameObject;

			Assert.AreEqual(1, scene.dequeHolder.deque.Count);
			Assert.IsNotNull(scene.dequeHolder.deque[0].capturedItem);
			Assert.AreNotEqual(firstItem, scene.dequeHolder.deque[0].capturedItem);

			yield return null;
		}

		[UnityTest]
		public IEnumerator InsertingCardsFlushesEmptyCards()
		{
			// [] -> [empty] -> [empty card] -> [item, empty]

			Assert.AreEqual(0, scene.dequeHolder.deque.Count);

			Assert.AreEqual(1, scene.dequeHolder.deque.Count);
			Assert.IsNull(scene.dequeHolder.deque[0].capturedItem);

			Assert.AreEqual(2, scene.dequeHolder.deque.Count);
			Assert.IsNotNull(scene.dequeHolder.deque[0].capturedItem);
			Assert.IsNull(scene.dequeHolder.deque[1].capturedItem);

			yield return null;
		}

		[UnityTest]
		public IEnumerator InsertingCardsFlushesFullCards()
		{
			// [] -> [empty] -> [full card] -> [item, item]

			Assert.AreEqual(0, scene.dequeHolder.deque.Count);

			Assert.AreEqual(1, scene.dequeHolder.deque.Count);
			Assert.IsNull(scene.dequeHolder.deque[0].capturedItem);

			Assert.AreEqual(2, scene.dequeHolder.deque.Count);
			Assert.IsNotNull(scene.dequeHolder.deque[0].capturedItem);
			Assert.IsNotNull(scene.dequeHolder.deque[1].capturedItem);

			yield return null;
		}

		[UnityTest]
		public IEnumerator DequeDoesntAcceptEmptyCards()
		{
			// [] -> [empty] -> [empty]

			Assert.AreEqual(0, scene.dequeHolder.deque.Count);

			Assert.AreEqual(1, scene.dequeHolder.deque.Count);

			Assert.AreEqual(1, scene.dequeHolder.deque.Count);

			yield return null;
		}

		[UnityTest]
		public IEnumerator DequeDoesntAcceptPunchedCards()
		{
			// [] -> [empty] -> [empty]

			Assert.AreEqual(0, scene.dequeHolder.deque.Count);

			Assert.AreEqual(1, scene.dequeHolder.deque.Count);

			Assert.AreEqual(1, scene.dequeHolder.deque.Count);

			yield return null;
		}
		*/

		// TODO: this test
		/*
		[UnityTest]
		public IEnumerator CaptureCameraTakesPicturesOfCodes()
		{
			Assert.IsNotNull(CaptureCamera.GetCaptureCodeTexture(BitSet.FromCode("ssSS+/sS")));

			yield return null;
		}
		*/
	}
}