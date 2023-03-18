using UnityEngine;

namespace SBEPIS.Items
{
	[RequireComponent(typeof(ItemModule))]
	public class Item : MonoBehaviour
	{
		public ItemModule module { get; private set; }

		private void Awake()
		{
			module = GetComponent<ItemModule>();
		}
	}
}
