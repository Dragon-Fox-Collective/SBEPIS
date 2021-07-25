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

	public Player holdingPlayer { get; private set; }
	public new Rigidbody rigidbody { get; private set; }

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (transform.position.y < -10)
			Destroy(gameObject);
	}

	public virtual void OnPickedUp(Player player)
	{
		holdingPlayer = player;

		SetLayerRecursively(gameObject, LayerMask.NameToLayer("Held Item"));
		rigidbody.useGravity = false;
	}

	public virtual void OnHeld(Player player)
	{
		
	}

	public virtual void OnDropped(Player player)
	{
		holdingPlayer = null;

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