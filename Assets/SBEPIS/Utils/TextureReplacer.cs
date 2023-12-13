using UnityEngine;

namespace SBEPIS.Utils
{
	public class TextureReplacer : MonoBehaviour
	{
		public Texture2D texture;
		public MeshRenderer[] meshRenderers;

		private void Start()
		{
			foreach (MeshRenderer meshRenderer in meshRenderers)
				meshRenderer.material.mainTexture = texture;
			
			Destroy(this);
		}
	}
}
