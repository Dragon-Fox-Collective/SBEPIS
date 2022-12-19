using SBEPIS.Bits.Bits;
using SBEPIS.Bits.Tags;
using SBEPIS.Bits.ThaumergeRules;
using SBEPIS.Items;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Bits
{
	public static class Thaumerger
	{
		private static readonly ThaumergeRule[] Rules = {
			new BaseModelReplaceThaumergeRule(),
			new AeratedAttachThaumergeRule(),
			new DefaultReplaceThaumergeRule(),
			new MaterialThaumergeRule(),
		};

		public static Item Thaumerge(TaggedBitSet bits, ItemBaseManager bases)
		{
			Item item = Object.Instantiate(bases.trueBase);

			foreach (ThaumergeRule rule in Rules)
				rule.Init();

			while (true)
			{
				foreach (ThaumergeRule rule in Rules)
					if (rule.Apply(bits, item.itemBase, bases))
						goto loop;
				break;
			loop:;
			}

			return item;
		}
	}
}

namespace SBEPIS.Bits.ThaumergeRules
{
	public abstract class ThaumergeRule
	{
		public virtual void Init() { }
		public abstract bool Apply(TaggedBitSet bits, ItemBase item, ItemBaseManager bases);
	}

	public abstract class DoOnceThaumaturgeRule : ThaumergeRule
	{
		private bool applied;

		public override void Init() => applied = false;

		public override bool Apply(TaggedBitSet bits, ItemBase item, ItemBaseManager bases)
		{
			if (applied)
				return false;
			applied = true;
			return ApplyOnce(bits, item, bases);
		}

		public abstract bool ApplyOnce(TaggedBitSet bits, ItemBase item, ItemBaseManager bases);
	}

	public class BaseModelReplaceThaumergeRule : DoOnceThaumaturgeRule
	{
		public override bool ApplyOnce(TaggedBitSet bits, ItemBase item, ItemBaseManager bases)
		{
			if (item.bits.tags.Any(member => member is BaseModelTag))
				return false;

			if (bits.tags.FirstOrDefault(member => member is BaseModelTag) is not BaseModelTag tag)
				return false;

			ItemBase module = Object.Instantiate(tag.itemBase);

			module.transform.Replace(item.replaceObject);
			item.replaceObject = module.replaceObject;

			item.bits |= module.bits.bits;
			item.bits += tag;

			return true;
		}
	}

	public class AeratedAttachThaumergeRule : DoOnceThaumaturgeRule
	{
		public override bool ApplyOnce(TaggedBitSet bits, ItemBase item, ItemBaseManager bases)
		{
			if (!bits.Has(Bits1.Aerated) || !item.aeratedAttachmentPoint)
				return false;

			return true;
		}
	}

	public class DefaultReplaceThaumergeRule : ThaumergeRule
	{
		public override bool Apply(TaggedBitSet bits, ItemBase item, ItemBaseManager bases)
		{
			if (bits.bits == item.bits.bits)
				return false;

			ItemBase module = null;
			int moduleScore = int.MinValue;
			foreach (ItemBase newModule in bases.itemBases)
			{
				if (bits.Has(newModule.bits.bits) && !item.bits.Has(newModule.bits.bits))
				{
					int newModuleScore = BitSet.GetUniquenessScore(item.bits.bits, newModule.bits.bits);
					if (newModuleScore > moduleScore)
					{
						module = newModule;
						moduleScore = newModuleScore;
					}
				}
			}

			if (module is null)
				return false;

			module = Object.Instantiate(module);

			module.transform.Replace(item.replaceObject);
			item.replaceObject = module.replaceObject;

			item.bits |= module.bits.bits;

			return true;
		}
	}

	public class MaterialThaumergeRule : DoOnceThaumaturgeRule
	{
		public override bool ApplyOnce(TaggedBitSet bits, ItemBase item, ItemBaseManager bases)
		{
			if (item.bits.tags.Any(member => member is MaterialTag))
				return false;

			if (bits.tags.FirstOrDefault(member => member is MaterialTag) is not MaterialTag tag)
				return false;

			foreach (Renderer renderer in item.GetComponentsInChildren<Renderer>())
				renderer.materials = new Material[renderer.materials.Length].Fill(tag.material);

			item.bits += tag;

			return true;
		}
	}
}
