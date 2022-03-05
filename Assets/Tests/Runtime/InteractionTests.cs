using NUnit.Framework;
using SBEPIS.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class InteractionTests : InputTestFixture
	{
		private InteractionScene scene;

		private Mouse mouse;
		private InputAction grabAction;

		public override void Setup()
		{
			base.Setup();

			scene = TestUtils.GetTestingPrefab<InteractionScene>();

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
			scene.grabber.transform.position = scene.grabbable.transform.position;
			yield return new WaitForFixedUpdate();

			Press(mouse.leftButton);
			yield return null;

			Assert.AreEqual(scene.grabbable, scene.grabber.heldGrabbable);
		}

		[UnityTest]
		public IEnumerator GrabLiftsGrabbables()
		{
			Vector3 oldPosition = scene.grabbable.transform.position;

			scene.grabber.transform.position = oldPosition;
			yield return new WaitForFixedUpdate();

			Press(mouse.leftButton);
			yield return null;

			scene.grabber.transform.position += Vector3.up;
			Assert.That(scene.grabbable.transform.position.y, Is.LessThanOrEqualTo(oldPosition.y));
			yield return new WaitForFixedUpdate();

			Assert.That(scene.grabbable.transform.position.y, Is.GreaterThan(oldPosition.y));
		}

		[UnityTest]
		public IEnumerator ClickingActivatesPhysicsButton()
		{
			scene.grabber.transform.LookAt(scene.buttonMaterialChanger.transform, Vector3.up);

			Press(mouse.leftButton);
			yield return null;

			Release(mouse.leftButton);
			yield return new WaitForSeconds(0.1f);

			Assert.That(scene.buttonMaterialChanger.renderer.material, Is.EqualTo(scene.buttonMaterialChanger.newMaterial));
		}

		[UnityTest]
		public IEnumerator PhysicsActivatePhysicsButton()
		{
			scene.grabbable.transform.position = scene.buttonMaterialChanger.transform.position + Vector3.up;
			yield return new WaitForSeconds(0.5f);

			Assert.That(scene.buttonMaterialChanger.renderer.material, Is.EqualTo(scene.buttonMaterialChanger.newMaterial));
		}

		[UnityTest]
		public IEnumerator ClickingActivatesPhysicsLever()
		{
			scene.grabber.transform.LookAt(scene.leverMaterialChanger.transform, Vector3.up);

			Press(mouse.leftButton);
			yield return null;

			Release(mouse.leftButton);
			yield return new WaitForSeconds(0.1f);

			Assert.That(scene.leverMaterialChanger.renderer.material, Is.EqualTo(scene.leverMaterialChanger.newMaterial));
		}

		[UnityTest]
		public IEnumerator PhysicsActivatePhysicsLever()
		{
			scene.grabbable.transform.position = scene.leverMaterialChanger.transform.position + Vector3.up;
			yield return new WaitForSeconds(0.5f);

			Assert.That(scene.leverMaterialChanger.renderer.material, Is.EqualTo(scene.leverMaterialChanger.newMaterial));
		}
	}
}
