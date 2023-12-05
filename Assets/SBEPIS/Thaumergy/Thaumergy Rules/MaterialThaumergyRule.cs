using System.Linq;
using SBEPIS.Bits;
using SBEPIS.Bits.Tags;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Thaumergy.ThaumergyRules
{
	[CreateAssetMenu(fileName = nameof(MaterialThaumergyRule), menuName = "ThaumergyRules/" + nameof(MaterialThaumergyRule))]
	public class MaterialThaumergyRule : DoOnceThaumergyRule
	{
		public override bool ApplyOnce(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (item.Bits.Tags.Any(member => member is MaterialTag))
				return false;
			
			if (bits.Tags.FirstOrDefault(member => member is MaterialTag) is not MaterialTag tag)
				return false;
			
			foreach (Renderer renderer in item.GetComponentsInChildren<Renderer>())
				renderer.materials = new Material[renderer.materials.Length].Fill(tag.Material);
			
			item.Bits += tag;
			
			return true;
		}
	}
}
