using NUnit.Framework;
using SBEPIS.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using OculusTouchController = Unity.XR.Oculus.Input.OculusTouchController;

namespace SBEPIS.Tests
{
	public class CapturllectVRTests : InputTestFixture, ICapturllectTest
	{
		private CapturllectVRScene scene;

		private OculusTouchController controller;
		private InputAction grabAction;
		private InputAction capturllectAction;
		private InputAction toggleDequeAction;

		public override void Setup()
		{
			base.Setup();

			scene = TestUtils.GetTestingPrefab<CapturllectVRScene>();

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
		public IEnumerator CapturllectSummonsDeque()
		{
			Press(controller.primaryButton);
			yield return new WaitForSeconds(0.5f);

			Assert.IsTrue(scene.dequeHolder.deque.gameObject.activeInHierarchy);
		}

		[UnityTest]
		public IEnumerator CapturllectDesummonsDeque()
		{
			Press(controller.primaryButton);
			yield return new WaitForSeconds(0.5f);

			Release(controller.gripPressed);
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
	}
}