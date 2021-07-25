using System;
using UnityEngine;

public class CaptchalogueCard : Item
{
	public Material iconMaterial;
	public Material captchaMaterial;
	public Renderer[] renderers;
	public SkinnedMeshRenderer holeCaps;

	public Item heldItem { get; private set; }
	public long punchedHash { get; private set; }

	private void Start()
	{
		UpdateMaterials(0, null);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!gameObject.activeInHierarchy || !collision.gameObject.activeInHierarchy)
			return;

		Item collisionItem = collision.gameObject.GetComponent<Item>();
		if (punchedHash == 0 && !heldItem && collisionItem)
		{
			if (collisionItem.holdingPlayer)
				collisionItem.holdingPlayer.DropItem();
			Captchalogue(collisionItem);
		}
	}

	public override void OnHeld(Player player)
	{
		base.OnHeld(player);

		if (Input.GetAxis("Mouse ScrollWheel") < 0)
			player.holdDistance = 1;
		else if (Input.GetAxis("Mouse ScrollWheel") > 0)
			player.holdDistance = 2;

		if (Input.GetMouseButtonDown(1) && heldItem)
			Eject();
	}

	public void Captchalogue(Item item)
	{
		if (heldItem)
			Eject();

		heldItem = item;
		item.transform.SetParent(transform);
		item.gameObject.SetActive(false);

		UpdateMaterials(item.itemType.captchaHash, GameManager.instance.captcharoid.Captcha(item));
	}

	public void Eject()
	{
		if (!heldItem)
			return;

		heldItem.transform.SetParent(null);
		heldItem.transform.position = transform.position + transform.forward;
		heldItem.transform.rotation = transform.rotation;
		heldItem.rigidbody.velocity = transform.forward * 6 + rigidbody.velocity;
		heldItem.rigidbody.angularVelocity = Vector3.zero;
		SetLayerRecursively(heldItem.gameObject, LayerMask.NameToLayer("Default"));
		heldItem.gameObject.SetActive(true);
		heldItem = null;

		UpdateMaterials(0, null);
	}

	private void UpdateMaterials(long captchaHash, Texture2D icon)
	{
		float seed = 0;
		if (captchaHash != 0)
			for (int i = 0; i < 8; i++)
				seed += Mathf.Pow(10f, i - 4) * ((captchaHash >> 6 * i) & ((1L << 6) - 1));

		foreach (Renderer renderer in renderers)
			for (int i = 0; i < renderer.materials.Length; i++)
			{
				string materialName = renderer.materials[i].name.Replace(" (Instance)", "");
				if (materialName == iconMaterial.name)
				{
					if (!icon)
						Destroy(renderer.materials[i].mainTexture);
					renderer.materials[i].mainTexture = icon;
				}
				else if (materialName == captchaMaterial.name)
				{
					renderer.materials[i].SetFloat("Seed", seed);
					renderer.materials[i].SetTexture("CaptchaCode", ItemType.GetCaptchaTexture(captchaHash));
				}
			}
	}

	public void Punch(long captchaHash)
	{
		this.punchedHash = captchaHash;

		for (int i = 0; i < 48; i++)
			holeCaps.SetBlendShapeWeight(i, Math.Min(punchedHash & (1L << i), 1) * 100);
	}
}
