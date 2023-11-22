using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Commands
{
	public class CommandStaff : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent onStaffOpened;
		[SerializeField]
		private UnityEvent onStaffClosed;
		[SerializeField]
		private UnityEvent<Note> onNoteAdded;
		[SerializeField]
		private UnityEvent onNotesCleared;

		public void OpenStaff()
		{
			gameObject.SetActive(true);
			onStaffOpened.Invoke();
		}
		
		public void CloseStaff()
		{
			gameObject.SetActive(false);
			ClearNotes();
			onStaffClosed.Invoke();
		}

		public void ClearNotes()
		{
			onNotesCleared.Invoke();
		}

		public void AddNote(Note note)
		{
			onNoteAdded.Invoke(note);
		}
	}
}
