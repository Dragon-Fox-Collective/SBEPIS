using System.Linq;
using KBCore.Refs;
using SBEPIS.Utils.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace SBEPIS.Utils.SBEPIS.Utils.Audio
{
	public class CollisionSoundMaterialPlayer : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private SoundMaterial material;
		[SerializeField] private AudioMixerGroup output;
		[SerializeField] private float impulseThreshold = 0.1f;
		
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
		
		private void OnCollisionEnter(Collision collision)
		{
			float impulse = collision.impulse.magnitude;
			if (impulse >= impulseThreshold)
				Play(collision.contacts.Select(contact => contact.point).Average(), impulse);
		}
	}
}