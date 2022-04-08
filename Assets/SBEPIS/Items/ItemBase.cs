using SBEPIS.Bits;
using System;
using UnityEngine;

namespace SBEPIS.Items
{
	public class ItemBase : MonoBehaviour
	{
		public BitSet bits;

		public Rigidbody jointTarget;
		public Rigidbody replaceObject;
		public Transform aeratedAttachmentPoint;

		private void Awake()
		{
			if (GetComponent<Rigidbody>())
				throw new InvalidOperationException("The GameObject holding the ItemBase component should not have a rigidbody.");
		}
	}
}
