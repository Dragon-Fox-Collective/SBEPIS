using System.Collections.Generic;
using System.Linq;
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
		
		[TextArea]
		[SerializeField] private string keyboardLayout = "QWERTYUIOP\nASDFGHJKL\n_ZXCVBNM←";
		[SerializeField] private Vector2 spacing = new(0.1f, 0.15f);
		
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
			List<List<string>> positions = keyboardLayout.Split('\n').Select(line => line.Select(ch => ch.ToString()).ToList()).ToList();
			foreach (Key key in keys)
			{
				int lineIndex = positions.FindIndex(line => line.Contains(key.DisplayText));
				List<string> line = positions[lineIndex];
				int chIndex = line.IndexOf(key.DisplayText);
				
				key.transform.localPosition = new Vector3(
					(chIndex - (line.Count - 1) / 2f) * spacing.x,
					(lineIndex - (positions.Count - 1) / 2f) * -spacing.y,
					0);
				
				key.CardTarget.onGrab.AddListener(() => Text = key.Handle(Text));
			}
		}
	}
}
