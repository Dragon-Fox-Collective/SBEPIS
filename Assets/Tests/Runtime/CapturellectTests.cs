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
			grabAction.performed += scene.grabber.OnGrab;
			grabAction.canceled += scene.grabber.OnGrab;
			grabAction.Enable();
			capturllectAction = new InputAction("Capturllect", InputActionType.Button, "<XRController>/primaryButton", "tap");
			capturllectAction.performed += scene.dequeBoxOwner.OnToggleDeque;
			capturllectAction.canceled += scene.dequeBoxOwner.OnToggleDeque;
			capturllectAction.Enable();
			toggleDequeAction = new InputAction("Toggle Deque", InputActionType.Button, "<XRController>/primaryButton", "hold");
			toggleDequeAction.performed += scene.dequeBoxOwner.OnToggleDeque;
			toggleDequeAction.canceled += scene.dequeBoxOwner.OnToggleDeque;
			toggleDequeAction.Enable();
		}
		
		[UnityTest]
		public IEnumerator CapturllectTogglesDeque()
		{
			bool wasActive = scene.dequeBoxOwner.Deque.gameObject.activeSelf;
			
			Press(controller.primaryButton);
			yield return new WaitForSeconds(0.5f);
			
			Assert.That(scene.dequeBoxOwner.Deque.gameObject.activeSelf, Is.Not.EqualTo(wasActive));
			
			Release(controller.primaryButton);
			yield return null;
			
			Press(controller.primaryButton);
			yield return new WaitForSeconds(0.5f);
			
			Assert.That(scene.dequeBoxOwner.Deque.gameObject.activeSelf, Is.EqualTo(wasActive));
		}
		
		[UnityTest]
		public IEnumerator CapturllectCapturllectsItem_WhenHoldingItem()
		{
			Assert.IsNull(scene.emptyCard.CapturedItem);
			Assert.IsTrue(scene.obj.activeInHierarchy);

			scene.grabber.transform.position = scene.obj.transform.position;
			yield return new WaitForFixedUpdate();

			Press(controller.gripPressed);
			yield return null;

			Press(controller.primaryButton);
			yield return null;

			Release(controller.primaryButton);
			yield return null;

			Assert.AreEqual(scene.obj, scene.emptyCard.CapturedItem);
			Assert.IsFalse(scene.obj.activeInHierarchy);
		}

		[UnityTest]
		public IEnumerator CapturllectFetchesItem_WhenHoldingCard()
		{
			Capturellectable capturedItem = scene.fullCard.CapturedItem;
			Assert.IsNotNull(capturedItem);
			Assert.IsFalse(capturedItem.gameObject.activeInHierarchy);

			scene.grabber.transform.position = scene.fullCard.transform.position;
			yield return new WaitForFixedUpdate();

			Press(controller.gripPressed);
			yield return null;

			Press(controller.primaryButton);
			yield return null;

			Release(controller.primaryButton);
			yield return null;

			Assert.IsNull(scene.fullCard.CapturedItem);
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