using System;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(DequeBoxOwner))]
	public class DequeBoxDiajectorOpener : MonoBehaviour
	{
		public float primeDelay = 0.2f;
		
		private DequeBoxOwner dequeBoxOwner;
		
		private CollisionTrigger collisionTrigger;
		private Grabbable grabbable;
		
		private void Awake()
		{
			dequeBoxOwner = GetComponent<DequeBoxOwner>();
			
			dequeBoxOwner.dequeBoxEvents.onSet.AddListener(Bind);
			dequeBoxOwner.dequeBoxEvents.onUnset.AddListener(Unbind);
		}
		
		public void Bind(DequeBoxOwner dequeBoxOwner, DequeBox dequeBox)
		{
			if (grabbable)
				throw new InvalidOperationException($"{dequeBox} tried to bind to {this} but was already bound with {grabbable.GetComponent<DequeBox>()}");
			
			if (!dequeBox.TryGetComponent(out grabbable))
				throw new InvalidOperationException($"{dequeBox} tried to bind to {this} but had no grabbbable");
			
			collisionTrigger = dequeBox.gameObject.AddComponent<CollisionTrigger>();
			collisionTrigger.primeDelay = primeDelay;
			collisionTrigger.trigger.AddListener(OpenDiajector);
			
			grabbable.onUse.AddListener(CloseDiajector);
			grabbable.onDrop.AddListener(StartPrime);
			grabbable.onGrab.AddListener(CancelPrime);
		}
		
		public void Unbind(DequeBoxOwner dequeBoxOwner, DequeBox oldDequeBox, DequeBox newDequeBox)
		{
			if (!grabbable)
				throw new InvalidOperationException($"{oldDequeBox} tried to unbind from {this} but was already unbound");
			
			Destroy(collisionTrigger);
			collisionTrigger = null;
			
			grabbable.onUse.RemoveListener(CloseDiajector);
			grabbable.onDrop.RemoveListener(StartPrime);
			grabbable.onGrab.RemoveListener(CancelPrime);
			grabbable = null;
		}
		
		private void StartPrime(Grabber grabber, Grabbable grabbable) => collisionTrigger.StartPrime();
		private void CancelPrime(Grabber grabber, Grabbable grabbable) => collisionTrigger.CancelPrime();

		private void OpenDiajector()
		{
			Vector3 position = dequeBoxOwner.DequeBox.transform.position;
			Vector3 upDirection = dequeBoxOwner.DequeBox.GravitySum.upDirection;
			Vector3 groundDelta = Vector3.ProjectOnPlane(dequeBoxOwner.transform.position - position, upDirection);
			Quaternion rotation = Quaternion.LookRotation(groundDelta, upDirection);
			dequeBoxOwner.DequeOwner.diajector.StartAssembly(position, rotation);
		}
		private void CloseDiajector(Grabber grabber, Grabbable grabbable) => dequeBoxOwner.DequeOwner.diajector.StartDisassembly();
	}
}