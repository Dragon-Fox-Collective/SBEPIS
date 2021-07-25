using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlacementHelper : MonoBehaviour
{
	public ItemType itemType;

	private new Collider collider;

	private void Awake()
	{
		collider = GetComponent<Collider>();
	}
}
