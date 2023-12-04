using SBEPIS.Bits;
using UnityEngine;

namespace SBEPIS.Items
{
	public class ItemModule : MonoBehaviour
	{
		[SerializeField] private TaggedBitSetFactory baseBits;
		private TaggedBitSet bits;
		private bool madeBits;
		public TaggedBitSet Bits {
			get
			{
				if (!madeBits)
				{
					if (baseBits is not null)
						Bits = baseBits.Make();
					madeBits = true;
				}
				return bits;
			}
			set
			{
				bits = value;
				baseBits.Bits = value.Bits; // Just for the inspector
				madeBits = true;
			}
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
			
			Item item = Instantiate(ItemModuleManager.Instance.itemBase, transform.parent);
			item.transform.SetPositionAndRotation(transform.position, transform.rotation);
			transform.SetParent(item.transform, true);
			item.name = name;
			item.Module.Bits = Bits;
			return true;
		}
		
		public override string ToString()
		{
			return name + Bits;
		}
		
		private void OnValidate()
		{
			madeBits = false;
		}
	}
}
