using System.Linq;
using SBEPIS.Bits;
using SBEPIS.Bits.Tags;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Thaumergy.ThaumergyRules
{
	[CreateAssetMenu(fileName = nameof(DefaultReplaceThaumergyRule), menuName = "ThaumergyRules/" + nameof(DefaultReplaceThaumergyRule))]
	public class DefaultReplaceThaumergyRule : ThaumergyRule
	{
		private bool didAtLeastOneIteration;
		private bool finished;

		public override void Init()
		{
			didAtLeastOneIteration = false;
			finished = false;
		}

		public override bool Apply(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (item.Bits.Bits.Has(bits.Bits) && item.Bits.Bits != BitSet.Empty)
				finished = true;
			
			if (finished)
				return false;
			
			ItemModule modulePrefab = GetModulePrefabFromScore(bits, item, modules);

			if (modulePrefab is null)
			{
				if (bits.Bits != BitSet.Empty)
					Debug.LogError($"Had to use PGO on partial model {item.Bits.Bits} to cover {bits.Bits - item.Bits.Bits} of {bits.Bits}");
				item.Bits |= bits.Bits;
				finished = true;
				modulePrefab = modules.Modules.Last();
			}

			ItemModule module = Instantiate(modulePrefab);
			PlaceModuleUnderItem(item, module);
            
			if (!didAtLeastOneIteration && !item.Bits.Tags.Any(tag => tag is BaseModelTag) && modulePrefab.Bits.Tags.Any(tag => tag is BaseModelTag))
				item.Bits += modulePrefab.Bits.Tags.First(tag => tag is BaseModelTag);
			
			if (item.Bits.Bits.Has(bits.Bits))
				finished = true;
			didAtLeastOneIteration = true;
			return true;
		}
		
		private static ItemModule GetModulePrefabFromScore(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			ItemModule module = null;
			int moduleScore = int.MinValue;
			foreach (ItemModule newModule in modules.Modules)
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