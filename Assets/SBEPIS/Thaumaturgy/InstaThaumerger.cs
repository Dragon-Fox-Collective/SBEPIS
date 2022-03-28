using SBEPIS.Bits;
using SBEPIS.Items;
using System;
using UnityEngine;

namespace SBEPIS.Thaumaturgy
{
	public class InstaThaumerger : MonoBehaviour
	{
		public ItemBaseManager itemBases;
		public RulesList rules;
		public BitSet bits;

		private void Awake()
		{
			
		}

		private void Start()
		{
			Alchemize();
			Destroy(gameObject);
		}

		private void Alchemize()
		{
			ItemBase itemBase = null;
			foreach (ItemBase newItemBase in itemBases)
			{
				if ((bits & newItemBase.bits) == newItemBase.bits)
				{
					itemBase = newItemBase;
					break;
				}
			}

			if (itemBase == null)
				throw new InvalidOperationException($"No item base for {bits} found");

			itemBase = Instantiate(itemBase.gameObject, transform.position, transform.rotation).GetComponent<ItemBase>();
			BitSet usedBits = itemBase.bits;

			if (usedBits != bits)
			{
				foreach (Rule rule in rules)
				{
					if ((bits & rule.requiredBits) == rule.requiredBits)
					{
						ItemBase module = null;
						int moduleScore = int.MinValue;
						foreach (ItemBase newModule in itemBases)
						{
							int newModuleScore = CaptureCodeUtils.GetUniquenessScore(itemBase.bits, newModule.bits);
							if (newModuleScore > moduleScore)
							{
								module = newModule;
								moduleScore = newModuleScore;
							}
						}

						if (module == null)
							throw new InvalidOperationException($"No module for {bits} found");

						module = Instantiate(module.gameObject).GetComponent<ItemBase>();

						rule.Apply(itemBase, module);

						usedBits |= rule.requiredBits;

						if (usedBits == bits)
							break;
					}
				}
			}

			if (usedBits != bits)
				throw new InvalidOperationException($"Leftover bits found for {itemBase}: {usedBits ^ bits}");
		}
	}
}
