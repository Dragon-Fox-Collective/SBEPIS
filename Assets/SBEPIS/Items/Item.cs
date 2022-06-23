using SBEPIS.Interaction;
using UnityEngine;

namespace SBEPIS.Items
{
	[RequireComponent(typeof(CompoundRigidbody), typeof(Grabbable))]
	public class Item : ItemBase
	{
		public new CompoundRigidbody rigidbody { get; private set; }

		private void Awake()
		{
			rigidbody = GetComponent<CompoundRigidbody>();
		}
	}
}
