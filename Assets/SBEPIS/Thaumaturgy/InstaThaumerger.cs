using SBEPIS.Bits;
using SBEPIS.Items;
using System;
using UnityEngine;

namespace SBEPIS.Thaumaturgy
{
	public class InstaThaumerger : MonoBehaviour
	{
		public MemberedBitSetFactory itemToAlchemize;

		private void Start()
		{
			Alchemize();
			Destroy(gameObject);
		}

		private void Alchemize()
		{
			Item item = Thaumerger.Thaumerge(itemToAlchemize.Make(), ItemBaseManager.instance);
			item.transform.SetPositionAndRotation(transform.position, transform.rotation);
		}
	}
}
