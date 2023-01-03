using SBEPIS.Bits.Tags;
using SBEPIS.Items;
using System;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Bits
{
	[Serializable]
	public class TaggedBitSetFactory
	{
		public BitSet bits;
		public ItemBase itemBase;
		public Material material;

		public TaggedBitSet Make() => bits.With(new Tag[]
				{
					itemBase ? new BaseModelTag(itemBase) : null,
					material ? new MaterialTag(material) : null,
				}.Where(member => member != null).ToArray());
	}
}
