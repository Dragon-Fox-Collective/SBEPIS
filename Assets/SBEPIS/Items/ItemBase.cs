using SBEPIS.Bits;
using UnityEngine;

namespace SBEPIS.Items
{
	public class ItemBase : MonoBehaviour
	{
		public TaggedBitSetFactory baseBits;
		[SerializeField]
		[HideInInspector]
		private TaggedBitSet _bits;
		private bool madeBits;
		public TaggedBitSet bits {
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
