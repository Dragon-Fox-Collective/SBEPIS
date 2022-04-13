using SBEPIS.Bits;
using SBEPIS.Interaction;
using System;
using UnityEngine;

namespace SBEPIS.Items
{
	[RequireComponent(typeof(CompoundRigidbody), typeof(Grabbable))]
	public class ItemBase : MonoBehaviour
	{
		public new CompoundRigidbody rigidbody { get; private set; }

		public BitSet bits;

		public Transform replaceObject;
		public Transform aeratedAttachmentPoint;

		private void Awake()
		{
			rigidbody = GetComponent<CompoundRigidbody>();
			if (transform.parent && transform.parent.GetComponentInParent<ItemBase>())
				DestroyForCombining();
		}

		public void DestroyForCombining()
		{
			Destroy(this);
			Destroy(GetComponent<Grabbable>());
			Destroy(rigidbody);
			Destroy(rigidbody.rigidbody);
			rigidbody.rigidbody.Disable();
		}
	}
}
