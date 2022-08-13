using SBEPIS.Bits;
using UnityEngine;

namespace SBEPIS.Items
{
	[RequireComponent(typeof(ItemBase))]
	public class Capturllectable : MonoBehaviour
	{
		public bool canCapturllect = true;
		public MemberedBitSet bits => itemBase.bits;

		private ItemBase itemBase;

		private void Awake()
		{
			itemBase = GetComponent<ItemBase>();
		}
	}
}
