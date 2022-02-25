using System.Collections;
using NUnit.Framework;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;
using OculusTouchController = Unity.XR.Oculus.Input.OculusTouchController;

namespace SBEPIS.Tests.PlayMode
{
	public class InteractionVRTests : InputTestFixture
	{
		private InteractionVRScene scene;

		private OculusTouchController controller;
		private InputAction grabAction;

		public override void Setup()
		{
			base.Setup();

			scene = TestUtils.GetTestingPrefab<InteractionVRScene>();

			controller = InputSystem.AddDevice<OculusTouchController>();
			grabAction = new InputAction("Grab", InputActionType.Button, "<XRController>/gripPressed");
			grabAction.performed += scene.grabberVR.OnGrab;
			grabAction.Enable();
		}

		public override void TearDown()
		{
			base.TearDown();

			Object.Destroy(scene.gameObject);
		}

		[UnityTest]
		public IEnumerator GrabGrabsGrabbables()
		{
			scene.grabberVR.transform.position = scene.grabbable.transform.position;
			yield return new WaitForFixedUpdate();

			Press(controller.gripPressed);
			yield return null;

			Assert.AreEqual(scene.grabbable, scene.grabberVR.heldGrabbable);
		}

		[UnityTest]
		public IEnumerator GrabLiftsGrabbables()
		{
			Vector3 oldPosition = scene.grabbable.transform.position;
			scene.grabberVR.transform.position = oldPosition;
			yield return new WaitForFixedUpdate();

			Press(controller.gripPressed);
			yield return null;

			scene.grabberVR.transform.position += Vector3.up;
			Assert.That(scene.grabbable.transform.position.y, Is.LessThanOrEqualTo(oldPosition.y));
			yield return new WaitForFixedUpdate();

			Assert.That(scene.grabbable.transform.position.y, Is.GreaterThan(oldPosition.y));
		}
	}
}