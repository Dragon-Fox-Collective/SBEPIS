using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Utils
{
	[Serializable]
	public struct Name
	{
		[FormerlySerializedAs("_space")]
		[SerializeField]
		private Space space;
		public Space Space => space;
	}
}