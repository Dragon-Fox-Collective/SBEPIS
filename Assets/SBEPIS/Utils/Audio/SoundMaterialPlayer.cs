using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Utils.Audio
{
	public class SoundMaterialPlayer : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private AudioSource audioSource;
		[SerializeField, Anywhere] private SoundMaterial material;
		[SerializeField] private AnimationCurve volumeCurve = AnimationCurve.Linear(0, 0, 1, 1);
		
		public void Play(float factor) => audioSource.PlayOneShot(material.RandomSound, volumeCurve.Evaluate(factor));
		public void Play(AudioClip clip, float factor) => audioSource.PlayOneShot(clip, volumeCurve.Evaluate(factor));
	}
}
