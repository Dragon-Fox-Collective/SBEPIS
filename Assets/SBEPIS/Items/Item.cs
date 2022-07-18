using SBEPIS.Interaction.Controller;
using SBEPIS.Interaction.Physics;
using UnityEngine;

namespace SBEPIS.Items
{
	[RequireComponent(typeof(CompoundRigidbody), typeof(Grabbable), typeof(ItemBase))]
	public class Item : MonoBehaviour
	{
		public new CompoundRigidbody rigidbody { get; private set; }
		public ItemBase itemBase { get; private set; }

		private void Awake()
		{
			rigidbody = GetComponent<CompoundRigidbody>();
			itemBase = GetComponent<ItemBase>();
		}
	}
}
