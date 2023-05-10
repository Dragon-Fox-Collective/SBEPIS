using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class CaptureFront : MonoBehaviour
	{
		[SerializeField] private Material captureMaterial;
		[SerializeField] private Renderer[] renderers;
		
		private Texture2D texture;
		
		private static readonly int BaseMap = Shader.PropertyToID("_Base_Map");
		
		public void UpdateImage(CaptureContainer card, Capturellectable item)
		{
			texture = CaptureCamera.Instance.TakePictureOfObject(item.gameObject);
			ShowImage();
		}
		
		public void RemoveImage()
		{
			texture = null;
			HideImage();
		}
		
		public void ShowImage() => SetMaterialTexture(texture);
		public void HideImage() => SetMaterialTexture(null);
		
		private void SetMaterialTexture(Texture2D texture) => renderers.PerformOnMaterial(captureMaterial, material => material.SetTexture(BaseMap, texture));
	}
}