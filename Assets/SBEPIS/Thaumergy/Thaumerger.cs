using System.Collections.Generic;
using System.Linq;
using SBEPIS.Bits;
using SBEPIS.Bits.ThaumergeRules;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Thaumergy
{
	public static class Thaumerger
	{
		private static readonly ThaumergeRule[] Rules = {
			new BaseModelReplaceThaumergeRule(),
			new AeratedAttachThaumergeRule(),
			new DefaultReplaceThaumergeRule(),
			new MaterialThaumergeRule(),
		};
		
		public static Item Thaumerge(TaggedBitSet bits, ItemModuleManager modules)
		{
			Item item = Object.Instantiate(modules.itemBase);
			ItemModule module = item.itemModule;
			
			foreach (ThaumergeRule rule in Rules)
				rule.Init();

			while (true)
				if (!IterateRules(Rules, bits, module, modules))
					break;
			
			return item;
		}
		
		private static bool IterateRules(IEnumerable<ThaumergeRule> rules, TaggedBitSet bits, ItemModule module, ItemModuleManager modules) =>
			rules.Any(rule => rule.Apply(bits, module, modules));
	}
}
