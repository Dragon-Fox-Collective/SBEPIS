using KBCore.Refs;
using SBEPIS.Bits;
using TMPro;
using UnityEngine;

namespace SBEPIS.Thaumergy
{
	public class BootlegAlchemyResultText : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private TMP_Text text;
		
		[SerializeField, Parent]
		private BootlegAlchemyStation alchemyStation;
		
		private string initialText;
		
		private void Start()
		{
			initialText = text.text;
			UpdateText();
		}

		public void UpdateText()
		{
			if (!alchemyStation.Bits1 && !alchemyStation.Bits2)
				text.text = initialText;
			else if (!alchemyStation.Tags1 && !alchemyStation.Tags2)
				text.text = alchemyStation.Result.Bits.Code;
			else
				text.text = alchemyStation.Result.Bits.Code + alchemyStation.Result.Tags.ToDelimString();
		}
	}
}