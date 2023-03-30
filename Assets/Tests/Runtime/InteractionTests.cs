using NUnit.Framework;
using SBEPIS.Utils;
using System.Collections;
using SBEPIS.Tests.Scenes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class InteractionTests : TestSceneSuite<InteractionScene>
	{
		private Mouse mouse;
		private InputAction grabAction;

		public override void Setup()
		{
			base.Setup();

			mouse = InputSystem.AddDevice<Mouse>();
			grabAction = new InputAction("Grab", InputActionType.Button, "<Mouse>/leftButton");
			grabAction.performed += Scene.grabber.OnGrab;
			grabAction.canceled += Scene.grabber.OnGrab;
			grabAction.Enable();
		}

		[UnityTest]
		public IEnumerator GrabGrabsGrabbables()
		{
			Scene.grabber.transform.position = Scene.grabbable.transform.position;
			yield return new WaitForFixedUpdate();

			Press(mouse.leftButton);
			yield return null;

			Assert.AreEqual(Scene.grabbable, Scene.grabber.heldGrabbable);
		}

		[UnityTest]
		public IEnumerator GrabLiftsGrabbables()
		{
			Vector3 oldPosition = Scene.grabbable.transform.position;

			Scene.grabber.transform.position = oldPosition;
			yield return new WaitForFixedUpdate();

			Press(mouse.leftButton);
			yield return null;

			Scene.grabber.transform.position += Vector3.up;
			Assert.That(Scene.grabbable.transform.position.y, Is.LessThanOrEqualTo(oldPosition.y));
			yield return new WaitForFixedUpdate();

			Assert.That(Scene.grabbable.transform.position.y, Is.GreaterThan(oldPosition.y));
		}

		[UnityTest]
		public IEnumerator ClickingActivatesPhysicsButton()
		{
			Scene.grabber.transform.LookAt(Scene.buttonMaterialChanger.transform, Vector3.up);

			Press(mouse.leftButton);
			yield return null;

			Release(mouse.leftButton);
			yield return new WaitForSeconds(0.1f);

			Assert.That(Scene.buttonMaterialChanger.renderer.material, Is.EqualTo(Scene.buttonMaterialChanger.newMaterial));
		}

		[UnityTest]
		public IEnumerator PhysicsActivatePhysicsButton()
		{
			Scene.grabbable.transform.position = Scene.buttonMaterialChanger.transform.position + Vector3.up;
			yield return new WaitForSeconds(0.5f);

			Assert.That(Scene.buttonMaterialChanger.renderer.material, Is.EqualTo(Scene.buttonMaterialChanger.newMaterial));
		}

		[UnityTest]
		public IEnumerator ClickingActivatesPhysicsLever()
		{
			Scene.grabber.transform.LookAt(Scene.leverMaterialChanger.transform, Vector3.up);

			Press(mouse.leftButton);
			yield return null;

			Release(mouse.leftButton);
			yield return new WaitForSeconds(0.1f);

			Assert.That(Scene.leverMaterialChanger.renderer.material, Is.EqualTo(Scene.leverMaterialChanger.newMaterial));
		}

		[UnityTest]
		public IEnumerator PhysicsActivatePhysicsLever()
		{
			Scene.grabbable.transform.position = Scene.leverMaterialChanger.transform.position + Vector3.up;
			yield return new WaitForSeconds(0.5f);

			Assert.That(Scene.leverMaterialChanger.renderer.material, Is.EqualTo(Scene.leverMaterialChanger.newMaterial));
		}
	}
}
