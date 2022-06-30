using SBEPIS.Bits;
using SBEPIS.Interaction;
using System;
using UnityEngine;

namespace SBEPIS.Items
{
	public class ItemBase : MonoBehaviour
	{
		public MemberedBitSetFactory baseBits;
		[SerializeField]
		[HideInInspector]
		private MemberedBitSet _bits;
		private bool madeBits;
		public MemberedBitSet bits {
			get
			{
				if (!madeBits)
				{
					if (baseBits is not null)
						_bits = baseBits.Make();
					madeBits = true;
				}
				return _bits;
			}
			set => _bits = value;
		}

		public Transform replaceObject;
		public Transform aeratedAttachmentPoint;

		private void Start()
		{
			BecomeItem();
		}

		public bool BecomeItem()
		{
			if (!GetComponentInParent<Item>())
			{
				gameObject.AddComponent<Item>();
				return true;
			}
			else
				return false;
		}

		public override string ToString()
		{
			return $"{base.ToString()}{{{bits}}}";
		}
	}
}
