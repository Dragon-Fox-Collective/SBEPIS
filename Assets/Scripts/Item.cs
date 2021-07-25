using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
	public ItemType itemType; // FIXME: Double mobius referencial
	public Quaternion captchaRotation = Quaternion.identity;
	public float captchaScale = 1f;
	public CaptchaEvent preCaptcha, postCaptcha;

	public bool isMouseDown { get; set; }
	public float holdDistance { get; set; }
	public new Rigidbody rigidbody { get; private set; }

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		holdDistance = 2;
	}

	private void Update()
	{
		if (transform.position.y < -10)
			Destroy(gameObject);
	}

	public virtual void OnMouseDown()
	{
		isMouseDown = true;

		SetLayerRecursively(gameObject, LayerMask.NameToLayer("HeldItem"));
		rigidbody.useGravity = false;
	}

	public virtual void OnMouseDrag()
	{
		Vector3 velocity = rigidbody.velocity;
		transform.position = Vector3.SmoothDamp(transform.position, GameManager.instance.player.camera.transform.position + GameManager.instance.player.camera.transform.forward * holdDistance, ref velocity, 0.1f);
		rigidbody.velocity = velocity;
	}

	public virtual void OnMouseUp()
	{
		isMouseDown = false;

		SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default"));
		rigidbody.useGravity = true;
	}

	public static void SetLayerRecursively(GameObject gameObject, int layer)
	{
		gameObject.layer = layer;
		foreach (Transform child in gameObject.transform)
			SetLayerRecursively(child.gameObject, layer);
	}
}

[Serializable]
public class CaptchaEvent : UnityEvent { }