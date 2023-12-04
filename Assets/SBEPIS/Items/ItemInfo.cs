using KBCore.Refs;
using SBEPIS.Bits;
using TMPro;
using UnityEngine;

namespace SBEPIS.Items
{
	public class ItemInfo : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private TMP_Text text;
		
		private string initialText;
		
		private void Start()
		{
			initialText = text.text;
		}
		
		public void UpdateText(Item item)
		{
			text.text = item ? item.Module.Bits.Bits.Code + item.Module.Bits.Tags.ToDelimString() : initialText;
		}
	}
}