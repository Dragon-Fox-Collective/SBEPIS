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

		public void UpdateCaptureCodeToDefault() => UpdateCaptureCode(BitSet.NOTHING);

		public void UpdateCaptureCode(Item item) => UpdateCaptureCode(item.itemBase.bits.bits);

		public void UpdateCaptureCode(BitSet bits)
		{
			Punchable.PerformOnMaterial(renderers, captureMaterial, material => {
				material.SetFloat("_Seed", bits.Seed);
				material.SetTexture("_Capture_Code", CaptureCamera.GetCaptureCodeTexture(bits));
			});
		}
	}
}
