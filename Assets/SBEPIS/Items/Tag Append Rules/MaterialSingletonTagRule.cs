using SBEPIS.Bits.Tags;
using UnityEngine;

namespace SBEPIS.Bits.TagAppendRules
{
	[CreateAssetMenu(fileName = nameof(MaterialSingletonTagRule), menuName = "TagAppendRules/" + nameof(MaterialSingletonTagRule))]
	public class MaterialSingletonTagRule : SingletonTagRule<MaterialTag> {}
}