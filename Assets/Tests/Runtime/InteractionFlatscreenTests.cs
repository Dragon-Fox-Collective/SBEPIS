using NUnit.Framework;
using SBEPIS.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class InteractionFlatscreenTests : InputTestFixture, IInteractionTest
	{
		private InteractionFlatscreenScene scene;

		private Mouse mouse;
		private InputAction grabAction;

		public override void Setup()
		{
			base.Setup();

			scene = TestUtils.GetTestingPrefab<InteractionFlatscreenScene>();

			mouse = InputSystem.AddDevice<Mouse>();
			grabAction = new InputAction("Grab", InputActionType.Button, "<Mouse>/leftButton");
			grabAction.performed += scene.grabber.OnGrab;
			grabAction.canceled += scene.grabber.OnGrab;
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
			Press(mouse.leftButton);
			yield return null;

			Assert.AreEqual(scene.grabbable, scene.grabber.heldGrabbable);
		}

		[UnityTest]
		public IEnumerator GrabLiftsGrabbables()
		{
			Vector3 oldPosition = scene.grabbable.transform.position;

			Press(mouse.leftButton);
			yield return null;

			scene.grabber.transform.position += Vector3.up;
			Assert.That(scene.grabbable.transform.position.y, Is.LessThanOrEqualTo(oldPosition.y));
			yield return new WaitForFixedUpdate();

			Assert.That(scene.grabbable.transform.position.y, Is.GreaterThan(oldPosition.y));
		}

		[UnityTest]
		public IEnumerator GrabbingSetsLayerToHeldItem()
		{
			Press(mouse.leftButton);
			yield return null;

			Assert.That(scene.grabbable.gameObject.IsOnLayer(LayerMask.GetMask("Held Item")));
		}

		[UnityTest]
		public IEnumerator UngrabbingSetsLayerToDefault()
		{
			Press(mouse.leftButton);
			yield return null;

			Release(mouse.leftButton);
			yield return null;

			Assert.That(scene.grabbable.gameObject.IsOnLayer(LayerMask.GetMask("Default")));
		}
	}
}
