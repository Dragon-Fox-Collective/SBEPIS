using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class CaptureHashable : MonoBehaviour
	{
		public string captureCode;

		public long captureHash { get; private set; }

		private void Awake()
		{
			captureHash = CaptureCodeUtils.HashCaptureCode(captureCode);
		}
	}
}
