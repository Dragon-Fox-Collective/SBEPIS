using System.Linq;
using SBEPIS.Bits;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Thaumergy.ThaumergyRules
{
	[CreateAssetMenu(fileName = nameof(ReplaceObjectDecoratorThaumergyRule), menuName = "ThaumergyRules/" + nameof(ReplaceObjectDecoratorThaumergyRule))]
	public class ReplaceObjectDecoratorThaumergyRule : ThaumergyRule
	{
		[SerializeField] private BitSet bits;
		[SerializeField] private Transform decoration;
		
		public override bool Apply(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (!item.Bits.Bits.Any()) return false;
			if (!bits.Has(this.bits)) return false;
			if (item.Bits.Has(this.bits)) return false;
			if (!item.ReplaceObject) return false;
			
			Instantiate(decoration, item.ReplaceObject, false);
			item.Bits |= this.bits;
			
			return true;
		}
	}
}