using System;
using UnityEngine;

namespace SBEPIS.Utils
{
	[Serializable]
	public struct Name
	{
		[SerializeField]
		private Space _space;

		public Space space => _space;
	}
}