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

		public ItemGrabber holdingHolder { get; private set; }
		public bool canGrab { get; set; }
		public new Rigidbody rigidbody { get; private set; }

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			canGrab = true;
		}

		public void Grab(ItemGrabber player)
		{
			holdingHolder = player;

			gameObject.SetLayerRecursively(LayerMask.NameToLayer("Held Item"));
			rigidbody.useGravity = false;

			onPickUp.Invoke(player);
		}

		public void HoldUpdate(ItemGrabber player)
		{
			onHoldUpdate.Invoke(player);
		}

		public void Drop(ItemGrabber player)
		{
			holdingHolder = null;

			gameObject.SetLayerRecursively(LayerMask.NameToLayer("Default"));
			rigidbody.useGravity = true;

			onDrop.Invoke(player);
		}
	}

	[Serializable]
	public class ItemEvent : UnityEvent<ItemGrabber> { }
}