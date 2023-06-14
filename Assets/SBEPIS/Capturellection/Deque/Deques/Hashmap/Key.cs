using KBCore.Refs;
using System.Linq;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class Key : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private CardTarget cardTarget;
		public CardTarget CardTarget => cardTarget;
		
		[SerializeField, Self(Flag.Optional)] private InterfaceRef<KeyHandler>[] handlers;
		
		[SerializeField] private Material cardFaceMaterial;
		
		[SerializeField] private string displayText = "";
		public string DisplayText => displayText;
		
		[SerializeField] private float textureScale = 1;
		
		private static readonly int BaseMap = Shader.PropertyToID("_Base_Map");
		private static readonly int MapScale = Shader.PropertyToID("_Map_Scale");
		
		public void SetupCard(DequeElement card) => card.GetComponentsInChildren<Renderer>().PerformOnMaterial(cardFaceMaterial, material =>
		{
			material.SetTexture(BaseMap, CaptureCamera.GetStringTexture(displayText));
			material.SetFloat(MapScale, textureScale);
		});
		
		public string Handle(string current) => handlers.Aggregate(current, (str, handler) => handler.Value.Handle(str));
	}
}
