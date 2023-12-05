using System.Linq;
using SBEPIS.Bits;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Thaumergy.ThaumergyRules
{
	[CreateAssetMenu(fileName = nameof(UniqueReplaceThaumergyRule), menuName = "ThaumergyRules/" + nameof(UniqueReplaceThaumergyRule))]
	public class UniqueReplaceThaumergyRule : DoOnceThaumergyRule
	{
		public override bool ApplyOnce(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (item.Bits.Bits != BitSet.Empty)
				return false;
			
			ItemModule modulePrefab = modules.UniqueModules.FirstOrDefault(module => bits.Bits == module.Bits.Bits);

			if (modulePrefab is null)
				return false;

			ItemModule module = Instantiate(modulePrefab);
			module.transform.Replace(item.ReplaceObject);
			item.ReplaceObject = module.ReplaceObject;
			item.Bits |= module.Bits.Bits;
			
			return true;
		}
	}
}