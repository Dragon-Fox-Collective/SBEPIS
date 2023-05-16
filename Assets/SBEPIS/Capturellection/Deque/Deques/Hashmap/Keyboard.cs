using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection.Deques
{
	public class Keyboard : ValidatedMonoBehaviour
	{
		[SerializeField, Child] private TMP_Text textDisplay;
		[SerializeField, Child] private Key[] keys;
		[SerializeField] private string noTextDisplayString = "[name]";
		
		public UnityEvent<string> onType = new();
		
		private string text;
		public string Text
		{
			get => text;
			set
			{
				text = value;
				textDisplay.text = text.Length > 0 ? text : noTextDisplayString;
				onType.Invoke(text);
			}
		}
		
		private void Awake()
		{
			Text = "";
			foreach (Key key in keys)
				key.CardTarget.onGrab.AddListener(() => Text = key.Handle(Text));
		}
	}
}
