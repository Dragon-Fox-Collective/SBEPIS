using System.Linq;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(SplitTextureSetup), typeof(DequeBox))]
	public class SplitTextureSetupBoxStartSetter : MonoBehaviour
	{
		private void Start()
		{
			GetComponent<SplitTextureSetup>().textures = GetComponent<DequeBox>().Deque.definition.ruleset.GetBoxTextures().ToList();
		}
	}
}