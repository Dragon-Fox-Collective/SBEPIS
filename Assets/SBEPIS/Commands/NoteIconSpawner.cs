using System;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Commands
{
	public class NoteIconSpawner : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere]
		private RectTransform notePrefab;

		private List<Transform> notes = new();
		
		public void ClearNotes()
		{
			notes.ForEach(note => Destroy(note.transform.gameObject));
			notes.Clear();
		}

		public void SpawnNote(Note note)
		{
			RectTransform noteObj = Instantiate(notePrefab, transform);
			noteObj.anchorMin = noteObj.anchorMax = new Vector2(0, 0.5f);
			noteObj.anchoredPosition = new Vector2(
				((float)notes.Count).Map(0, 1, 170f, 220f),
				((float)note.Position).Map(Notes.F4.Position, Notes.A4.Position, -10f, 15f));
			
			notes.Add(noteObj);
		}
	}
}
