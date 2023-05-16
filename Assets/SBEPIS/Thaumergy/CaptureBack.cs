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
		private bool showing = true;
		
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
			if (showing) SetMaterialCode(bits);
		}
		
		public void ResetCaptureCode()
		{
			bits = default;
			SetMaterialCode(default);
		}
		
		public void ShowCode()
		{
			showing = true;
			SetMaterialCode(bits);
		}
		
		public void HideCode()
		{
			showing = false;
			SetMaterialCode(default);
		}
		
		private void SetMaterialCode(BitSet bits) => renderers.PerformOnMaterial(captureMaterial, material => {
			material.SetFloat(Seed, BitManager.instance.Bits.BitSetToSeed(bits));
			material.SetTexture(CaptureCode, CaptureCamera.GetStringTexture(BitManager.instance.Bits.BitSetToCode(bits)));
		});
	}
}