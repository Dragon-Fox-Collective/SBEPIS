using System.Linq;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(SplitTextureSetup))]
	public class SplitTextureSetupCardOwnerSetter : MonoBehaviour
	{
		private SplitTextureSetup split;
		
		private void Awake()
		{
			split = GetComponent<SplitTextureSetup>();
		}
		
		public void UpdateTextures(DequeStorable card, Deque deque)
		{
			split.Textures = deque.Inventory.GetCardTextures(card).ToList();
		}
	}
}