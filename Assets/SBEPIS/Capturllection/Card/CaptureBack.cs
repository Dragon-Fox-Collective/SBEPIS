using SBEPIS.Bits;
using SBEPIS.Items;
using SBEPIS.Thaumaturgy;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class CaptureBack : MonoBehaviour
	{
		public Material captureMaterial;
		public Renderer[] renderers;

		public void UpdateCaptureCodeToDefault() => UpdateCaptureCode(new BitSet());

		public void UpdateCaptureCode(Capturellectainer card, Capturllectable item) => UpdateCaptureCode(item.bits.bits);

		public void UpdateCaptureCode(BitSet bits)
		{
			renderers.PerformOnMaterial(captureMaterial, material => {
				material.SetFloat("_Seed", BitManager.instance.bits.BitSetToSeed(bits));
				material.SetTexture("_Capture_Code", CaptureCamera.GetCaptureCodeTexture(bits));
			});
		}
	}
}
