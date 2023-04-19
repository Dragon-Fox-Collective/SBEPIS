using SBEPIS.Bits;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Items
{
	public class ItemModule : MonoBehaviour
	{
		public TaggedBitSetFactory baseBits;
		private TaggedBitSet bits;
		private bool madeBits;
		public TaggedBitSet Bits {
			get
			{
				if (!madeBits)
				{
					if (baseBits is not null)
						bits = baseBits.Make();
					madeBits = true;
				}
				return bits;
			}
			set => bits = value;
		}
		
		public Transform replaceObject;
		public Transform aeratedAttachmentPoint;
		
		private void Start()
		{
			BecomeItem();
		}
		
		public bool BecomeItem()
		{
			if (GetComponentInParent<Item>())
				return false;
			
			Item item = Instantiate(ItemModuleManager.instance.itemBase, transform.parent);
			item.transform.SetPositionAndRotation(transform.position, transform.rotation);
			transform.SetParent(item.transform, true);
			item.name = name;
			return true;
		}
		
		public override string ToString()
		{
			return $"{name}{{{Bits}}}";
		}
		
		private void OnValidate()
		{
			madeBits = false;
		}
	}
}
