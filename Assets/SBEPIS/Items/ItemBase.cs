using SBEPIS.Bits;
using SBEPIS.Interaction;
using System;
using UnityEngine;

namespace SBEPIS.Items
{
	[RequireComponent(typeof(CompoundRigidbody), typeof(Grabbable))]
	public class ItemBase : MonoBehaviour
	{
		public BitSet bits;

		public Rigidbody jointTarget;
		public Transform replaceObject;
		public Transform aeratedAttachmentPoint;

		private void Awake()
		{
			if (GetComponent<Rigidbody>())
				throw new InvalidOperationException("The GameObject holding the ItemBase component should not have a rigidbody.");
		}
	}
}
