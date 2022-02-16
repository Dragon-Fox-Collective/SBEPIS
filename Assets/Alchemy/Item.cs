using SBEPIS.Interaction;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Alchemy
{
	[RequireComponent(typeof(Rigidbody))]
	public class Item : MonoBehaviour
	{
		public Itemkind itemkind;
		/// <summary>
		/// Rotation to use when taking a picture of this item
		/// </summary>
		public Quaternion captchaRotation = Quaternion.identity;
		/// <summary>
		/// Scale to use when taking a picture of this item
		/// </summary>
		public float captchaScale = 1f;
		public ItemEvent onPickUp, onHold, onDrop;
		public CaptchaEvent preCaptcha, postCaptcha;

		public ItemHolder holdingHolder { get; private set; }
		public bool canPickUp { get; set; }
		public new Rigidbody rigidbody { get; private set; }

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			canPickUp = true;
		}

		public void OnPickedUp(ItemHolder player)
		{
			holdingHolder = player;

			gameObject.SetLayerRecursively(LayerMask.NameToLayer("Held Item"));
			rigidbody.useGravity = false;

			onPickUp.Invoke(player);
		}

		public void OnHeld(ItemHolder player)
		{
			onHold.Invoke(player);
		}

		public void OnDropped(ItemHolder player)
		{
			holdingHolder = null;

			gameObject.SetLayerRecursively(LayerMask.NameToLayer("Default"));
			rigidbody.useGravity = true;

			onDrop.Invoke(player);
		}
	}

	[Serializable]
	public class ItemEvent : UnityEvent<ItemHolder> { }

	[Serializable]
	public class CaptchaEvent : UnityEvent { }
}