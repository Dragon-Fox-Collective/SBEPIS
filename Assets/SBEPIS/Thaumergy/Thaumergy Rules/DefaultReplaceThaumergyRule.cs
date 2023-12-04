using System.Linq;
using SBEPIS.Bits;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Thaumergy.ThaumergyRules
{
	[CreateAssetMenu(fileName = nameof(DefaultReplaceThaumergyRule), menuName = "ThaumergyRules/" + nameof(DefaultReplaceThaumergyRule))]
	public class DefaultReplaceThaumergyRule : ThaumergyRule
	{
		private bool happenedAtLeastOnce;

		public override void Init() => happenedAtLeastOnce = false;

		public override bool Apply(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (bits.Bits == item.Bits.Bits && happenedAtLeastOnce)
				return false;
			
			ItemModule modulePrefab = bits.Bits == BitSet.Empty
				? modules.modules.Last()
				: GetModulePrefabFromScore(bits, item, modules);
			
			if (modulePrefab is null)
				return false;
			
			ItemModule module = Instantiate(modulePrefab);
			PlaceModuleUnderItem(item, module);

			happenedAtLeastOnce = true;
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
			module.transform.Replace(item.ReplaceObject);
			item.ReplaceObject = module.ReplaceObject;
			item.Bits |= module.Bits.Bits;
		}
	}
}