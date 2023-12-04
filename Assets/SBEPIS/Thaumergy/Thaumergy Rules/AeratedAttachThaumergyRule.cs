using System.Linq;
using SBEPIS.Bits;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Thaumergy.ThaumergyRules
{
	[CreateAssetMenu(fileName = nameof(AeratedAttachThaumergyRule), menuName = "ThaumergyRules/" + nameof(AeratedAttachThaumergyRule))]
	public class AeratedAttachThaumergyRule : DoOnceThaumaturgyRule
	{
		private Bit aerated;
		
		public void Awake()
		{
			aerated = BitManager.Instance.Bits.First(bit => bit.BitName == "Aerated");
		}
		
		public override bool ApplyOnce(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (!bits.Has(aerated) || !item.AeratedAttachmentPoint)
				return false;
			
			return true;
		}
	}
}