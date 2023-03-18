using SBEPIS.Capturllection;
using SBEPIS.Controller;
using SBEPIS.Physics;
using UnityEngine;

namespace SBEPIS.Items
{
	[RequireComponent(typeof(CompoundRigidbody), typeof(Grabbable), typeof(ItemModule))]
	[RequireComponent(typeof(GravitySum), typeof(Capturllectable))]
	public class Item : MonoBehaviour
	{
		public ItemModule itemModule { get; private set; }

		private void Awake()
		{
			itemModule = GetComponent<ItemModule>();
		}
	}
}
