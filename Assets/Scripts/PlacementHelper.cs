using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PlacementHelper : MonoBehaviour
{
	public ItemType itemType;
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
		item.rigidbody.isKinematic = true;
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

	public void Orphan()
	{
		item.onPickUp.RemoveListener(Orphan);
		item.rigidbody.isKinematic = false;
		item = null;
		isAdopting = true;
		collider.enabled = true;
		onRemove.Invoke();
	}
}

[Serializable]
public class PlacementEvent : UnityEvent { }