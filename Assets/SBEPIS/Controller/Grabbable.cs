using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class Grabbable : MonoBehaviour
	{
		public List<GrabPoint> grabPoints = new();
		public ItemEvent onTouch = new();
		public ItemEvent onGrab = new();
		public ItemEvent onHoldUpdate = new();
		public ItemEvent onUse = new();
		public ItemEvent onDrop = new();
		public ItemEvent onStopTouch = new();
		
		public Grabber grabbingGrabber { get; private set; }
		public new Rigidbody rigidbody { get; private set; }
		public bool isBeingHeld { get; private set; }
		
		private const float DropCooldown = 0.2f;
		private float timeSinceLastDrop = DropCooldown;
		public bool canGrab => timeSinceLastDrop >= DropCooldown;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			timeSinceLastDrop += Time.fixedDeltaTime;
		}

		public void GetGrabbed(Grabber grabber)
		{
			grabbingGrabber = grabber;
			isBeingHeld = true;
			onGrab.Invoke(grabber, this);
		}

		public void HoldUpdate(Grabber grabber)
		{
			onHoldUpdate.Invoke(grabber, this);
		}

		public void GetDropped(Grabber grabber)
		{
			grabbingGrabber = null;
			isBeingHeld = false;
			timeSinceLastDrop = 0;
			onDrop.Invoke(grabber, this);
		}

		public void Drop()
		{
			if (isBeingHeld)
				grabbingGrabber.Drop();
		}
	}

	[Serializable]
	public class ItemEvent : UnityEvent<Grabber, Grabbable> { }
}