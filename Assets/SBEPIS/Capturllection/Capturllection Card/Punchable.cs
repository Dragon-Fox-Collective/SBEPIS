using UnityEngine;

namespace SBEPIS.Thaumaturgy
{
	public class Punchable : MonoBehaviour
	{
		public SkinnedMeshRenderer punchHoles;
		[SerializeField]
		private string defaultCode;

		public long punchedHash { get; private set; }

		private void Start()
		{
			if (defaultCode.Length > 0)
				Punch(CaptureCodeUtils.HashCaptureCode(defaultCode));
		}

		public void Punch(long captchaHash)
		{
			this.punchedHash = captchaHash;

			for (int i = 0; i < 48; i++)
			{
				punchHoles.SetBlendShapeWeight(punchHoles.sharedMesh.GetBlendShapeIndex($"Key {i + 1}"), CaptureCodeUtils.GetCaptureBit(punchedHash, i) ? 100 : 0);

				for (int j = 0; j < i; j++)
				{
					int sharedIndex = punchHoles.sharedMesh.GetBlendShapeIndex($"Key {j + 1} + {i + 1}");
					if (sharedIndex >= 0)
						punchHoles.SetBlendShapeWeight(sharedIndex, CaptureCodeUtils.GetCaptureBit(punchedHash, i) || CaptureCodeUtils.GetCaptureBit(punchedHash, j) ? 100 : 0);
				}
			}
		}
	}
}