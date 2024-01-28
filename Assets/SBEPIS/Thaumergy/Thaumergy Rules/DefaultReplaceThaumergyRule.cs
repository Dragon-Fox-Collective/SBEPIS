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
			
			ItemModule modulePrefab = GetModulePrefabFromScore(bits.Bits, item, modules);
			
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
		
		private static ItemModule GetModulePrefabFromScore(BitSet bits, ItemModule item, ItemModuleManager modules) =>
			modules.Modules
				.Where(module => bits.Has(module.Bits.Bits) && !item.Bits.Has(module.Bits.Bits))
				.MaxBy(module => BitSet.GetUniquenessScore(item.Bits.Bits, module.Bits.Bits), orDefault: true);
		
		private static void PlaceModuleUnderItem(ItemModule item, ItemModule module)
		{
			module.transform.Replace(item.ReplaceObject);
			item.ReplaceObject = module.ReplaceObject;
			item.AeratedAttachmentPoint = module.AeratedAttachmentPoint;
			item.Bits |= module.Bits.Bits;
		}
	}
}