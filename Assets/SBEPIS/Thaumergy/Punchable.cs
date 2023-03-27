using SBEPIS.Bits;
using SBEPIS.Capturllection;
using UnityEngine;

namespace SBEPIS.Thaumergy
{
	public class Punchable : MonoBehaviour
	{
		public BitSet punchedBits;
		public Material[] materials;
		public Renderer[] renderers;
		
		private void Awake()
		{
			if (TryGetComponent(out Card card))
				card.storePredicates.Add(() => punchedBits.isPerfectlyGeneric);
		}
		
		private void Start()
		{
			Punch(punchedBits);
		}
		
		public void Punch(BitSet bits)
		{
			punchedBits = bits;

			for (int i = 0; i < Mathf.Min(48, BitManager.instance.bits.Count); i++)
				foreach (Material material in materials)
					renderers.PerformOnMaterial(material, material => material.SetFloat($"_Bit_{i + 1}", BitManager.instance.bits.BitSetHasBitAt(bits, i) ? 1 : 0));
		}
	}
}