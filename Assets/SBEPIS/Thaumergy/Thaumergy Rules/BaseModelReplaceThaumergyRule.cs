using System.Linq;
using SBEPIS.Bits;
using SBEPIS.Bits.Tags;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Thaumergy.ThaumergyRules
{
	[CreateAssetMenu(fileName = nameof(BaseModelReplaceThaumergyRule), menuName = "ThaumergyRules/" + nameof(BaseModelReplaceThaumergyRule))]
	public class BaseModelReplaceThaumergyRule : DoOnceThaumaturgyRule
	{
		public override bool ApplyOnce(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (item.Bits.Tags.Any(member => member is BaseModelTag))
				return false;

			if (bits.Tags.FirstOrDefault(member => member is BaseModelTag) is not BaseModelTag tag)
				return false;

			ItemModule module = Instantiate(tag.ItemModule);

			module.transform.Replace(item.ReplaceObject);
			item.ReplaceObject = module.ReplaceObject;

			item.Bits |= module.Bits.Bits;
			item.Bits += tag;

			return true;
		}
	}
}