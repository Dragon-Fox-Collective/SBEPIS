using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class Grabbable : MonoBehaviour
	{
		public List<GrabPoint> grabPoints = new();
		public ItemEvent onTouch = new(), onGrab = new(), onHoldUpdate = new(), onUse = new(), onDrop = new(), onStopTouch = new();

		public Grabber grabbingGrabber { get; private set; }
		public bool canGrab { get; set; }
		public new Rigidbody rigidbody { get; private set; }
		public bool isBeingHeld { get; private set; }

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			canGrab = true;
		}

		public void Grab(Grabber player)
		{
			grabbingGrabber = player;
			isBeingHeld = true;
			onGrab.Invoke(player, this);
		}

		public void HoldUpdate(Grabber player)
		{
			onHoldUpdate.Invoke(player, this);
		}

		public void Drop(Grabber player)
		{
			grabbingGrabber = null;
			isBeingHeld = false;
			onDrop.Invoke(player, this);
		}
	}

	[Serializable]
	public class ItemEvent : UnityEvent<Grabber, Grabbable> { }
}