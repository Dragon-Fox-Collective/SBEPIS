using System;
using UnityEngine;
using UnityEngine.Events;
using WrightWay.SBEPIS.Player;

namespace WrightWay.SBEPIS
{
	[RequireComponent(typeof(Collider))]
	public class PlacementHelper : MonoBehaviour
	{
		public Itemkind itemkind;
		public Transform itemParent;
		public bool isAdopting = true;
		public PlacementEvent onPlace;
		public PlacementEvent onRemove;

		public new Collider collider { get; private set; }
		public Item item { get; private set; }

		private void Awake()
		{
			collider = GetComponent<Collider>();
		}

		public void Adopt(Item item)
		{
			this.item = item;
			item.transform.SetParent(itemParent);
			item.transform.localPosition = Vector3.zero;
			item.transform.localRotation = Quaternion.identity;
			Item.DisableRigidbody(item.rigidbody);
			isAdopting = false;
			collider.enabled = false;
			DisallowOrphan();
			onPlace.Invoke();
		}

		public void DisallowOrphan()
		{
			item.onPickUp.RemoveListener(Orphan);
			item.rigidbody.detectCollisions = false;
			item.canPickUp = false;
		}

		public void AllowOrphan()
		{
			item.onPickUp.AddListener(Orphan);
			item.rigidbody.detectCollisions = true;
			item.canPickUp = true;
		}

		public void Orphan(ItemHolder holder)
		{
			item.onPickUp.RemoveListener(Orphan);
			Item.EnableRigidbody(item.rigidbody);
			item.transform.SetParent(null);
			item = null;
			isAdopting = true;
			collider.enabled = true;
			onRemove.Invoke();
		}
	}

	[Serializable]
	public class PlacementEvent : UnityEvent { }
}