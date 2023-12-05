using SBEPIS.Bits.Tags;
using UnityEngine;

namespace SBEPIS.Bits.TagAppendRules
{
	[CreateAssetMenu(fileName = nameof(BaseModelSingletonTagRule), menuName = "DoubleTagAppendRules/" + nameof(BaseModelSingletonTagRule))]
	public class BaseModelSingletonTagRule : SingletonTagRule<BaseModelTag> {}
}