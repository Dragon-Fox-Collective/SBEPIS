using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class CaptureFront : MonoBehaviour
	{
		[SerializeField] private Material captureMaterial;
		[SerializeField] private Renderer[] renderers;
		
		private Texture2D texture;
		private bool showing = true;
		
		private static readonly int BaseMap = Shader.PropertyToID("_Base_Map");
		
		public void UpdateImage(CaptureContainer card, Capturellectable item)
		{
			texture = CaptureCamera.Instance.TakePictureOfObject(item.gameObject);
			if (showing) SetMaterialTexture(texture);
		}
		
		public void RemoveImage()
		{
			texture = null;
			SetMaterialTexture(null);
		}
		
		public void ShowImage()
		{
			showing = true;
			SetMaterialTexture(texture);
		}
		
		public void HideImage()
		{
			showing = false;
			SetMaterialTexture(null);
		}
		
		private void SetMaterialTexture(Texture2D texture) => renderers.PerformOnMaterial(captureMaterial, material => material.SetTexture(BaseMap, texture));
	}
}