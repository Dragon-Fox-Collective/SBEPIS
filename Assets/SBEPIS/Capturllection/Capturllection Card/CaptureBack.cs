using SBEPIS.Bits;
using System;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Capturllector))]
	public class CaptureBack : MonoBehaviour
	{
		public Material captureMaterial;
		public Renderer[] renderers;

		private Capturllector capture;

		private void Awake()
		{
			capture = GetComponent<Capturllector>();
		}

		private void Start()
		{
			UpdateCaptureCode();
		}

		public void UpdateCaptureCode()
		{
			BitSet bits = capture.capturedItem ? capture.capturedItem.bits : BitSet.NOTHING;
			PerformOnMaterial(renderers, captureMaterial, material => {
				material.SetFloat("Seed", CaptureCodeUtils.GetCaptureSeed(bits));
				material.SetTexture("CaptchaCode", CaptureCamera.GetCaptureCodeTexture(bits));
			});
		}

		public static void PerformOnMaterial(Renderer[] renderers, Material material, Action<Material> action)
		{
			foreach (Renderer renderer in renderers)
				for (int i = 0; i < renderer.materials.Length; i++)
				{
					string materialName = renderer.materials[i].name;
					if (materialName.EndsWith(" (Instance)") && materialName[..^11] == material.name)
						action.Invoke(renderer.materials[i]);
				}
		}
	}
}
