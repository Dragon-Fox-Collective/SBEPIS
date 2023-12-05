using System.Collections.Generic;
using System.Linq;

namespace SBEPIS.Bits.TagAppendRules
{
	public abstract class SingletonTagRule<T> : TagAppendRule where T : Tag
	{
		public override void Apply(BitSet aBits, List<Tag> aTags, BitSet bBits, List<Tag> bTags, BitSet resultBits, List<Tag> resultTags)
		{
			T tag = aTags.OfType<T>().FirstOrDefault() ?? bTags.OfType<T>().FirstOrDefault();
			aTags.RemoveAll(member => member is T);
			bTags.RemoveAll(member => member is T);
			if (tag != null)
				resultTags.Add(tag);
		}
	}
}