using SBEPIS.Items;
using System;
using UnityEngine;

namespace SBEPIS.Bits
{
	[Serializable]
	public class Tag
	{
		[HideInInspector]
		[SerializeField]
		private string key;

		public Tag(string key)
		{
			this.key = key;
		}

		public override bool Equals(object obj) => obj is Tag other && this == other;
		public override int GetHashCode() => key.GetHashCode();
		public static bool operator ==(Tag a, Tag b) => a?.GetType() == b?.GetType() && a?.key == b?.key;
		public static bool operator !=(Tag a, Tag b) => a?.GetType() != b?.GetType() || a?.key != b?.key;

		public override string ToString() => "Member{" + key + "}";
	}
}

namespace SBEPIS.Bits.Tags
{
	[Serializable]
	public class BaseModelTag : Tag
	{
		[SerializeField]
		private ItemBase _itemBase;
		public ItemBase itemBase => _itemBase;

		public BaseModelTag(ItemBase itemBase) : base($"baseModel:{itemBase.name}")
		{
			_itemBase = itemBase;
		}
	}

	[Serializable]
	public class MaterialTag : Tag
	{
		[SerializeField]
		private Material _material;
		public Material material => _material;

		public MaterialTag(Material material) : base($"material:{material.name}")
		{
			_material = material;
		}
	}
}