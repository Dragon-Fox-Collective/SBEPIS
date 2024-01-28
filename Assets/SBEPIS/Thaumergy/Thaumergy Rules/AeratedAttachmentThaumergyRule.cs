using SBEPIS.Bits;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Thaumergy.ThaumergyRules
{
	[CreateAssetMenu(fileName = nameof(AeratedAttachmentThaumergyRule), menuName = "ThaumergyRules/" + nameof(AeratedAttachmentThaumergyRule))]
	public class AeratedAttachmentThaumergyRule : ThaumergyRule
	{
		[SerializeField] private Bit aerated;
		[SerializeField] private BitSet bits;
		[SerializeField] private Transform attachment;
		
		public override bool Apply(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (!bits.Has(aerated)) return false;
			if (!bits.Has(this.bits)) return false;
			if (!item.AeratedAttachmentPoint) return false;
			
			Transform module = Instantiate(attachment);
			module.transform.Replace(item.AeratedAttachmentPoint);
			item.AeratedAttachmentPoint = null;
			item.Bits |= this.bits | aerated;
			
			return true;
		}
	}
}