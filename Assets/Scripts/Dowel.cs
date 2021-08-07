using System;
using UnityEngine;

namespace WrightWay.SBEPIS
{
	public class Dowel : MonoBehaviour
	{
		[SerializeField]
		private new SkinnedMeshRenderer renderer;
		[SerializeField]
		private Itemkind defaultItem;

		public long captchaHash { get; set; }

		private void Start()
		{
			if (defaultItem)
			{
				captchaHash = defaultItem.captchaHash;
				for (int i = 0; i < 8; i++)
				{
					long charIndex = (captchaHash >> 6 * i) & ((1L << 6) - 1);
					SetWidth(i, charIndex / 63f);
				}
			}
		}

		public void SetWidth(int i, float fraction)
		{
			renderer.SetBlendShapeWeight(i, fraction * 100f);
		}

		public float GetWidth(int i)
		{
			return renderer.GetBlendShapeWeight(i) / 100f;
		}
	}
}