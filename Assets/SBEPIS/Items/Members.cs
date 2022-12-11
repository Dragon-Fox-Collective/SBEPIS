using SBEPIS.Items;
using System;
using UnityEngine;

namespace SBEPIS.Bits
{
	[Serializable]
	public class Member
	{
		[HideInInspector]
		[SerializeField]
		private string key;

		public Member(string key)
		{
			this.key = key;
		}

		public override bool Equals(object obj) => obj is Member other && this == other;
		public override int GetHashCode() => key.GetHashCode();
		public static bool operator ==(Member a, Member b) => a?.GetType() == b?.GetType() && a?.key == b?.key;
		public static bool operator !=(Member a, Member b) => a?.GetType() != b?.GetType() || a?.key != b?.key;

		public override string ToString() => "Member{" + key + "}";
	}
}

namespace SBEPIS.Bits.Members
{
	[Serializable]
	public class BaseModelMember : Member
	{
		[SerializeField]
		private ItemBase _itemBase;
		public ItemBase itemBase => _itemBase;

		public BaseModelMember(ItemBase itemBase) : base($"baseModel:{itemBase}")
		{
			_itemBase = itemBase;
		}
	}

	[Serializable]
	public class MaterialMember : Member
	{
		[SerializeField]
		private Material _material;
		public Material material => _material;

		public MaterialMember(Material material) : base($"material:{material.name}")
		{
			_material = material;
		}
	}
}