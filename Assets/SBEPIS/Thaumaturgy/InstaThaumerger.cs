using SBEPIS.Bits;
using SBEPIS.Items;
using System;
using UnityEngine;

namespace SBEPIS.Thaumaturgy
{
	public class InstaThaumerger : MonoBehaviour
	{
		public ItemBaseManager itemBases;
		public BitSet bits;

		private void Start()
		{
			Alchemize();
			Destroy(gameObject);
		}

		private void Alchemize()
		{
			ItemBase item = Thaumerger.Thaumerge(bits, itemBases);
			item.transform.SetPositionAndRotation(transform.position, transform.rotation);
		}
	}
}
