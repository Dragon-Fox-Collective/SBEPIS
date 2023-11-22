using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Commands
{
	public class CommandListener : MonoBehaviour
	{
		[SerializeField]
		private Command[] commands = Array.Empty<Command>();

		[SerializeField]
		private UnityEvent<Command> onCommandSent = new();
		
		private List<Note> notes = new();
		
		public void ClearNotes()
		{
			notes.Clear();
		}

		public void AddNote(Note note)
		{
			notes.Add(note);
			CheckPatterns();
		}

		private void CheckPatterns()
		{
			ArraySegment<Note> slice = new(notes.ToArray());
			foreach (Command command in commands)
			{
				if (command.Interpret(slice))
				{
					ClearNotes();
					onCommandSent.Invoke(command);
				}
			}
		}
	}
}
