using System.Linq;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(SplitTextureSetup), typeof(DequeBox))]
	public class SplitTextureSetupBoxStartSetter : MonoBehaviour
	{
		private void Start()
		{
			GetComponent<SplitTextureSetup>().Textures = GetComponent<DequeBox>().Deque.definition.ruleset.GetBoxTextures().ToList();
		}
	}
}