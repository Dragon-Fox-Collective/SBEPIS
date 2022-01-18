using System;
using UnityEngine;
using WrightWay.SBEPIS.Util;

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
					SetWidth(i, CaptchaUtil.GetCaptchaPercent(captchaHash, i));
			}
		}

		public void SetWidth(int i, float fraction)
		{
			renderer.SetBlendShapeWeight(renderer.sharedMesh.GetBlendShapeIndex("Carve " + (i + 1)), fraction * 100f);
		}

		public float GetWidth(int i)
		{
			return renderer.GetBlendShapeWeight(renderer.sharedMesh.GetBlendShapeIndex("Carve " + (i + 1))) / 100f;
		}

		public void SetEdge(int i, bool rightEdge, float fraction)
		{
			fraction = fraction.Map(0, 1, -1, 1);
			renderer.SetBlendShapeWeight(renderer.sharedMesh.GetBlendShapeIndex("Edge " + (rightEdge ? i + 1 : i)), (rightEdge ? fraction : 1 - fraction) * 100f);
		}

		public float GetEdge(int i, bool rightEdge)
		{
			float weight = renderer.GetBlendShapeWeight(renderer.sharedMesh.GetBlendShapeIndex("Edge " + (rightEdge ? i + 1 : i))) / 100f;
			return (rightEdge ? weight : 1 - weight).Map(-1, 1, 0, 1);
		}

		public void SetFace(int i, float fraction)
		{
			renderer.SetBlendShapeWeight(renderer.sharedMesh.GetBlendShapeIndex("Face " + (i + 1)), fraction * 100f);
		}

		public float GetFace(int i)
		{
			return renderer.GetBlendShapeWeight(renderer.sharedMesh.GetBlendShapeIndex("Face " + (i + 1))) / 100f;
		}
	}
}