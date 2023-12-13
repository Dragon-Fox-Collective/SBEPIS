using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Utils
{
	public class Space : ScriptableObject
	{
		[FormerlySerializedAs("_spaceName")]
		[SerializeField]
		private string spaceName;
		public string SpaceName => spaceName;
	}
}