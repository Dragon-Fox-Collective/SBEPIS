using SBEPIS.Bits;
using SBEPIS.Items;
using SBEPIS.Thaumaturgy;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Capturllectainer))]
	public class CaptureBack : MonoBehaviour
	{
		public Material captureMaterial;
		public Renderer[] renderers;

		private Capturllectainer capture;

		private void Awake()
		{
			capture = GetComponent<Capturllectainer>();
			capture.onCapture.AddListener(UpdateCaptureCode);
			capture.onRetrieve.AddListener(UpdateCaptureCode);
		}

		private void Start()
		{
			UpdateCaptureCode();
		}

		private void OnDestroy()
		{
			capture.onCapture.RemoveListener(UpdateCaptureCode);
			capture.onRetrieve.RemoveListener(UpdateCaptureCode);
		}

		private void UpdateCaptureCode(Item item) => UpdateCaptureCode();

		public void UpdateCaptureCode()
		{
			BitSet bits = capture.capturedItem ? capture.capturedItem.itemBase.bits.bits : BitSet.NOTHING;
			Punchable.PerformOnMaterial(renderers, captureMaterial, material => {
				material.SetFloat("Seed", bits.Seed);
				material.SetTexture("CaptchaCode", CaptureCamera.GetCaptureCodeTexture(bits));
			});
		}
	}
}
