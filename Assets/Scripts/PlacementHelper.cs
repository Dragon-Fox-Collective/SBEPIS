using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlacementHelper : MonoBehaviour
{
	public ItemType itemType;
	public Transform itemParent;
	public Animator animator;

	public new Collider collider { get; private set; }
	public new Item item { get; private set; }

	private void Awake()
	{
		collider = GetComponent<Collider>();
	}

	public void Adopt(Item item)
	{
		this.item = item;
		item.canPickUp = false;
		item.transform.SetParent(itemParent);
		item.transform.localPosition = Vector3.zero;
		item.transform.localRotation = Quaternion.identity;
		item.rigidbody.isKinematic = true;
		item.rigidbody.detectCollisions = false;
		animator.SetBool("Adopted", true);
	}
}
