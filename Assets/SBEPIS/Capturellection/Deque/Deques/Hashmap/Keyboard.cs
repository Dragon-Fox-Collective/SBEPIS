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
		
		public string Text { get; private set; } = "";
		
		private void Awake()
		{
			textDisplay.text = Text;
			
			foreach (Key key in keys)
				key.CardTarget.onGrab.AddListener(TypeKey(key));
		}
		
		private UnityAction TypeKey(Key key) => () =>
		{
			Text += key.Ch;
			textDisplay.text = Text.Length > 0 ? Text : noTextDisplayString;
			onType.Invoke(Text);
		};
	}
}
