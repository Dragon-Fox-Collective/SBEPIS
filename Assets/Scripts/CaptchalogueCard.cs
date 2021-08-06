using System;
using UnityEngine;

namespace WrightWay.SBEPIS
{
	[RequireComponent(typeof(Rigidbody))]
	public class CaptchalogueCard : MonoBehaviour
	{
		[SerializeField]
		private Material iconMaterial;
		[SerializeField]
		private Material captchaMaterial;
		[SerializeField]
		private Material colorMaterial;
		[SerializeField]
		private Renderer[] renderers;
		[SerializeField]
		private SkinnedMeshRenderer holeCaps;

		public Item heldItem { get; private set; }
		public long punchedHash { get; private set; }
		public new Rigidbody rigidbody { get; private set; }
		private Color modusColor = Color.red;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			Eject();
		}

		public void Captchalogue(Item item)
		{
			heldItem = item;
			item.gameObject.SetActive(false);
			item.transform.SetParent(transform);
			UpdateMaterials(item.itemkind.captchaHash, GameManager.instance.captcharoid.Captcha(item));
		}

		public void Eject()
		{
			heldItem = null;
			UpdateMaterials(0, null);
		}

		public void UpdateMaterials(Color modusColor)
		{
			this.modusColor = modusColor;
			UpdateMaterials(0, null, modusColor, renderers, null, null, colorMaterial);
		}

		private void UpdateMaterials(long captchaHash, Texture2D icon)
		{
			UpdateMaterials(captchaHash, icon, modusColor, renderers, captchaMaterial, iconMaterial, colorMaterial);
		}

		public static void UpdateMaterials(long captchaHash, Texture2D icon, Color color, Renderer[] renderers, Material captchaMaterial, Material iconMaterial, Material colorMaterial)
		{
			float seed = 0;
			if (captchaHash != 0)
				for (int i = 0; i < 8; i++)
					seed += Mathf.Pow(10f, i - 4) * ((captchaHash >> 6 * i) & ((1L << 6) - 1));

			foreach (Renderer renderer in renderers)
				for (int i = 0; i < renderer.materials.Length; i++)
				{
					string materialName = renderer.materials[i].name.Replace(" (Instance)", "");
					if (iconMaterial && materialName == iconMaterial.name)
					{
						if (renderer.materials[i].HasProperty("Base_Map"))
						{
							if (!icon)
								Destroy(renderer.materials[i].GetTexture("Base_Map"));
							renderer.materials[i].SetTexture("Base_Map", icon);
						}
						else
						{
							if (!icon)
								Destroy(renderer.materials[i].mainTexture);
							renderer.materials[i].mainTexture = icon;
						}
					}
					if (captchaMaterial && materialName == captchaMaterial.name)
					{
						renderer.materials[i].SetFloat("Seed", seed);
						renderer.materials[i].SetTexture("CaptchaCode", Itemkind.GetCaptchaTexture(captchaHash));
					}
					if (colorMaterial && materialName == colorMaterial.name)
					{
						if (renderer.materials[i].HasProperty("Background_Color"))
							renderer.materials[i].SetColor("Background_Color", color);
						else
							renderer.materials[i].color = color;
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
}