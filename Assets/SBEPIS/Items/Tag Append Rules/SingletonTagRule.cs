using System.Collections.Generic;
using System.Linq;

namespace SBEPIS.Bits.TagAppendRules
{
	public abstract class SingletonTagRule<T> : TagAppendRule where T : Tag
	{
		public override void Apply(BitSet aBits, List<Tag> aTags, BitSet bBits, List<Tag> bTags, BitSet resultBits, List<Tag> resultTags)
		{
			T tag = aTags.Concat(bTags).OfType<T>().FirstOrDefault(tag => Predicate(aBits, aTags, bBits, bTags, resultBits, resultTags, tag));
			aTags.RemoveAll(member => member is T);
			bTags.RemoveAll(member => member is T);
			if (tag != null)
				resultTags.Add(tag);
		}

		protected virtual bool Predicate(BitSet aBits, List<Tag> aTags, BitSet bBits, List<Tag> bTags, BitSet resultBits, List<Tag> resultTags, T tag) => true;
	}
}