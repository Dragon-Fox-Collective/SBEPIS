using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Utils
{
	public class SplitTextureSetup : MonoBehaviour
	{
		public List<Renderer> renderers;
		public Material material;
		
		private static readonly int FallbackTexture = Shader.PropertyToID("_Fallback_Texture");
		private static readonly int Textures = Shader.PropertyToID("_Textures");
		private static readonly int NumTextures = Shader.PropertyToID("_Num_Textures");

		public void UpdateTexture(List<Texture2D> textures)
		{
			Texture2D firstTexture = textures[0];
			Texture2DArray textureArray = new(firstTexture.width, firstTexture.height, textures.Count, firstTexture.format, firstTexture.mipmapCount > 1);
			for (int i = 0; i < textures.Count; i++)
				Graphics.CopyTexture(textures[i], 0, textureArray, i);
			
			renderers.PerformOnMaterial(material, material =>
			{
				material.SetTexture(Textures, textureArray);
				material.SetFloat(NumTextures, textures.Count);
			});
		}

		public void ResetTexture()
		{
			renderers.PerformOnMaterial(material, material =>
			{
				material.SetFloat(NumTextures, 0);
			});
		}
	}
}