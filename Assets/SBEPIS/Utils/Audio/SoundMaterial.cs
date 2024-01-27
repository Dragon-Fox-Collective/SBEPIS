using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Utils.Audio
{
	[CreateAssetMenu]
	public class SoundMaterial : ScriptableObject
	{
		[SerializeField] private List<AudioClip> sounds = new();
		
		public AudioClip RandomSound => sounds.RandomElement();
	}
}
