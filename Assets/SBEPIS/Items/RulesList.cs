using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Items
{
	[CreateAssetMenu(menuName = "Bits/Rules List")]
	public class RulesList : ScriptableObject
	{
		public Rule[] rules;
	}

	[Serializable]
	public class Rule
    {
		public Bit[] baseObject;
		public Bit[] module;
    }
}
