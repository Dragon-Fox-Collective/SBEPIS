using System;
using UnityEngine;

namespace SBEPIS.Bits
{
	[Serializable]
	public class Bit
	{
		public Bit(string name)
		{
			_name = name;
		}
		
		[SerializeField]
		private string _name;
		public string name => _name;

		public override string ToString() => _name;
	}
}