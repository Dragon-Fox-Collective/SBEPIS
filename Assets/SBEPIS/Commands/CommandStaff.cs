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
		private RectTransform notePrefab;
		
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
			RectTransform noteObj = Instantiate(notePrefab, transform);
			noteObj.anchorMin = noteObj.anchorMax = new Vector2(0, 0.5f);
			noteObj.anchoredPosition = new Vector2(
				((float)notes.Count).Map(0, 1, 170f, 220f),
				((float)note.Position).Map(Notes.F4.Position, Notes.A4.Position, -10f, 15f));
			
			notes.Add((noteObj, note));
		}
	}
}
