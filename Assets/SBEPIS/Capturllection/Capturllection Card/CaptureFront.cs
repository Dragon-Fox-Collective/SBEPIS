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

		public void UpdateImage(Item item)
		{
			Punchable.PerformOnMaterial(renderers, captureMaterial, material => {
				material.SetTexture("_Base_Map", CaptureCamera.instance.TakePictureOfObject(item.gameObject));
			});
		}

		public void RemoveImage()
		{
			Punchable.PerformOnMaterial(renderers, captureMaterial, material => {
				material.SetTexture("_Base_Map", null);
			});
		}
	}
}
