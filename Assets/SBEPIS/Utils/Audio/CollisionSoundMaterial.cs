using KBCore.Refs;
using UnityEngine;
using UnityEngine.Audio;

namespace SBEPIS.Utils.Audio
{
	public class CollisionSoundMaterial : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private SoundMaterial material;
		[SerializeField] private AudioMixerGroup output;
		
		public void Play(Vector3 position, float volume)
		{
			// From AudioSource.PlayOneShot
			GameObject oneShotGameObject = new("Collision One Shot Audio");
			oneShotGameObject.transform.position = position;
			AudioSource oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
			oneShotAudioSource.clip = material.RandomSound;
			oneShotAudioSource.spatialBlend = 1f;
			oneShotAudioSource.volume = volume;
			oneShotAudioSource.outputAudioMixerGroup = output;
			oneShotAudioSource.Play();
			Destroy(oneShotGameObject, oneShotAudioSource.clip.length / oneShotAudioSource.pitch);
		}
	}
}