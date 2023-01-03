using SBEPIS.Bits;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(ItemBase))]
	public class Capturllectable : MonoBehaviour
	{
		public bool canCapturllect = true;
		public TaggedBitSet bits => itemBase.bits;

		private ItemBase itemBase;

		private void Awake()
		{
			itemBase = GetComponent<ItemBase>();
		}
	}
}
