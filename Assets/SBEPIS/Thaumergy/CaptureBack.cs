using SBEPIS.Bits;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class CaptureBack : MonoBehaviour
	{
		[SerializeField] private Material captureMaterial;
		[SerializeField] private Renderer[] renderers;
		
		private BitSet bits;
		
		private static readonly int Seed = Shader.PropertyToID("_Seed");
		private static readonly int CaptureCode = Shader.PropertyToID("_Capture_Code");
		
		private void Awake()
		{
			ResetCaptureCode();
		}
		
		public void UpdateCaptureCode(CaptureContainer card, Capturellectable item) => UpdateCaptureCode(item.GetComponent<ItemModule>().Bits.Bits);
		public void UpdateCaptureCode(BitSet bits)
		{
			this.bits = bits;
			ShowCode();
		}
		
		public void ResetCaptureCode() => UpdateCaptureCode(default);
		
		public void ShowCode() => SetMaterialCode(bits);
		public void HideCode() => SetMaterialCode(default);
		
		private void SetMaterialCode(BitSet bits) => renderers.PerformOnMaterial(captureMaterial, material => {
			material.SetFloat(Seed, BitManager.instance.Bits.BitSetToSeed(bits));
			material.SetTexture(CaptureCode, CaptureCamera.GetCaptureCodeTexture(bits));
		});
	}
}