using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Utils.Audio
{
	public class CollisionSound : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private AudioSource audioSource;
		[SerializeField, Anywhere] private CollisionSoundMaterial material;
		[SerializeField] private float impulseThreshold = 0f;
		[SerializeField] private float impulseVolumeScale = 1f;
		
		private void OnCollisionEnter(Collision collision)
		{
			if (collision.impulse.magnitude >= impulseThreshold)
				audioSource.PlayOneShot(material.RandomSound, collision.impulse.magnitude * impulseVolumeScale);
		}
	}
}
