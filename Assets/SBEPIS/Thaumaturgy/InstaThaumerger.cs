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
			print(ItemBaseManager.instance.itemBases.ToDelimString());
			Item item = Thaumerger.Thaumerge(itemToAlchemize.Make(), ItemBaseManager.instance);
			item.gameObject.name = gameObject.name;
			print($"Made {item} with {item.itemBase.bits}");
			item.transform.SetPositionAndRotation(transform.position, transform.rotation);
			print($"Set position of {item} to {transform.position}");
		}
	}
}
