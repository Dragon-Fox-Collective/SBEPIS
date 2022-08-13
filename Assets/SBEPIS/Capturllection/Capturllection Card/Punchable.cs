using SBEPIS.Bits;
using SBEPIS.Capturllection;
using System;
using UnityEngine;

namespace SBEPIS.Thaumaturgy
{
	public class Punchable : MonoBehaviour
	{
		public BitSet punchedBits;
		public Material[] materials;
		public Renderer[] renderers;

		private void Awake()
		{
			DequeStorable card = GetComponent<DequeStorable>();
			if (card)
				card.storePredicates.Add(() => punchedBits == BitSet.NOTHING);
		}

		private void Start()
		{
			Punch(punchedBits);
		}

		public void Punch(BitSet bits)
		{
			punchedBits = bits;

			for (int i = 0; i < 48; i++)
				foreach (Material material in materials)
					PerformOnMaterial(renderers, material, material => material.SetFloat($"_Bit_{i + 1}", bits.BitAt(i) ? 1 : 0));
		}

		public static void PerformOnMaterial(Renderer[] renderers, Material material, Action<Material> action)
		{
			foreach (Renderer renderer in renderers)
				for (int i = 0; i < renderer.materials.Length; i++)
				{
					string materialName = renderer.materials[i].name;
					if (materialName.EndsWith(" (Instance)") && materialName[..^11] == material.name)
						action.Invoke(renderer.materials[i]);
				}
		}
	}
}