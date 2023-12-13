using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Utils.Audio
{
	public class SoundMaterialPlayerSpawner : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere]
		private AudioSource audioSourcePrefab;
		[SerializeField, Anywhere]
		private SoundMaterial material;
		[SerializeField]
		private AnimationCurve volumeCurve = AnimationCurve.Linear(0, 0, 1, 1);
		
		public void Play(float factor)
		{
			AudioSource player = Instantiate(audioSourcePrefab);
			player.transform.position = transform.position;
			AudioClip clip = material.RandomSound;
			player.PlayOneShot(clip, volumeCurve.Evaluate(factor));
			Destroy(player.gameObject, clip.length);
		}
	}
}
