using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptchalogueCard : MonoBehaviour
{
	public Shader captchaShader;
	public Renderer[] captchaRenderers;
	public SkinnedMeshRenderer holeCaps;

	private Item _item;
	public Item item
	{
		get => _item;
		set
		{
			if (item && value)
				item = null;

			if (value)
			{
				_item = value;
				item.transform.SetParent(transform);
				item.gameObject.SetActive(false);
				itemHash = item.itemType.captchaHash;
			}
			else if (item)
			{
				item.transform.SetParent(null);
				item.transform.position = transform.position + Vector3.up;
				item.GetComponent<Rigidbody>().velocity = Vector3.up * 6 + GetComponent<Rigidbody>().velocity;
				item.gameObject.SetActive(true);
				_item = null;
				itemHash = 0;
			}
		}
	}

	private long _itemHash;
	public long itemHash
	{
		get => _itemHash;
		set
		{
			_itemHash = value;

			for (int i = 0; i < 48; i++)
				holeCaps.SetBlendShapeWeight(i, Math.Min(itemHash & (1L << i), 1) * 100);

			float seed = 0;
			for (int i = 0; i < 8; i++)
				seed += ItemType.unhashCaptchaIndex(itemHash, i);
			foreach (Renderer renderer in captchaRenderers)
				foreach (Material material in renderer.materials)
					if (material.shader == captchaShader)
					{
						material.SetFloat("Seed", seed);
						material.SetTexture("CaptchaCode", ItemType.GetCaptchaTexture(itemHash));
					}
		}
	}

	private void Start()
	{
		itemHash = 0;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!gameObject.activeInHierarchy)
			return;

		Item collisionItem = collision.gameObject.GetComponent<Item>();
		if (itemHash == 0 && !item && collisionItem)
			item = collisionItem;
	}

	private void OnMouseDrag()
	{
		if (Input.GetMouseButtonDown(1) && item)
			item = null;
	}
}
