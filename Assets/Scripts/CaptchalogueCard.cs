using System;
using UnityEngine;

namespace WrightWay.SBEPIS
{
	[RequireComponent(typeof(Rigidbody))]
	public class CaptchalogueCard : MonoBehaviour
	{
		public Material iconMaterial;
		public Material captchaMaterial;
		public Renderer[] renderers;
		public SkinnedMeshRenderer holeCaps;

		public Item heldItem { get; private set; }
		public long punchedHash { get; private set; }

		private new Rigidbody rigidbody;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			UpdateMaterials(0, null);
		}

		public void Captchalogue(Item item)
		{
			UpdateMaterials(item.itemkind.captchaHash, GameManager.instance.captcharoid.Captcha(item));
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
						renderer.materials[i].SetTexture("CaptchaCode", Itemkind.GetCaptchaTexture(captchaHash));
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