using KBCore.Refs;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class Deque : MonoBehaviour
	{
		[SerializeField, Anywhere] private StorableGroupDefinition definition;
		public StorableGroupDefinition Definition => definition;
		
		private void OnValidate() => this.ValidateRefs();
	}
}
