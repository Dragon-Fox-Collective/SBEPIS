using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Utils.Audio
{
	public class DestoryAfterPlayOnAwake : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private AudioSource source;
		
		private void Start()
		{
			Destroy(gameObject, source.clip.length / source.pitch);
		}
	}
}
