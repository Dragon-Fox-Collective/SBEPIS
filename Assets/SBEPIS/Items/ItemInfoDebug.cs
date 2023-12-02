using KBCore.Refs;
using TMPro;
using UnityEngine;

namespace SBEPIS.Items
{
	public class ItemInfoDebug : ValidatedMonoBehaviour
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
			text.text = item ? item.ToString() : noItemText;
		}
	}
}