using SBEPIS.Bits.ThaumergeRules;
using SBEPIS.Items;
using System;

namespace SBEPIS.Bits
{
	public static class Thaumerger
	{
		private static readonly ThaumergeRule[] rules = {
			new AeratedAttachThaumergeRule(),
			new DefaultReplaceThaumergeRule(),
		};

		public static ItemBase Thaumerge(BitSet bits, ItemBaseManager itemBases)
		{
			ItemBase item = null;
			foreach (ItemBase baseItem in itemBases)
			{
				if (bits.Has(baseItem.bits))
				{
					item = baseItem;
					break;
				}
			}

			if (item == null)
				throw new InvalidOperationException($"No item base for {bits} found");

			item = UnityEngine.Object.Instantiate(item.gameObject).GetComponent<ItemBase>();
			BitSet usedBits = item.bits;

			if (usedBits != bits)
			{
				foreach (ThaumergeRule rule in rules)
				{
					if (rule.IsApplicable(bits))
					{
						ItemBase module = null;
						int moduleScore = int.MinValue;
						foreach (ItemBase newModule in itemBases)
						{
							if (bits.Has(newModule.bits))
							{
								int newModuleScore = CaptureCodeUtils.GetUniquenessScore(item.bits, newModule.bits);
								if (newModuleScore > moduleScore)
								{
									module = newModule;
									moduleScore = newModuleScore;
								}
							}
						}

						if (module == null)
							throw new InvalidOperationException($"No module for {bits} found");

						module = UnityEngine.Object.Instantiate(module.gameObject).GetComponent<ItemBase>();
						module.DestroyForCombining();

						rule.Apply(item, module);

						usedBits |= module.bits;

						if (usedBits == bits)
							break;
					}
				}
			}

			if (usedBits != bits)
				throw new InvalidOperationException($"Leftover bits found for {item}: {usedBits ^ bits}");

			item.rigidbody.Recalculate();

			return item;
		}
	}
}

namespace SBEPIS.Bits.ThaumergeRules
{
	public interface ThaumergeRule
	{
		public abstract bool IsApplicable(BitSet totalBits);
		public abstract void Apply(ItemBase itemBase, ItemBase module);
	}

	public class AeratedAttachThaumergeRule : ThaumergeRule
	{
		public bool IsApplicable(BitSet totalBits) => totalBits.Has(WeaponUseBits.Aerated);

		public void Apply(ItemBase itemBase, ItemBase module)
		{
			module.transform.Replace(itemBase.aeratedAttachmentPoint);
			itemBase.aeratedAttachmentPoint = module.aeratedAttachmentPoint;
		}
	}

	public class DefaultReplaceThaumergeRule : ThaumergeRule
	{
		public bool IsApplicable(BitSet totalBits) => true;

		public void Apply(ItemBase itemBase, ItemBase module)
		{
			module.transform.Replace(itemBase.replaceObject);
			itemBase.replaceObject = module.replaceObject;
		}
	}
}
