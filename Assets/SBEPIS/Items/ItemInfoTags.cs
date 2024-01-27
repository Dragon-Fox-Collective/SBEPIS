using System.Collections.Generic;
using KBCore.Refs;
using TMPro;
using UnityEngine;

namespace SBEPIS.Items
{
	public class ItemInfoTags : ValidatedMonoBehaviour
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
			text.text = item ? item.Module.Bits.Tags.ToDelimString() : initialText;
		}
	}
}