using SBEPIS.Bits.Tags;
using UnityEngine;

namespace SBEPIS.Bits.TagAppendRules
{
	[CreateAssetMenu(fileName = nameof(MaterialSingletonTagRule), menuName = "DoubleTagAppendRules/" + nameof(MaterialSingletonTagRule))]
	public class MaterialSingletonTagRule : SingletonTagRule<MaterialTag> {}
}