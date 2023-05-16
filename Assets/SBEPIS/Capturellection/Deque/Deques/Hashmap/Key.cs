using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class Key : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private CardTarget cardTarget;
		public CardTarget CardTarget => cardTarget;
		[SerializeField] private Material cardFaceMaterial;
		[SerializeField] private char ch = 'A';
		[SerializeField] private float textureScale = 1;
		public char Ch => ch;
		
		private static readonly int BaseMap = Shader.PropertyToID("_Base_Map");
		private static readonly int MapScale = Shader.PropertyToID("_Map_Scale");
		
		public void SetupCard(DequeElement card) => card.GetComponentsInChildren<Renderer>().PerformOnMaterial(cardFaceMaterial, material =>
		{
			material.SetTexture(BaseMap, CaptureCamera.GetStringTexture(ch.ToString()));
			material.SetFloat(MapScale, textureScale);
		});
	}
}
