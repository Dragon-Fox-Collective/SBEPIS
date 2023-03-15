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
