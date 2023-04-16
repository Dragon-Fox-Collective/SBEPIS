using SBEPIS.Bits;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class CaptureBack : MonoBehaviour
	{
		public Material captureMaterial;
		public Renderer[] renderers;
		
		public void UpdateCaptureCodeToDefault() => UpdateCaptureCode(new BitSet());
		
		public void UpdateCaptureCode(CaptureContainer card, Capturellectable item) => UpdateCaptureCode(item.GetComponent<ItemModule>().bits.bits);
		
		public void UpdateCaptureCode(BitSet bits)
		{
			renderers.PerformOnMaterial(captureMaterial, material => {
				material.SetFloat("_Seed", BitManager.instance.bits.BitSetToSeed(bits));
				material.SetTexture("_Capture_Code", CaptureCamera.GetCaptureCodeTexture(bits));
			});
		}
	}
}