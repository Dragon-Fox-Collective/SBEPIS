using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBEPIS.Bits;

namespace SBEPIS.Items
{
	[CreateAssetMenu(menuName = "Bits/Rules List")]
	public class RulesList : ScriptableObject, IEnumerable<Rule>
	{
		public Rule[] rules = {
			new AeratedAttachRule(),
			new DefaultReplaceRule(),
		};

		public IEnumerator<Rule> GetEnumerator()
		{
			return rules.Cast<Rule>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public abstract class Rule
	{
		public BitSet requiredBits;

		public abstract void Apply(ItemBase itemBase, ItemBase module);
	}

	public class AeratedAttachRule : Rule
	{
		public AeratedAttachRule()
		{
			requiredBits = LeastBits.Aerated;
		}

		public override void Apply(ItemBase itemBase, ItemBase module)
		{
			module.transform.parent = itemBase.aeratedAttachmentPoint;
			module.transform.localPosition = Vector3.zero;
			module.transform.localRotation = Quaternion.identity;
			module.transform.localScale = Vector3.one;

			itemBase.aeratedAttachmentPoint = module.aeratedAttachmentPoint;
		}
	}

	public class DefaultReplaceRule : Rule
	{
		public DefaultReplaceRule()
		{
			requiredBits = BitSet.Nothing;
		}

		public override void Apply(ItemBase itemBase, ItemBase module)
		{
			module.transform.parent = itemBase.replaceObject;
			module.transform.localPosition = Vector3.zero;
			module.transform.localRotation = Quaternion.identity;
			module.transform.localScale = Vector3.one;
			module.transform.SetParent(itemBase.replaceObject.parent, true);

			UnityEngine.Object.Destroy(itemBase.replaceObject.gameObject);
			itemBase.replaceObject = module.replaceObject;
		}
	}
}
