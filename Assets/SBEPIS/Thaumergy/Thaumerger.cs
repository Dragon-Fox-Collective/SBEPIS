using System.Collections.Generic;
using System.Linq;
using SBEPIS.Bits;
using SBEPIS.Items;
using SBEPIS.Thaumergy.ThaumergyRules;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Thaumergy
{
	[CreateAssetMenu(fileName = nameof(Thaumerger))]
	public class Thaumerger : ScriptableSingleton<Thaumerger>
	{
		[SerializeField]
		private List<ThaumergyRule> rules = new();
		
		public Item Thaumerge(TaggedBitSet bits, ItemModuleManager modules)
		{
			Item item = Instantiate(modules.itemBase);
			InitRules();
			while (IterateRules(bits, item.Module, modules)) { }
			return item;
		}
		
		private void InitRules() => rules.ForEach(rule => rule.Init());
		
		private bool IterateRules(TaggedBitSet bits, ItemModule item, ItemModuleManager modules) =>
			rules.Any(rule => rule.Apply(bits, item, modules));
	}
}

namespace SBEPIS.Thaumergy.ThaumergyRules
{
	public abstract class ThaumergyRule : ScriptableObject
	{
		public virtual void Init() { }
		public abstract bool Apply(TaggedBitSet bits, ItemModule item, ItemModuleManager modules);
	}
}