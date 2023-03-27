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
		
		private List<Texture2D> _textures;
		public List<Texture2D> textures
		{
			get => _textures;
			set
			{
				_textures = value;
				
				if (_textures == null)
					ResetTextures();
				else
					UpdateTextures();
			}
		}
		
		private void UpdateTextures()
		{
			Texture2D firstTexture = textures[0];
			Texture2DArray textureArray = new(firstTexture.width, firstTexture.height, textures.Count, firstTexture.format, firstTexture.mipmapCount > 1);
			for (int i = 0; i < textures.Count; i++)
				Graphics.CopyTexture(textures[^(i+1)], 0, textureArray, i);
			
			renderers.PerformOnMaterial(material, material =>
			{
				material.SetTexture(Textures, textureArray);
				material.SetFloat(NumTextures, textures.Count);
			});
		}

		private void ResetTextures()
		{
			renderers.PerformOnMaterial(material, material =>
			{
				material.SetFloat(NumTextures, 0);
			});
		}
	}
}