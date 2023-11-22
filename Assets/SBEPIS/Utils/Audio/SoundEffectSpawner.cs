using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Utils.Audio
{
	public class SoundEffectSpawner : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere]
		private AudioSource audioSourcePrefab;
		
		public void Spawn()
		{
			AudioSource audioSource = Instantiate(audioSourcePrefab, transform);
			audioSource.Play();
			Destroy(audioSource.gameObject, audioSource.clip.length / audioSource.pitch);
		}
	}
}