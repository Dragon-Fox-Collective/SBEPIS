using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WrightWay.SBEPIS.Modus;
using WrightWay.SBEPIS.Player;

namespace WrightWay.SBEPIS
{
	public class Sylladex : MonoBehaviour
	{
		private FetchModus modus = new StackModus();
		private List<Item> cards = new List<Item>(new Item[] { null, null, null, null, null });

		public void Captchalogue(Item item, SylladexOwner owner)
		{
			Eject(modus.Deposit(item, cards), owner);
			item.transform.SetParent(transform);
			item.gameObject.SetActive(false);
		}

		public Item Display()
		{
			return modus.Display(cards);
		}

		public void Retrieve()
		{
			modus.Retrieve(cards, false);
		}

		public void Eject(Item item, SylladexOwner owner)
		{
			if (!item)
				return;

			item.transform.SetParent(null);
			item.transform.position = owner.transform.position + owner.transform.forward;
			item.transform.rotation = owner.transform.rotation;
			item.rigidbody.velocity = owner.transform.forward * 6;
			item.rigidbody.angularVelocity = Vector3.zero;
			Item.SetLayerRecursively(item.gameObject, LayerMask.NameToLayer("Default")); // could be Held from parent
			item.gameObject.SetActive(true);
		}
	}
}