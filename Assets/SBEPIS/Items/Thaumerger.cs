using SBEPIS.Bits.Bits;
using SBEPIS.Bits.Members;
using SBEPIS.Bits.ThaumergeRules;
using SBEPIS.Interaction;
using SBEPIS.Items;
using System;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Bits
{
	public static class Thaumerger
	{
		private static readonly ThaumergeRule[] rules = {
			new BaseModelReplaceThaumergeRule(),
			new AeratedAttachThaumergeRule(),
			new DefaultReplaceThaumergeRule(),
		};

		public static ItemBase Thaumerge(MemberedBitSet bits, ItemBaseManager bases)
		{
			Item item = UnityEngine.Object.Instantiate(bases.trueBase.gameObject).GetComponent<Item>();

			foreach (ThaumergeRule rule in rules)
				rule.Init();

			while (true)
			{
				foreach (ThaumergeRule rule in rules)
					if (rule.Apply(bits, item, bases))
						goto loop;
				break;
			loop:;
			}

			item.GetComponent<CompoundRigidbody>().Recalculate();

			return item;
		}
	}
}

namespace SBEPIS.Bits.ThaumergeRules
{
	public abstract class ThaumergeRule
	{
		public virtual void Init() { }
		public abstract bool Apply(MemberedBitSet bits, Item item, ItemBaseManager bases);
	}

	public abstract class DoOnceThaumaturgeRule : ThaumergeRule
	{
		protected bool applied;

		public override void Init() => applied = false;

		public override bool Apply(MemberedBitSet bits, Item item, ItemBaseManager bases)
		{
			if (applied)
				return false;

			bool rtn = ApplyOnce(bits, item, bases);
			applied = true;
			return rtn;
		}

		public abstract bool ApplyOnce(MemberedBitSet bits, Item item, ItemBaseManager bases);
	}

	public class BaseModelReplaceThaumergeRule : DoOnceThaumaturgeRule
	{
		public override bool ApplyOnce(MemberedBitSet bits, Item item, ItemBaseManager bases)
		{
			if (item.bits.members.Any(member => member is BaseModelMember))
				return false;

			BaseModelMember member = bits.members.FirstOrDefault(member => member is BaseModelMember) as BaseModelMember;
			if (member is null)
				return false;

			ItemBase module = UnityEngine.Object.Instantiate(member.itemBase.gameObject).GetComponent<ItemBase>();

			module.transform.Replace(item.replaceObject);
			item.replaceObject = module.replaceObject;

			item.bits += member;

			return true;
		}
	}

	public class AeratedAttachThaumergeRule : DoOnceThaumaturgeRule
	{
		public override bool ApplyOnce(MemberedBitSet bits, Item item, ItemBaseManager bases)
		{
			if (!bits.Has(Bits1.Aerated) || !item.aeratedAttachmentPoint)
				return false;

			return true;
		}
	}

	public class DefaultReplaceThaumergeRule : ThaumergeRule
	{
		public override bool Apply(MemberedBitSet bits, Item item, ItemBaseManager bases)
		{
			if (bits.bits == item.bits.bits)
				return false;

			ItemBase module = null;
			int moduleScore = int.MinValue;
			foreach (ItemBase newModule in bases.itemBases)
			{
				if (bits.Has(newModule.bits.bits) && (item.bits.bits & newModule.bits.bits) != newModule.bits.bits)
				{
					int newModuleScore = CaptureCodeUtils.GetUniquenessScore(item.bits.bits, newModule.bits.bits);
					if (newModuleScore > moduleScore)
					{
						module = newModule;
						moduleScore = newModuleScore;
					}
				}
			}

			if (module is null)
				return false;

			module = UnityEngine.Object.Instantiate(module.gameObject).GetComponent<ItemBase>();

			module.transform.Replace(item.replaceObject);
			item.replaceObject = module.replaceObject;

			item.bits |= module.bits.bits;

			return true;
		}
	}
}
