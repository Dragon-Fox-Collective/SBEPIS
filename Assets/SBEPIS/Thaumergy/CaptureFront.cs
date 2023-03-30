using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class CaptureFront : MonoBehaviour
	{
		public Material captureMaterial;
		public Renderer[] renderers;
		
		public void UpdateImage(Capturellectainer card, Capturellectable item)
		{
			Texture2D texture = CaptureCamera.instance.TakePictureOfObject(item.gameObject);
			renderers.PerformOnMaterial(captureMaterial, material => {
				material.SetTexture("_Base_Map", texture);
			});
		}
		
		public void RemoveImage()
		{
			renderers.PerformOnMaterial(captureMaterial, material => {
				material.SetTexture("_Base_Map", null);
			});
		}
	}
}