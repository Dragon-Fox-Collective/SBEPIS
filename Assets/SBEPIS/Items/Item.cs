using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Items
{
	[RequireComponent(typeof(ItemModule))]
	public class Item : MonoBehaviour
	{
		[SerializeField, Self]
		private ItemModule module;
		public ItemModule Module => module;
		
		private void OnValidate() => this.ValidateRefs();
	}
}
