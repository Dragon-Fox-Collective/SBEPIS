using KBCore.Refs;
using TMPro;
using UnityEngine;

namespace SBEPIS.Thaumergy
{
	public class BootlegAlchemyModeText : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private TMP_Text text;
		
		[SerializeField, Parent]
		private BootlegAlchemyStation alchemyStation;
		
		private string initialText;
		
		private void Awake()
		{
			initialText = text.text;
		}
		
		private void Start()
		{
			UpdateText();
		}
		
		public void UpdateText() => text.text = initialText + alchemyStation.Mode;
	}
}