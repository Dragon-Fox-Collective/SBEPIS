using SBEPIS.Bits;
using SBEPIS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
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
			item.gameObject.name = gameObject.name;
			item.transform.SetPositionAndRotation(transform.position, transform.rotation);
		}
	}
}
