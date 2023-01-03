using UnityEngine;

namespace SBEPIS.Utils
{
	public class Space : ScriptableObject
	{
		[SerializeField]
		private string _spaceName;

		public string spaceName => _spaceName;
	}
}