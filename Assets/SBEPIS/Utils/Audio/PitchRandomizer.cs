using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Utils.Audio
{
	public class PitchRandomizer : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private AudioSource audioSource;
		[SerializeField] private float min = 1;
		[SerializeField] private float max = 1;
		
		private void Start() => audioSource.pitch = Random.Range(min, max);
	}
}
