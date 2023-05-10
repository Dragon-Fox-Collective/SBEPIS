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
			InitRules(Rules);
			while (IterateRules(Rules, bits, item.Module, modules)) { }
			Debug.Log($"Thaumerged {item} {item.Module.Bits}");
			return item;
		}
		
		private static void InitRules(IEnumerable<ThaumergeRule> rules) => rules.ForEach(rule => rule.Init());
		
		private static bool IterateRules(IEnumerable<ThaumergeRule> rules, TaggedBitSet bits, ItemModule item, ItemModuleManager modules) =>
			rules.Any(rule => rule.Apply(bits, item, modules));
	}
}
