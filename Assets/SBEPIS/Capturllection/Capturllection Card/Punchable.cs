using SBEPIS.Bits;
using UnityEngine;

namespace SBEPIS.Thaumaturgy
{
	public class Punchable : MonoBehaviour
	{
		public SkinnedMeshRenderer punchHoles;
		[SerializeField]
		private string defaultCode;

		public BitSet punchedBits { get; private set; }

		private void Start()
		{
			if (defaultCode.Length > 0)
				Punch((BitSet)defaultCode);
		}

		public void Punch(BitSet bits)
		{
			this.punchedBits = bits;

			for (int i = 0; i < 48; i++)
			{
				punchHoles.SetBlendShapeWeight(punchHoles.sharedMesh.GetBlendShapeIndex($"Key {i + 1}"), CaptureCodeUtils.GetCaptureBit(punchedBits, i) ? 100 : 0);

				for (int j = 0; j < i; j++)
				{
					int sharedIndex = punchHoles.sharedMesh.GetBlendShapeIndex($"Key {j + 1} + {i + 1}");
					if (sharedIndex >= 0)
						punchHoles.SetBlendShapeWeight(sharedIndex, CaptureCodeUtils.GetCaptureBit(punchedBits, i) || CaptureCodeUtils.GetCaptureBit(punchedBits, j) ? 100 : 0);
				}
			}
		}
	}
}