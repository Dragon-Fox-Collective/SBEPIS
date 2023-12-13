using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Commands
{
	public class NoteSoundSpawner : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere]
		private AudioSource notePrefab;
		
		[SerializeField]
		private float rootFrequency = 261.63f;
		
		public void PlayNote(Note note)
		{
			AudioSource noteSource = Instantiate(notePrefab);
			noteSource.pitch = note.Frequency / rootFrequency;
			noteSource.Play();
			Destroy(noteSource.gameObject, noteSource.clip.length / noteSource.pitch);
		}
	}
}
