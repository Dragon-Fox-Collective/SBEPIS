using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Items
{
	[CreateAssetMenu(menuName = "Bits/Bit Arrangement")]
	public class BitArrangement : ScriptableObject
	{
		public Bit[] bits = new Bit[48];
	}
}
