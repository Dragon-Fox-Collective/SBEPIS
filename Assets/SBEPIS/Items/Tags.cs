using SBEPIS.Bits.Tags;
using SBEPIS.Items;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Bits
{
	[Serializable]
	public class TaggedBitSetFactory
	{
		[SerializeField] private BitSet bits;
		public BitSet Bits { get => bits; set => bits = value; }
		[FormerlySerializedAs("itemBase")]
		[SerializeField] private ItemModule itemModule;
		[SerializeField] private Material material;

		public TaggedBitSet Make() => bits + new Tag[]
				{
					itemModule ? new BaseModelTag(itemModule) : null,
					material ? new MaterialTag(material) : null,
				}.Where(member => member != null);
	}
}

namespace SBEPIS.Bits.Tags
{
	[Serializable]
	public class BaseModelTag : Tag
	{
		[FormerlySerializedAs("_itemBase")]
		[SerializeField]
		private ItemModule _itemModule;
		public ItemModule ItemModule => _itemModule;

		public BaseModelTag(ItemModule itemModule) : base($"baseModel:{itemModule.name}")
		{
			_itemModule = itemModule;
		}
	}

	[Serializable]
	public class MaterialTag : Tag
	{
		[SerializeField]
		private Material _material;
		public Material Material => _material;

		public MaterialTag(Material material) : base($"material:{material.name}")
		{
			_material = material;
		}
	}
}