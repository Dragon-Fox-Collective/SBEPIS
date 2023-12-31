using SBEPIS.Bits;
using UnityEngine;

namespace SBEPIS.Thaumergy
{
	public class Punchable : MonoBehaviour
	{
		public BitSet punchedBits;
		public Material[] materials;
		public Renderer[] renderers;
		
		private void Start()
		{
			Punch(punchedBits);
		}
		
		public void Punch(BitSet bits)
		{
			punchedBits = bits;
			
			for (int i = 0; i < Mathf.Min(48, BitManager.Instance.Bits.NumBits); i++)
				foreach (Material baseMaterial in materials)
				{
					int i1 = i;
					renderers.PerformOnMaterial(baseMaterial, material => material.SetFloat($"_Bit_{i1 + 1}", BitManager.Instance.Bits.BitSetHasBitAt(bits, i1) ? 1 : 0));
				}
		}
	}
}