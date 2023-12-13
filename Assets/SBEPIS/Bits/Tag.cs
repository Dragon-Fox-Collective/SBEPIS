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
		// ReSharper disable once NonReadonlyMemberInGetHashCode
		public override int GetHashCode() => key.GetHashCode();
		public static bool operator ==(Tag a, Tag b) => a?.GetType() == b?.GetType() && a?.key == b?.key;
		public static bool operator !=(Tag a, Tag b) => a?.GetType() != b?.GetType() || a?.key != b?.key;

		public override string ToString() => key;
	}
}