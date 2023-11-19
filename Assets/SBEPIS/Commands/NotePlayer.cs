using System;
using System.Reflection;
using System.Text.RegularExpressions;
using KBCore.Refs;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Commands
{
	public class NotePlayer : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere]
		private AudioSource notePrefab;
		
		private static Regex actionNameRegex = new("^Play (.*)$");
		
		public void OnPlayNote(CallbackContext context)
		{
			if (!context.performed)
				return;
			
			string note = actionNameRegex.Match(context.action.name).Groups[1].Value;
			PlayNote((Note)typeof(Notes).GetField(note.Replace("#", "S"), BindingFlags.Public | BindingFlags.Static)?.GetValue(null));
		}
		
		public void PlayNote(Note note)
		{
			if (note == null)
				throw new ArgumentNullException(nameof(note));
			
			AudioSource noteSource = Instantiate(notePrefab);
			noteSource.pitch = note.Frequency / Notes.C4.Frequency;
			noteSource.Play();
			Destroy(noteSource.gameObject, noteSource.clip.length * noteSource.pitch);
		}
	}
}
