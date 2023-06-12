using KBCore.Refs;
using TMPro;
using UnityEngine;

namespace SBEPIS.Utils
{
	public class CardCounterLabel : ValidatedMonoBehaviour
	{
		[SerializeField, Child] private TMP_Text text;
		
		public void ChangeCount(int count) => text.text = $"↓    Number of cards: {count}    ↑";
	}
}
