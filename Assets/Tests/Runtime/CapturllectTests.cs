using NUnit.Framework;
using SBEPIS.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using OculusTouchController = Unity.XR.Oculus.Input.OculusTouchController;

namespace SBEPIS.Tests
{
	public class CapturllectTests : InputTestFixture
	{
		private CapturllectScene scene;

		private OculusTouchController controller;
		private InputAction grabAction;
		private InputAction capturllectAction;
		private InputAction toggleDequeAction;

		public override void Setup()
		{
			base.Setup();

			scene = TestUtils.GetTestingPrefab<CapturllectScene>();

			controller = InputSystem.AddDevice<OculusTouchController>();
			grabAction = new InputAction("Grab", InputActionType.Button, "<XRController>/gripPressed");
			grabAction.performed += scene.grabber.OnGrab;
			grabAction.canceled += scene.grabber.OnGrab;
			grabAction.Enable();
			capturllectAction = new InputAction("Capturllect", InputActionType.Button, "<XRController>/primaryButton", "tap");
			capturllectAction.performed += scene.dequeHolder.OnToggleDeque;
			capturllectAction.canceled += scene.dequeHolder.OnToggleDeque;
			capturllectAction.Enable();
			toggleDequeAction = new InputAction("Toggle Deque", InputActionType.Button, "<XRController>/primaryButton", "hold");
			toggleDequeAction.performed += scene.dequeHolder.OnToggleDeque;
			toggleDequeAction.canceled += scene.dequeHolder.OnToggleDeque;
			toggleDequeAction.Enable();
		}

		public override void TearDown()
		{
			base.TearDown();

			Object.Destroy(scene.gameObject);
		}

		[UnityTest]
		public IEnumerator CapturllectTogglesDeque()
		{
			Press(controller.primaryButton);
			yield return new WaitForSeconds(0.5f);

			Assert.IsTrue(scene.dequeHolder.deque.gameObject.activeInHierarchy);

			Release(controller.primaryButton);
			yield return null;

			Press(controller.primaryButton);
			yield return new WaitForSeconds(0.5f);

			Assert.IsFalse(scene.dequeHolder.deque.gameObject.activeInHierarchy);
		}

		[UnityTest]
		public IEnumerator CapturllectCapturllectsItem_WhenHoldingItem()
		{
			scene.grabber.transform.position = scene.item.transform.position;
			yield return new WaitForFixedUpdate();

			Press(controller.gripPressed);
			yield return null;

			Press(controller.primaryButton);
			yield return null;

			Release(controller.primaryButton);
			yield return null;

			Assert.AreEqual(scene.item, scene.emptyCard.capturedItem);
			Assert.IsFalse(scene.item.gameObject.activeInHierarchy);
		}

		[UnityTest]
		public IEnumerator CapturllectFetchesItem_WhenHoldingCard()
		{
			scene.grabber.transform.position = scene.fullCard.transform.position;
			yield return new WaitForFixedUpdate();

			Press(controller.gripPressed);
			yield return null;

			Press(controller.primaryButton);
			yield return null;

			Release(controller.primaryButton);
			yield return null;

			Assert.IsNull(scene.fullCard.capturedItem);
			Assert.IsTrue(scene.item.gameObject.activeInHierarchy);
		}

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
	}
}