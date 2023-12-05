using SBEPIS.Bits;
using SBEPIS.Items;

namespace SBEPIS.Thaumergy.ThaumergyRules
{
	public abstract class DoOnceThaumergyRule : ThaumergyRule
	{
		private bool applied;

		public override void Init() => applied = false;

		public override bool Apply(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (applied)
				return false;
			applied = true;
			return ApplyOnce(bits, item, modules);
		}

		public abstract bool ApplyOnce(TaggedBitSet bits, ItemModule item, ItemModuleManager modules);
	}
}