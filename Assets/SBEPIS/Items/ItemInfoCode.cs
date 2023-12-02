using KBCore.Refs;
using SBEPIS.Bits;
using TMPro;
using UnityEngine;

namespace SBEPIS.Items
{
	public class ItemInfoCode : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private TMP_Text text;
		
		[SerializeField]
		private string noItemText = "Place item nearby to debug";
		
		private void Start()
		{
			text.text = noItemText;
		}
		
		public void UpdateText(Item item)
		{
			text.text = item ? BitManager.Instance.Bits.BitSetToCode(item.Module.Bits.Bits) : noItemText;
		}
	}
}