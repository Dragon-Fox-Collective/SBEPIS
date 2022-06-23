using SBEPIS.Bits;
using SBEPIS.Items;
using System;
using UnityEngine;

namespace SBEPIS.Thaumaturgy
{
	public class InstaThaumerger : MonoBehaviour
	{
		public MemberedBitSetFactory bits;

		private void Start()
		{
			Alchemize();
			Destroy(gameObject);
		}

		private void Alchemize()
		{
			ItemBase item = Thaumerger.Thaumerge(bits.Make(), ItemBaseManager.instance);
			item.transform.SetPositionAndRotation(transform.position, transform.rotation);
		}
	}
}
