using System.Linq;
using KBCore.Refs;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class SplitTextureSetupBoxStartSetter : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private SplitTextureSetup split;
		[SerializeField, Self] private DequeBox dequeBox;
		
		private void Start()
		{
			split.Textures = dequeBox.Deque.Definition.Ruleset.GetBoxTextures().ToList();
		}
	}
}