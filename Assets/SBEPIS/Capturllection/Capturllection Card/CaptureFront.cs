using SBEPIS.Bits;
using SBEPIS.Items;
using SBEPIS.Thaumaturgy;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class CaptureFront : MonoBehaviour
	{
		public Material captureMaterial;
		public Renderer[] renderers;

		public void UpdateImage(Capturllectainer card, Capturllectable item)
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
