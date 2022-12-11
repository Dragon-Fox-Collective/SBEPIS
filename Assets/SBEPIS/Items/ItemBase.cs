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
			if (!GetComponentInParent<Item>())
				gameObject.AddComponent<Item>();
		}

		public override string ToString()
		{
			return $"{name}{{{bits}}}";
		}

		private void OnValidate()
		{
			madeBits = false;
		}
	}
}
