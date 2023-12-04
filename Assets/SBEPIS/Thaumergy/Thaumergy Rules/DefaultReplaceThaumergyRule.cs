using SBEPIS.Bits;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Thaumergy.ThaumergyRules
{
	[CreateAssetMenu(fileName = nameof(DefaultReplaceThaumergyRule), menuName = "ThaumergyRules/" + nameof(DefaultReplaceThaumergyRule))]
	public class DefaultReplaceThaumergyRule : ThaumergyRule
	{
		public override bool Apply(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (bits.Bits == item.Bits.Bits)
				return false;
			
			ItemModule modulePrefab = GetModulePrefabFromScore(bits, item, modules);
			
			if (modulePrefab is null)
				return false;
			
			ItemModule module = Object.Instantiate(modulePrefab);
			PlaceModuleUnderItem(item, module);
			
			return true;
		}
		
		private static ItemModule GetModulePrefabFromScore(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			ItemModule module = null;
			int moduleScore = int.MinValue;
			foreach (ItemModule newModule in modules.modules)
			{
				if (bits.Has(newModule.Bits.Bits) && !item.Bits.Has(newModule.Bits.Bits))
				{
					int newModuleScore = BitSet.GetUniquenessScore(item.Bits.Bits, newModule.Bits.Bits);
					if (newModuleScore > moduleScore)
					{
						module = newModule;
						moduleScore = newModuleScore;
					}
				}
			}
			return module;
		}
		
		private static void PlaceModuleUnderItem(ItemModule item, ItemModule module)
		{
			module.transform.Replace(item.replaceObject);
			item.replaceObject = module.replaceObject;
			item.Bits |= module.Bits.Bits;
		}
	}
}