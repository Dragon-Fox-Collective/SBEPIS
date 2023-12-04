using SBEPIS.Bits;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Thaumergy
{
	public class InstaThaumerger : MonoBehaviour
	{
		public TaggedBitSetFactory itemToAlchemize;

		private void Start()
		{
			Item item = Alchemize();
			item.transform.Replace(transform);
		}

		private Item Alchemize()
		{
			Item item = Thaumerger.Thaumerge(itemToAlchemize.Make(), ItemModuleManager.Instance);
			item.gameObject.name = gameObject.name;
			return item;
		}
	}
}
