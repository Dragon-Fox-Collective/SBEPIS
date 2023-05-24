using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Utils.Audio
{
	[CreateAssetMenu]
	public class CollisionSoundMaterial : ScriptableObject
	{
		[SerializeField] private List<AudioClip> sounds = new();

		public AudioClip RandomSound => sounds.RandomElement();
	}
}
