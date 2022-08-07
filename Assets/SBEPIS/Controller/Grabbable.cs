using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class Grabbable : MonoBehaviour
	{
		public ItemEvent onTouch = new(), onGrab = new(), onHoldUpdate = new(), onDrop = new(), onStopTouch = new();

		public Grabber grabbingGrabber { get; private set; }
		public bool canGrab { get; set; }
		public new Rigidbody rigidbody { get; private set; }

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			canGrab = true;
		}

		public void Grab(Grabber player)
		{
			grabbingGrabber = player;

			onGrab.Invoke(player, this);
		}

		public void HoldUpdate(Grabber player)
		{
			onHoldUpdate.Invoke(player, this);
		}

		public void Drop(Grabber player)
		{
			grabbingGrabber = null;

			onDrop.Invoke(player, this);
		}
	}

	[Serializable]
	public class ItemEvent : UnityEvent<Grabber, Grabbable> { }
}