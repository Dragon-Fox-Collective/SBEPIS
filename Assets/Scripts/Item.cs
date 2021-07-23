using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
	public ItemType itemType; // FIXME: Double mobius referencial
	private new Rigidbody rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (transform.position.y < -10)
			Destroy(gameObject);
	}

	private void OnMouseDrag()
	{
		float distanceToScreen = Camera.main.WorldToScreenPoint(transform.position).z;
		Vector3 newScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
		Vector3 velocity = rigidbody.velocity;
		transform.position = Vector3.SmoothDamp(transform.position, new Vector3(newScreenPosition.x, 2, newScreenPosition.z), ref velocity, 0.3f);
		rigidbody.velocity = velocity;
	}
}
