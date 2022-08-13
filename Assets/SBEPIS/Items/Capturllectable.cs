using SBEPIS.Bits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Items
{
	[RequireComponent(typeof(ItemBase))]
	public class Capturllectable : MonoBehaviour
	{
		public bool isCapturllectable = true;
		public MemberedBitSet bits => itemBase.bits;

		private ItemBase itemBase;

		private void Awake()
		{
			itemBase = GetComponent<ItemBase>();
		}
	}
}
