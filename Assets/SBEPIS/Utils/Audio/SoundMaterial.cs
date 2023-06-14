using System.Collections.Generic;
using SBEPIS.Utils.Linq;
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
