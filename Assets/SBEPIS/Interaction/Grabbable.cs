using SBEPIS.Interaction;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Rigidbody))]
	public class Grabbable : MonoBehaviour
	{
		public ItemEvent onGrab, onHoldUpdate, onDrop;

		public Grabber holdingHolder { get; private set; }
		public bool canGrab { get; set; }
		public new Rigidbody rigidbody { get; private set; }

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			canGrab = true;
		}

		public void Grab(Grabber player)
		{
			holdingHolder = player;

			gameObject.SetLayerRecursively(LayerMask.NameToLayer("Held Item"));
			rigidbody.useGravity = false;

			onGrab.Invoke(player);
		}

		public void HoldUpdate(Grabber player)
		{
			onHoldUpdate.Invoke(player);
		}

		public void Drop(Grabber player)
		{
			holdingHolder = null;

			gameObject.SetLayerRecursively(LayerMask.NameToLayer("Default"));
			rigidbody.useGravity = true;

			onDrop.Invoke(player);
		}
	}

	[Serializable]
	public class ItemEvent : UnityEvent<Grabber> { }
}