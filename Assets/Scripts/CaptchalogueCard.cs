using System;
using UnityEngine;

public class CaptchalogueCard : MonoBehaviour
{
	public Shader iconShader;
	public Shader captchaShader;
	public Renderer[] renderers;
	public SkinnedMeshRenderer holeCaps;

	private bool flipped;
	private Item cardItem;

	public Item heldItem { get; private set; }
	public long punchedHash { get; private set; }

	private void Awake()
	{
		cardItem = GetComponent<Item>();
	}

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
			Captchalogue(collisionItem);
	}

	private void OnMouseDrag()
	{
		if (Input.GetMouseButtonDown(1) && heldItem)
			Eject();

		if (cardItem)
		{
			if (Input.GetMouseButtonDown(2))
				flipped = !flipped;

			Quaternion deriv = QuaternionUtil.AngVelToDeriv(transform.rotation, cardItem.rigidbody.angularVelocity);
			transform.rotation = QuaternionUtil.SmoothDamp(transform.rotation, flipped ? Quaternion.Euler(-90, 0, 180) : Quaternion.Euler(90, 0, 0), ref deriv, 0.2f);
			cardItem.rigidbody.angularVelocity = QuaternionUtil.DerivToAngVel(transform.rotation, deriv);
		}
	}

	public void Captchalogue(Item item)
	{
		if (heldItem)
			Eject();

		heldItem = item;
		item.transform.SetParent(transform);
		item.gameObject.SetActive(false);

		UpdateMaterials(item.itemType.captchaHash, FindObjectOfType<Captcharoid>().Captcha(item));
	}

	public void Eject()
	{
		if (!heldItem)
			return;

		heldItem.transform.SetParent(null);
		heldItem.transform.position = transform.position + Vector3.up;
		heldItem.transform.rotation = heldItem.GetComponent<CaptchalogueCard>() ? Quaternion.Euler(90, 0, 0) : Quaternion.identity;
		heldItem.rigidbody.velocity = Vector3.up * 6 + (cardItem ? cardItem.rigidbody.velocity : Vector3.zero);
		heldItem.rigidbody.angularVelocity = Vector3.zero;
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
			foreach (Material material in renderer.materials)
				if (material.shader == iconShader)
				{
					if (!icon)
						Destroy(material.GetTexture("Icon"));
					material.SetTexture("Icon", icon);
				}
				else if (material.shader == captchaShader)
				{
					material.SetFloat("Seed", seed);
					material.SetTexture("CaptchaCode", ItemType.GetCaptchaTexture(captchaHash));
				}
	}

	public void Punch(long captchaHash)
	{
		this.punchedHash = captchaHash;

		for (int i = 0; i < 48; i++)
			holeCaps.SetBlendShapeWeight(i, Math.Min(punchedHash & (1L << i), 1) * 100);
	}
}
