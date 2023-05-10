using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Items
{
	[RequireComponent(typeof(ItemModule))]
	public class Item : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private ItemModule module;
		public ItemModule Module => module;
	}
}
