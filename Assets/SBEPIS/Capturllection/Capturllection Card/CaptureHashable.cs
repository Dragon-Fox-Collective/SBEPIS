using SBEPIS.Bits;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class CaptureHashable : MonoBehaviour
	{
		public string captureCode;

		public BitSet bits { get; private set; }

		private void Awake()
		{
			bits = (BitSet)captureCode;
		}
	}
}
