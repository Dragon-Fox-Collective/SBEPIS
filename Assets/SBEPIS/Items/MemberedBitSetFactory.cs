using SBEPIS.Bits.Members;
using SBEPIS.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Bits
{
	[Serializable]
	public class MemberedBitSetFactory
	{
		public BitSet bits;
		public ItemBase itemBase;

		public MemberedBitSet Make() => bits.With(new Member[]
				{
					itemBase ? new BaseModelMember(itemBase) : null
				}.Where(member => member != null).ToArray());
	}
}
