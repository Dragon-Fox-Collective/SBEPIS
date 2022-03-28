using SBEPIS.Bits;
using UnityEngine;

namespace SBEPIS.Alchemy
{
	public class Totem : MonoBehaviour
	{
		[SerializeField]
		private new SkinnedMeshRenderer renderer;
		[SerializeField]
		private Itemkind defaultItem;

		public BitSet bits { get; set; }

		private void Start()
		{
			if (defaultItem)
			{
				bits = defaultItem.bits;
				for (int i = 0; i < 8; i++)
					SetDepth(i, CaptureCodeUtils.GetCapturePercent(bits, i));

				for (int i = 0; i < 7; i++)
				{
					SetEdgeDistance(i, true, 1);
					SetEdgeDepth(i, true, CaptureCodeUtils.GetCapturePercent(bits, i + 1));
				}
			}
		}

		public void SetDepth(int i, float fraction)
		{
			renderer.SetBlendShapeWeight(renderer.sharedMesh.GetBlendShapeIndex("Carve " + (i + 1)), fraction * 100f);
		}

		public float GetDepth(int i)
		{
			return renderer.GetBlendShapeWeight(renderer.sharedMesh.GetBlendShapeIndex("Carve " + (i + 1))) / 100f;
		}

		public void SetEdgeDistance(int i, bool rightEdge, float fraction)
		{
			fraction = rightEdge ? fraction : 1 - fraction;
			fraction = fraction.Map(0, 1, -1, 1);
			renderer.SetBlendShapeWeight(renderer.sharedMesh.GetBlendShapeIndex("Edge " + (rightEdge ? i + 1 : i)), fraction * 100f);
		}

		public float GetEdgeDistance(int i, bool rightEdge)
		{
			float weight = renderer.GetBlendShapeWeight(renderer.sharedMesh.GetBlendShapeIndex("Edge " + (rightEdge ? i + 1 : i))) / 100f;
			weight = weight.Map(-1, 1, 0, 1);
			weight = rightEdge ? weight : 1 - weight;
			return weight;
		}

		public void SetEdgeDepth(int i, bool rightEdge, float fraction)
		{
			renderer.SetBlendShapeWeight(renderer.sharedMesh.GetBlendShapeIndex("Face " + (rightEdge ? i + 1 : i)), fraction * 100f);
		}

		public float GetEdgeDepth(int i, bool rightEdge)
		{
			return renderer.GetBlendShapeWeight(renderer.sharedMesh.GetBlendShapeIndex("Face " + (rightEdge ? i + 1 : i))) / 100f;
		}
	}
}