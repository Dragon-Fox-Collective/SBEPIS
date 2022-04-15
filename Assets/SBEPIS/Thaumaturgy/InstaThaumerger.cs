using SBEPIS.Bits;
using SBEPIS.Items;
using System;
using UnityEngine;

namespace SBEPIS.Thaumaturgy
{
	public class InstaThaumerger : MonoBehaviour
	{
		public ItemBaseManager itemBases;
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
				if (bits.Has(newItemBase.bits))
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
				foreach (Rule rule in RulesList.instance)
				{
					if (rule.IsApplicable(bits))
					{
						ItemBase module = null;
						int moduleScore = int.MinValue;
						foreach (ItemBase newModule in itemBases)
						{
							if (bits.Has(newModule.bits))
							{
								int newModuleScore = CaptureCodeUtils.GetUniquenessScore(itemBase.bits, newModule.bits);
								if (newModuleScore > moduleScore)
								{
									module = newModule;
									moduleScore = newModuleScore;
								}
							}
						}

						if (module == null)
							throw new InvalidOperationException($"No module for {bits} found");

						module = Instantiate(module.gameObject).GetComponent<ItemBase>();
						module.DestroyForCombining();

						rule.Apply(itemBase, module);

						usedBits |= module.bits;

						if (usedBits == bits)
							break;
					}
				}
			}

			if (usedBits != bits)
				throw new InvalidOperationException($"Leftover bits found for {itemBase}: {usedBits ^ bits}");

			itemBase.rigidbody.Recalculate();
		}
	}
}
