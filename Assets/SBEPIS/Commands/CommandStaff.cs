using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SBEPIS.Commands
{
	public class CommandStaff : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere]
		private PlayerInput input;

		[SerializeField, Anywhere]
		private Transform notePrefab;
		
		private List<(Transform transform, Note note)> notes = new();
		
		public void OpenStaff()
		{
			gameObject.SetActive(true);
			input.SwitchCurrentActionMap("Command Staff");
		}
		
		public void CloseStaff()
		{
			gameObject.SetActive(false);
			input.SwitchCurrentActionMap("Gameplay");

			notes.ForEach(note => Destroy(note.transform.gameObject));
			notes.Clear();
		}

		public void AddNote(Note note)
		{
			Transform noteObj = Instantiate(notePrefab, transform);
			notes.Add((noteObj, note));
		}
	}
}
