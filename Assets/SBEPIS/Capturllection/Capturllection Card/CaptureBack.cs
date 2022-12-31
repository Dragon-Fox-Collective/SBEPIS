using SBEPIS.Bits;
using SBEPIS.Thaumaturgy;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Capturllector))]
	public class CaptureBack : MonoBehaviour
	{
		public Material captureMaterial;
		public Renderer[] renderers;

		private Capturllector capture;

		private void Awake()
		{
			capture = GetComponent<Capturllector>();
		}

		private void Start()
		{
			UpdateCaptureCode();
		}

		public void UpdateCaptureCode()
		{
			BitSet bits = capture.capturedItem ? capture.capturedItem.bits : new BitSet();
			Punchable.PerformOnMaterial(renderers, captureMaterial, material => {
				material.SetFloat("Seed", BitManager.instance.bits.BitSetToSeed(bits));
				material.SetTexture("CaptchaCode", CaptureCamera.GetCaptureCodeTexture(bits));
			});
		}
	}
}
